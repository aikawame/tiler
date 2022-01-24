using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using JetBrains.Annotations;
using Tiler.Models;
using Window = Tiler.Models.Window;

namespace Tiler.Views;

public partial class EditPage
{
  [UsedImplicitly]
  public ObservableCollection<Window> Windows { get; }

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

    ModernWpf.MessageBox.Show(Properties.Resources.Msg_SettingsSaved);
  }
}
