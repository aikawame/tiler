using System.Collections.Generic;
using System.Windows;
using Tiler.Models;

namespace Tiler.Views;

public partial class EditPage
{
  public List<Screen> Screens { get; }

  public EditPage()
  {
    InitializeComponent();
    DataContext = this;

    Screens = SettingCollection.Load().GetScreens();
    TabControl.ItemsSource = Screens;
  }

  private void Save_Click(object sender, RoutedEventArgs e)
  {
    SettingCollection settingCollection = SettingCollection.Load();
    settingCollection.Screens = Screens;
    settingCollection.Save();

    ModernWpf.MessageBox.Show(Properties.Resources.Msg_SettingsSaved);
  }
}
