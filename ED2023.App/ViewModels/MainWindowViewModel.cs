using System.ComponentModel;
using Avalonia.Controls;
using ED2023.App.Models;
using ED2023.App.Views;

namespace ED2023.App.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public string Greeting => "Welcome to Avalonia!";

    public BindingList<Models.TabItem> Tabs { get; set; } = new() {
        new () {
            Header = "Расписание",
            Content = new ScheduleView()
        },
        new() {
            Header = "Группы", 
            Content = new GroupView()
        },
        new() {
            Header = "Клиенты",
            Content = new ClientView()
        },
        new() {
            Header = "Платежи",
            Content = new PaymentView()
        }
    };
}
