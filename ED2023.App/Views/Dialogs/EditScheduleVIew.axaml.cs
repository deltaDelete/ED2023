using System;
using System.Globalization;
using System.Linq;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ED2023.App.Utils;
using ED2023.App.ViewModels;
using ED2023.Database.Models;

namespace ED2023.App.Views.Dialogs; 

public partial class EditScheduleView : ReactiveWindow<EditScheduleViewModel> {
    public EditScheduleView(Action<EditScheduleView, Schedule?> acceptAction) {
        InitializeComponent();
        ViewModel = new(this, i => acceptAction(this, i));
        SetTimePickers();
    }

    public EditScheduleView(Action<EditScheduleView, Schedule?> acceptAction, Schedule item) {
        InitializeComponent();
        ViewModel = new(this, i => acceptAction(this, i));
        var itemClone = item.Clone();
        itemClone.Course = ViewModel.Courses.FirstOrDefault(it => it.Id == item.Course.Id)!;
        itemClone.Group = ViewModel.Groups.FirstOrDefault(it => it.Id == item.Group.Id)!;
        itemClone.Attendances = item.Attendances;
        ViewModel.Item = itemClone;

        SetTimePickers();
    }

    private void SetTimePickers() {
        StartTimePicker.SelectedTime = (TimeSpan)
            TimeConverter.Convert(ViewModel.Item.Start, typeof(TimeSpan), null, CultureInfo.InvariantCulture);
        EndTimePicker.SelectedTime = (TimeSpan)
            TimeConverter.Convert(ViewModel.Item.End, typeof(TimeSpan), null, CultureInfo.InvariantCulture);
    }

    private static readonly TimeSpanToDateTimeBindingConverter TimeConverter = new();
    
    private void StartTimePickerChanged(object? sender, TimePickerSelectedValueChangedEventArgs e) {
        ViewModel.Item.Start = (DateTimeOffset)(TimeConverter.ConvertBack(e.NewTime, typeof(DateTime), ViewModel.Item.Start,
            CultureInfo.InvariantCulture))!;
    }
    
    private void EndTimePickerChanged(object? sender, TimePickerSelectedValueChangedEventArgs e) {
        ViewModel.Item.End = (DateTimeOffset)(TimeConverter.ConvertBack(e.NewTime, typeof(DateTime), ViewModel.Item.End,
            CultureInfo.InvariantCulture))!;
    }
}