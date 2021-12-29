using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Tiler.Models;
using Window = Tiler.Models.Window;

namespace Tiler
{
  public partial class EditPage
  {
    public ObservableCollection<Window> Windows { get; set; }

    public EditPage()
    {
      InitializeComponent();
      DataContext = this;

      Windows = new ObservableCollection<Window>(SettingCollection.Load().GetCurrentScreen().Windows);
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
      SettingCollection settingCollection = SettingCollection.Load();
      settingCollection.GetCurrentScreen().Windows = Windows.ToList();
      settingCollection.Save();

      ModernWpf.MessageBox.Show("Settings have been saved.");
    }

    private void Window_Closing(object sender, CancelEventArgs e)
    {
      e.Cancel = true;
      ShowInTaskbar = false;
      Hide();
    }
  }
}
