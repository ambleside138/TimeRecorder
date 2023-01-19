using Microsoft.UI.Xaml.Controls;

using TimeRecorderWinUI.ViewModels;

namespace TimeRecorderWinUI.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel
    {
        get;
    }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
    }
}
