using System.ComponentModel;
using Avalonia.Controls;
using ED2023.App.Models;
using ED2023.App.Views;

namespace ED2023.App.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public string Greeting => "Welcome to Avalonia!";

    public BindingList<Models.TabItem> Tabs { get; set; } = new BindingList<Models.TabItem>() {
        new Models.TabItem("Расписание", new ScheduleView()),
        new("Страница 1", new Button() {
            Content = "Клик"
        })
    };
}
