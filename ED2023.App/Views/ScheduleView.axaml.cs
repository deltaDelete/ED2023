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
using ED2023.App.Views.Dialogs;
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
            ViewModel.SelectedRow = ViewModel.Items.FirstOrDefault(it => it.Id == schedule?.Id);
            EditItem(ViewModel.SelectedRow);
        });
        List.RemoveItemCommand = ReactiveCommand.Create<Schedule?>(schedule => {
            ViewModel.SelectedRow = ViewModel.Items.FirstOrDefault(it => it.Id == schedule?.Id);
            RemoveItem(ViewModel.SelectedRow);
        });
    }

    private async void EditItem(Schedule? i) {
        if (i is null) return;
        var window = new EditScheduleView((view, schedule) => {
            if (schedule is null) return;
            var db = DatabaseContext.InstanceFor(this);
            db.Schedules.Attach(i);
            if (i.Attendances is not null) {
                db.Attendances.AttachRange(i.Attendances);
                db.Attendances.UpdateRange(i.Attendances);
            }
            db.Schedules.Update(i);
            db.SaveChanges();
            ViewModel!.ReplaceItem(i, schedule);
        }, i);
        await window.ShowDialog(Application.Current!.MainWindow());
    }

    private async Task NewItem() {
        var window = new EditScheduleView((view, schedule) => {
                if (schedule is null) return;
                var db = DatabaseContext.InstanceFor(this);
                db.Schedules.Attach(schedule);
                db.Schedules.Add(schedule);
                db.SaveChanges();
                ViewModel!.AddLocal(schedule);
            }
        );
        await window.ShowDialog(Application.Current!.MainWindow());
    }

    private async void RemoveItem(Schedule? i) {
        if (i is null) {
            return;
        }

        var mbox = MessageBoxUtils.CreateConfirmMessageBox(
            "Подтверждение",
            $"Вы действительно хотите удалить урок {i.Course.Title} из расписания"
        );
        var result = await mbox.ShowAsPopupAsync(this);
        if (result is not "Да") return;

        DatabaseContext.InstanceFor(this).Schedules.Remove(i);
        await DatabaseContext.InstanceFor(this).SaveChangesAsync();
        ViewModel?.RemoveLocal(i);
    }

    private List<Schedule> DatabaseGetter() {
        return DatabaseContext.InstanceFor(this).Schedules
            .Include(x => x.Group)
            .ThenInclude(x => x.ResponsibleTeacher)
            .Include(x => x.Group)
            .ThenInclude(x => x.Course)
            .Include(x => x.Course)
            .ThenInclude(x => x.Teacher)
            .Include(x => x.Attendances)
            .ThenInclude(x => x.Client)
            .ToList();
    }
}