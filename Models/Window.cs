using System.Windows;

namespace Naorai.Models
{
   public class Window
  {
    public string ProcessName { get; set; }

    public string Title { get; set; }

    public Rect Rect { get; set; }

    public Window()
    {
      ProcessName = "";
      Title = "";
      Rect = new Rect();
    }
  }
}
