using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows;
using System.Xml;

namespace Naorai.Models
{
  public class SettingCollection
  {
    public List<Screen> Screens;

    private static string GetFileName()
    {
      return $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}/Naorai/settings.json";
    }

    public static SettingCollection Load()
    {
      if (File.Exists(GetFileName()) == false) return LoadDefault();

      using (var fs = new FileStream(GetFileName(), FileMode.Open))
      using (var reader = JsonReaderWriterFactory.CreateJsonReader(fs, XmlDictionaryReaderQuotas.Max))
      {
        var serializer = new DataContractJsonSerializer(typeof(SettingCollection));

        return (SettingCollection)serializer.ReadObject(reader);
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

      return Screens.Where(screen => screen.Width == screenWidth && screen.Height == screenHeight).First();
    }

    public void Save()
    {
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
