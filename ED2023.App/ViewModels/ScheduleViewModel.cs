using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Collections;
using DynamicData;
using ED2023.Database.Models;
using Microsoft.Win32;
using ReactiveUI;

namespace ED2023.App.ViewModels;

public class ScheduleViewModel : TableViewModelBase<Schedule> {
    public new AvaloniaList<Schedule> Items {
        get => this._items;
        set {
            this.RaiseAndSetIfChanged(ref this._items, value);
        }
    }

    public AvaloniaList<IGrouping<Group, Schedule>> GroupedItems {
        get => _groupedItems;
        set => this.RaiseAndSetIfChanged(ref _groupedItems, value);
    }

    private AvaloniaList<IGrouping<Group, Schedule>> _groupedItems = new();

    public new Schedule? SelectedRow { get; set; }

    public ScheduleViewModel(Func<List<Schedule>> databaseGetter,
        Dictionary<int, Func<Schedule, object>> orderSelectors,
        Func<Schedule, object> defaultOrderSelector,
        Dictionary<int, Func<string, Func<Schedule, bool>>> filterSelectors,
        Func<string, Func<Schedule, bool>> defaultFilterSelector,
        Action<Schedule?> editItem,
        Func<Task> newItem,
        Action<Schedule?> removeItem) : base(databaseGetter,
        orderSelectors,
        defaultOrderSelector,
        filterSelectors,
        defaultFilterSelector,
        editItem,
        newItem,
        removeItem) {
        this.WhenAnyValue(it => it.Items)
            .DistinctUntilChanged()
            .Subscribe(
                list => this.GroupedItems = new(list.GroupBy(it => it.Group))
            );
    }
    
    public new void RemoveLocal(Schedule arg) {
        Items.Remove(arg);
        _itemsFull.Remove(arg);
        Filtered.Remove(arg);
        foreach (var group in GroupedItems) {
            group.ToList().Remove(arg);
        }
    }
    
    public void ReplaceItem(Schedule prevItem, Schedule newItem) {
        if (Filtered.Contains(prevItem)) {
            Filtered.Replace(prevItem, newItem);
            // var index = Filtered.IndexOf(prevItem);
        }

        if (_itemsFull.Contains(prevItem)) {
            var index = _itemsFull.IndexOf(prevItem);
            _itemsFull[index] = newItem;
        }

        if (Items.Contains(prevItem)) {
            Items.Replace(prevItem, newItem);
        }

        if (GroupedItems.Any(it => it.Key.Id == prevItem.Group.Id)) {
            GroupedItems.FirstOrDefault(it => it.Key.Id == prevItem.Group.Id)?
                .ToList().Remove(prevItem);
            GroupedItems.FirstOrDefault(it => it.Key.Id == newItem.Group.Id)?
                .ToList().Add(newItem);
        }
    }
    
    public void AddLocal(Schedule arg) {
        _itemsFull.Add(arg);
        if (Items.Count < 10) {
            Items.Add(arg);
        }
        GroupedItems.FirstOrDefault(it => it.Key.Id == arg.Group.Id)?
            .ToList().Add(arg);
    }
}