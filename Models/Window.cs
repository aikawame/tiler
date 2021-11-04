using System;
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
  }
}
