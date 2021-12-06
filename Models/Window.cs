using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Reactive.Bindings;

namespace Naorai.Models
{
  [DataContract]
  public class Window
  {
    [DataMember]
    public ReactiveProperty<string> ProcessName { get; set; }

    [DataMember]
    public ReactiveProperty<string> Title { get; set; }

    [DataMember]
    public ReactiveProperty<int> X { get; set; }

    [DataMember]
    public ReactiveProperty<int> Y { get; set; }

    [DataMember]
    public ReactiveProperty<int> Width { get; set;  }

    [DataMember]
    public ReactiveProperty<int> Height { get; set; }

    public IntPtr Handler { get; set; }

    public Window()
    {
      ProcessName = new ReactiveProperty<string>();
      Title = new ReactiveProperty<string>();
      X = new ReactiveProperty<int>();
      Y = new ReactiveProperty<int>();
      Width = new ReactiveProperty<int>();
      Height = new ReactiveProperty<int>();
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
        targetWindows = All().Where(w =>
        {
          return w.ProcessName.Value == ProcessName.Value && w.Title.Value == Title.Value;
        }).ToList();
      }
      else
      {
        targetWindows.Add(this);
      }

      targetWindows.ForEach(w =>
      {
        NativeMethods.MoveWindow(w.Handler, X.Value, Y.Value, Width.Value, Height.Value, false);
      });
    }
  }
}
