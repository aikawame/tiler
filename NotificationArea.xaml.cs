using System.Windows;
using Naorai.Models;
using Window = Naorai.Models.Window;

namespace Naorai
{
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

      MessageBox.Show("All window positions have been saved.");
    }

    private void StoreActive_Click(object sender, RoutedEventArgs e)
    {
      SettingCollection settingCollection = SettingCollection.Load();
      settingCollection.GetCurrentScreen().UpdateWindow(Window.Active());
      settingCollection.Save();

      MessageBox.Show("Active window position has been saved.");
    }

    private void Edit_Click(object sender, RoutedEventArgs e)
    {
      System.Windows.Window editPage = new EditPage();
      editPage.Show();
    }
    private void Exit_Click(object sender, RoutedEventArgs e)
    {
      Application.Current.Shutdown();
    }
  }
}
