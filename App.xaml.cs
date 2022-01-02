using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;

namespace Tiler;

public partial class App
{
  private TaskbarIcon? _taskbarIcon;

  protected override void OnStartup(StartupEventArgs e)
  {
    base.OnStartup(e);
    var taskbarIcon = FindResource("TaskbarIcon");
    if (taskbarIcon is null) return;

    _taskbarIcon = (TaskbarIcon)taskbarIcon;
  }

  protected override void OnExit(ExitEventArgs e)
  {
    if (_taskbarIcon is not null) _taskbarIcon.Dispose();

    base.OnExit(e);
  }
}
