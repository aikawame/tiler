using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows;

namespace Naorai.Models
{
  [DataContract]
  public class Window
  {
    [DataMember]
    public string ProcessName { get; set; }

    [DataMember]
    public string Title { get; set; }

    [DataMember]
    public Rect Rect { get; set; }

    public IntPtr Handler { get; set; }

    public Window()
    {
      ProcessName = "";
      Title = "";
      Rect = new Rect();
    }

    public static Window Active()
    {
      return WindowEnumerator.GetActiveWindow();
    }

    public static List<Window> All()
    {
      return WindowEnumerator.GetAllWindows();
    }

    public void Apply()
    {
      var targetWindows = new List<Window>();
      if (Handler == IntPtr.Zero)
      {
        targetWindows = All().Where(w => w.ProcessName == ProcessName && w.Title == Title).ToList();
      }
      else
      {
        targetWindows.Add(this);
      }

      targetWindows.ForEach(w =>
      {
        NativeMethods.MoveWindow(w.Handler, (int)Rect.X, (int)Rect.Y, (int)Rect.Width, (int)Rect.Height, false);
      });
    }
  }
}
