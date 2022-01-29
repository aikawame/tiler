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
  public string Label { get; set; }

  [DataMember]
  public List<Window> Windows { get; set; }

  public Screen()
  {
    Width   = 0;
    Height  = 0;
    Label   = "";
    Windows = new List<Window>();
  }

  public static Screen Active()
  {
    var screenWidth = (int)SystemParameters.VirtualScreenWidth;
    var screenHeight = (int)SystemParameters.VirtualScreenHeight;

    var screen = new Screen
    {
      Width   = screenWidth,
      Height  = screenHeight,
      Label   = $"{screenWidth} x {screenHeight}",
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
