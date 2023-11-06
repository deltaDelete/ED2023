using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ED2023.App.ViewModels;
using ED2023.Database;
using ED2023.Database.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ReactiveUI;

namespace ED2023.App.Controls;

public partial class ScheduleList : UserControl {
    public ScheduleList() {
        InitializeComponent();
    }


    public AvaloniaList<IGrouping<Group, Schedule>>? Items {
        get => _items;
        set => SetAndRaise(ItemsProperty, ref _items, value);
    }
    
    public ReactiveCommand<Schedule?, Unit> EditItemCommand { get; set; }
    public ReactiveCommand<Schedule?, Unit> RemoveItemCommand { get; set; }

    public static readonly DirectProperty<ScheduleList, AvaloniaList<IGrouping<Group, Schedule>>?> ItemsProperty
        = AvaloniaProperty.RegisterDirect<ScheduleList, AvaloniaList<IGrouping<Group, Schedule>>?>(
            nameof(Items),
            x => x.Items,
            (x, items) => x.Items = items
        );

    private AvaloniaList<IGrouping<Group, Schedule>>? _items;
}