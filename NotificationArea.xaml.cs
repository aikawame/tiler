using System.Windows;

namespace Naorai
{
  public partial class NotificationArea
  {
    private void Edit_Click(object sender, RoutedEventArgs e)
    {
      Window mainWindow = new MainWindow();
      mainWindow.Show();
    }
    private void Exit_Click(object sender, RoutedEventArgs e)
    {
      Application.Current.Shutdown();
    }
  }
}
