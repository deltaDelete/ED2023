using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Collections;
using ED2023.Database.Models;
using ReactiveUI;

namespace ED2023.App.ViewModels; 

public class ScheduleViewModel : TableViewModelBase<Schedule> {
    private AvaloniaList<IGrouping<Group, Schedule>> _items1 = new();

    public new AvaloniaList<IGrouping<Group, Schedule>> Items {
        get => _items1;
        set => this.RaiseAndSetIfChanged(ref _items1, value);
    }

    public new Schedule SelectedRow { get; set; }
    
    public ScheduleViewModel(Func<List<Schedule>> databaseGetter, Dictionary<int, Func<Schedule, object>> orderSelectors, Func<Schedule, object> defaultOrderSelector, Dictionary<int, Func<string, Func<Schedule, bool>>> filterSelectors, Func<string, Func<Schedule, bool>> defaultFilterSelector, Action<Schedule?> editItem, Func<Task> newItem, Action<Schedule?> removeItem) : base(databaseGetter, orderSelectors, defaultOrderSelector, filterSelectors, defaultFilterSelector, editItem, newItem, removeItem) {
        // TODO: Доделать вм для расписания, сделать добавление, удаление, изменение
    }
}