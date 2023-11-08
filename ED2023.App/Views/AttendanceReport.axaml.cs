using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ED2023.App.ViewModels;

namespace ED2023.App.Views; 

public partial class AttendanceReport : ReactiveUserControl<AttendanceReportViewModel> {
    public AttendanceReport() {
        InitializeComponent();
        ViewModel = new();
    }
}