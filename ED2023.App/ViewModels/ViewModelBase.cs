using ED2023.App.Views;
using ReactiveUI;

namespace ED2023.App.ViewModels;

public class ViewModelBase : ReactiveObject {
    private static MainWindow MainWindow => (App.Current as App).MainWindow;
}