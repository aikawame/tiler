using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml.Serialization;

namespace Naorai.Models
{
  public class Display
  {
    public ObservableCollection<Window> Windows = new ObservableCollection<Window>();

    private string GetSettingFileName()
    {
      string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
      double width = SystemParameters.VirtualScreenWidth;
      double height = SystemParameters.VirtualScreenHeight;

      return $"{appDataPath}/Naorai/Settings_{width}x{height}.xml";
    }

    public void Load()
    {
      WindowManager windowManager = new WindowManager();
      ObservableCollection<ActiveWindow> activeWindows = windowManager.GetActiveWindows();

      XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Window>));
      StreamReader streamReader = new StreamReader(GetSettingFileName(), new UTF8Encoding(false));
      ObservableCollection<Window> windows = (ObservableCollection<Window>)serializer.Deserialize(streamReader);
      streamReader.Close();

      foreach (ActiveWindow activeWindow in activeWindows)
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
            (int)window.Rect.Y - (int)window.Rect.Height,
            (int)window.Rect.Width,
            (int)window.Rect.Height,
            false
          );
        }
      }
    }

    public void Save()
    {
      WindowManager windowManager = new WindowManager();
      ObservableCollection<Window> windows = windowManager.GetWindows();

      XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Window>));
      StreamWriter streamWriter = new StreamWriter(GetSettingFileName(), false, new UTF8Encoding(false));
      serializer.Serialize(streamWriter, windows);
      streamWriter.Close();
    }

  }
}
