using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Windows;

namespace Tiler.Models;

[DataContract]
public class Screen
{
  [DataMember]
  public int Width { get; set; }

  [DataMember]
  public int Height { get; set; }

  [DataMember]
  public List<Window> Windows { get; set; }

  public Screen()
  {
    Width   = 0;
    Height  = 0;
    Windows = new List<Window>();
  }

  public static Screen Active()
  {
    var screen = new Screen
    {
      Width   = (int)SystemParameters.VirtualScreenWidth,
      Height  = (int)SystemParameters.VirtualScreenHeight,
      Windows = Window.All()
    };

    return screen;
  }

  public void Apply()
  {
    Windows.ForEach(w => w.Apply());
  }

  public void UpdateWindow(Window window)
  {
    int index = Windows.FindIndex(w => w.ProcessName == window.ProcessName && w.Title == window.Title);
    Windows[index] = window;
  }
}
