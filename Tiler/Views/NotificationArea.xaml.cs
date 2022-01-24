using System.Windows;
using Tiler.Models;
using Window = Tiler.Models.Window;

namespace Tiler.Views;

public partial class NotificationArea
{
  private void Restore_Click(object sender, RoutedEventArgs e)
  {
    SettingCollection settingCollection = SettingCollection.Load();
    settingCollection.GetCurrentScreen().Apply();
  }

  private void StoreAll_Click(object sender, RoutedEventArgs e)
  {
    SettingCollection settingCollection = SettingCollection.Load();
    settingCollection.UpdateScreen(Screen.Active());
    settingCollection.Save();

    ModernWpf.MessageBox.Show(Properties.Resources.Msg_AllWindowsStored);
  }

  private void StoreActive_Click(object sender, RoutedEventArgs e)
  {
    SettingCollection settingCollection = SettingCollection.Load();
    settingCollection.GetCurrentScreen().UpdateWindow(Window.Active());
    settingCollection.Save();

    ModernWpf.MessageBox.Show(Properties.Resources.Msg_ActiveWindowStored);
  }

  private void Edit_Click(object sender, RoutedEventArgs e)
  {
    System.Windows.Window mainView = new MainView();
    mainView.Show();
  }
  private void Exit_Click(object sender, RoutedEventArgs e)
  {
    Application.Current.Shutdown();
  }
}
