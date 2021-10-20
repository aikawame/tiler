using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;

namespace Naorai
{
  public partial class App
  {
    private TaskbarIcon _taskbarIcon;

    protected override void OnStartup(StartupEventArgs e)
    {
      _taskbarIcon = (TaskbarIcon)FindResource("TaskbarIcon");
      base.OnStartup(e);
    }
  }
}
