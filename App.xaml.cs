using System;
using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Win32;
using Tiler.Models;

namespace Tiler;

public partial class App
{
  public App()
  {
    SystemEvents.DisplaySettingsChanged += SystemEvents_DisplaySettingsChanged;
  }

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

  private void SystemEvents_DisplaySettingsChanged(object? sender, EventArgs e)
  {
    SettingCollection.Load().GetCurrentScreen().Apply();
  }
}
