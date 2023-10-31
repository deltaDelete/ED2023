using Avalonia.ReactiveUI;
using ED2023.App.ViewModels;

namespace ED2023.App.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
        ViewModel = new MainWindowViewModel();
    }
}