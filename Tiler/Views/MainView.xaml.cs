using System.ComponentModel;
using ui = ModernWpf.Controls;

namespace Tiler.Views;

public partial class MainView
{
  public MainView()
  {
    InitializeComponent();
    DataContext = this;

    Title = "";
  }

  private void NaviView_SelectionChanged(ui.NavigationView sender, ui.NavigationViewSelectionChangedEventArgs args)
  {
    var selectedItem = (ui.NavigationViewItem)args.SelectedItem;
    Title = selectedItem.Content.ToString();
    string? tag = selectedItem.Tag?.ToString();
    switch (tag)
    {
      case "edit":
        ContentFrame.Navigate(typeof(EditPage));
        break;
      case "others":
        ContentFrame.Navigate(typeof(OthersPage));
        break;
      default:
        ContentFrame.Navigate(typeof(AboutPage));
        break;
    }
  }

  private void Window_Closing(object sender, CancelEventArgs e)
  {
    e.Cancel = true;
    ShowInTaskbar = false;
    Hide();
  }
}
