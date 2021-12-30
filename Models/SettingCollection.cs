using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows;
using System.Xml;

namespace Tiler.Models
{
  public class SettingCollection
  {
    public List<Screen> Screens;

    public SettingCollection()
    {
      Screens = new List<Screen>();
    }

    private static string GetFileName()
    {
      return $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}/Tiler/settings.json";
    }

    public static SettingCollection Load()
    {
      if (File.Exists(GetFileName()) == false) return LoadDefault();

      using (var fs = new FileStream(GetFileName(), FileMode.Open))
      using (var reader = JsonReaderWriterFactory.CreateJsonReader(fs, XmlDictionaryReaderQuotas.Max))
      {
        var serializer = new DataContractJsonSerializer(typeof(SettingCollection));
        var settingCollection = serializer.ReadObject(reader);
        if (settingCollection is null) return LoadDefault();

        return (SettingCollection)settingCollection;
      }
    }

    public static SettingCollection LoadDefault()
    {
      var settingCollection = new SettingCollection();
      var screens = new List<Screen>();
      screens.Add(Screen.Active());
      settingCollection.Screens = screens;

      return settingCollection;
    }

    public Screen GetCurrentScreen()
    {
      var screenWidth = (int)SystemParameters.VirtualScreenWidth;
      var screenHeight = (int)SystemParameters.VirtualScreenHeight;

      var screens = Screens.Where(screen => screen.Width == screenWidth && screen.Height == screenHeight).ToList();

      if (screens.Any() == false)
      {
        var currentScreen = Screen.Active();
        Screens.Add(currentScreen);

        return currentScreen;
      }
      return screens.First();
    }

    public void Save()
    {
      var directory = Path.GetDirectoryName(GetFileName()) ?? "";
      if (Directory.Exists(directory) == false)
      {
        Directory.CreateDirectory(directory);
      }
      using (var fs = new FileStream(GetFileName(), FileMode.Create))
      using (var writer = JsonReaderWriterFactory.CreateJsonWriter(fs, Encoding.UTF8, true, true, "  "))
      {
        var serializer = new DataContractJsonSerializer(typeof(SettingCollection));
        serializer.WriteObject(writer, this);
      }
    }

    public void UpdateScreen(Screen screen)
    {
      int index = Screens.FindIndex(s => s.Width == screen.Width && s.Height == screen.Height);
      Screens[index] = screen;
    }
  }
}
