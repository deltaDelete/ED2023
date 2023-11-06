using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ED2023.App.Utils;
using ED2023.App.ViewModels;
using ED2023.Database;
using ED2023.Database.Models;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;

namespace ED2023.App.Views; 

public partial class ScheduleView : ReactiveUserControl<ScheduleViewModel> {
    public ScheduleView() {
        InitializeComponent();
        ViewModel = new(
            DatabaseGetter,
            new(),
            grouping => null,
            new(),
            grouping => it => true,
            EditItem,
            NewItem,
            RemoveItem
            );
        List.EditItemCommand = ReactiveCommand.Create<Schedule?>(schedule => {
            ViewModel.SelectedRow = ViewModel.Items.FirstOrDefault(it => it.Key.Id == schedule.Course.Id, null)
                                             .FirstOrDefault(it => it.Id == schedule.Id);
            ViewModel.EditItemCommand.Execute();
        });
        List.RemoveItemCommand = ReactiveCommand.Create<Schedule?>(schedule => {
            ViewModel.SelectedRow = ViewModel.Items.FirstOrDefault(it => it.Key.Id == schedule.Course.Id, null)
                                             .FirstOrDefault(it => it.Id == schedule.Id);
            ViewModel.RemoveItemCommand.Execute();
        });
    }

    private void EditItem(Schedule? i) {
        throw new NotImplementedException();
    }

    private Task NewItem() {
        throw new NotImplementedException();
    }

    private void RemoveItem(Schedule? i) {
        if (i is null) {
            return;
        }

        var mbox = MessageBoxUtils.CreateConfirmMessageBox(
            "Подтверждение",
            $"Вы действительно хотите удалить урок {i.Course.Title} из расписания"
        );
        var result = await mbox.ShowAsPopupAsync(this);
        if (result is not "Да") return;

        DatabaseContext.InstanceFor(this).Clients.Remove(i);
        await DatabaseContext.InstanceFor(this).SaveChangesAsync();
        ViewModel?.RemoveLocal(i);
    }

    private List<IGrouping<Group, Schedule>> DatabaseGetter() {
        return DatabaseContext.InstanceFor(this).Schedules
            .Include(x => x.Group)
            .Include(x => x.Course)
            .GroupBy(x => x.Group)
            .ToList();
    }
}