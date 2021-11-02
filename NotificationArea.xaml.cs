using System.Windows;
using Naorai.Models;

namespace Naorai
{
  public partial class NotificationArea
  {
    private void Restore_Click(object sender, RoutedEventArgs e)
    {
      Display display = new Display();
      display.Load();
    }

    private void StoreAll_Click(object sender, RoutedEventArgs e)
    {
      Display display = new Display();
      display.Save();
      MessageBox.Show("All window positions have been saved.");
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
