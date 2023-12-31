using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ED2023.App.ViewModels;
using ED2023.App.Views;

namespace ED2023.App;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }
}

public static class ApplicationExtensions {
    public static MainWindow MainWindow(this Application app) {
        return ((app.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)!.MainWindow! as MainWindow)!;
    }
}