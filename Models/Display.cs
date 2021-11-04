using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows;
using System.Xml;

namespace Naorai.Models
{
  public class Display
  {
    private string GetSettingFileName()
    {
      string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
      double width = SystemParameters.VirtualScreenWidth;
      double height = SystemParameters.VirtualScreenHeight;

      return $"{appDataPath}/Naorai/Settings_{width}x{height}.json";
    }

    public void Load()
    {
      if (File.Exists(GetSettingFileName()) == false) Save();

      WindowManager windowManager = new WindowManager();
      ObservableCollection<Window> activeWindows = windowManager.GetWindows();

      using (FileStream fs = new FileStream(GetSettingFileName(), FileMode.Open))
      using (var reader = JsonReaderWriterFactory.CreateJsonReader(fs, XmlDictionaryReaderQuotas.Max))
      {
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ObservableCollection<Window>));
        ObservableCollection<Window> windows = (ObservableCollection<Window>)serializer.ReadObject(reader);

        foreach (Window activeWindow in activeWindows)
        {
          IEnumerable<Window> windowsWhere = windows.Where(o =>
          {
            return o.ProcessName == activeWindow.ProcessName && o.Title == activeWindow.Title;
          });
          foreach (Window window in windowsWhere)
          {
            Debug.Print(window.Title);
            NativeMethods.MoveWindow(
              activeWindow.Handler,
              (int)window.Rect.X,
              (int)window.Rect.Y,
              (int)window.Rect.Width,
              (int)window.Rect.Height,
              false
            );
          }
        }
      }
    }

    public void Save()
    {
      WindowManager windowManager = new WindowManager();
      ObservableCollection<Window> windows = windowManager.GetWindows();

      using (FileStream fs = new FileStream(GetSettingFileName(), FileMode.Create))
      using (var writer = JsonReaderWriterFactory.CreateJsonWriter(fs, Encoding.UTF8, true, true, "  "))
      {
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ObservableCollection<Window>));
        serializer.WriteObject(writer, windows);
      }
    }
  }
}
