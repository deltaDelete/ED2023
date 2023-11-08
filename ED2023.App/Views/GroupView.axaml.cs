using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Logging;
using Avalonia.ReactiveUI;
using ED2023.App.ViewModels;
using ED2023.App.Views.Dialogs;
using ED2023.Database;
using ED2023.Database.Models;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia.Models;
using Splat;

namespace ED2023.App.Views;

public partial class GroupView : ReactiveUserControl<TableViewModelBase<Group>>, IEnableLogger {
    public GroupView() {
        InitializeComponent();
        ViewModel = new TableViewModelBase<Group>(
            DatabaseGetter,
            OrderSelectors,
            DefaultOrderSelector,
            FilterSelectors,
            DefaultFilterSelector,
            EditItem,
            NewItem,
            RemoveItem
        );
    }

    private async void RemoveItem(Group? i) {
        if (i is null) {
            return;
        }

        var mbox = MessageBoxManager.GetMessageBoxCustom(
            new() {
                ButtonDefinitions = new ButtonDefinition[] {
                    new() { Name = "Да", IsDefault = true },
                    new() { Name = "Нет", IsCancel = true }
                },
                ContentTitle = "Подтверждение",
                ContentMessage = $"Вы действительно хотите удалить группу {i.Name}",
                Icon = Icon.None,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                CanResize = false,
                SizeToContent = SizeToContent.WidthAndHeight,
                ShowInCenter = true,
                Topmost = true,
            }
        );
        var result = await mbox.ShowAsPopupAsync(this);
        if (result is not "Да") return;

        DatabaseContext.InstanceFor(this).Groups.Remove(i);
        await DatabaseContext.InstanceFor(this).SaveChangesAsync();
        ViewModel?.RemoveLocal(i);
    }

    private async Task NewItem() {
        var window = new EditGroupView(group => {
            if (group is null) return;
            using var db = DatabaseContext.NewInstance();
            db.Groups.Add(group);
            db.SaveChanges();
            this.Log().Error(group.Id);
            ViewModel!.AddLocal(group);
        });
        await window.ShowDialog(Application.Current!.MainWindow());
    }

    private async void EditItem(Group? i) {
        if (i is null) return;
        var window = new EditGroupView(group => {
            if (group is null) return;
            using var db = DatabaseContext.NewInstance();
            try {
                db.Groups.Attach(i);
            }
            catch (Exception e) {
                Console.WriteLine(e);
            }
            if (i.Members?.Any() ?? false) {
                foreach (var iMember in i.Members) {
                    if (iMember is null) continue;
                    try {
                        db.Clients.Attach(iMember);
                    }
                    catch (Exception e) {
                        Console.WriteLine(e);
                    }
                }
            }
            db.Groups.Update(i);
            db.SaveChanges();
            this.Log().Error(group.Id);
            ViewModel!.ReplaceItem(i, group);
        }, i);
        await window.ShowDialog(Application.Current!.MainWindow());
    }

    private static readonly Dictionary<int, Func<Group, object>> OrderSelectors = new() {
        { 1, it => it.Id },
        { 2, it => it.Name },
        { 3, it => $"{it.ResponsibleTeacher.LastName} {it.ResponsibleTeacher.FirstName}" },
        { 4, it => it.Course.Title },
    };

    private static readonly Dictionary<int, Func<string, Func<Group, bool>>> FilterSelectors = new() {
        { 1, query => it => it.Id.ToString().Contains(query) },
        { 2, query => it => it.Name.Contains(query, StringComparison.InvariantCultureIgnoreCase) }, {
            3,
            query => it =>
                $"{it.ResponsibleTeacher.LastName} {it.ResponsibleTeacher.FirstName}".Contains(
                    query, StringComparison.InvariantCultureIgnoreCase)
        },
        { 4, query => it => it.Course.Title.Contains(query, StringComparison.InvariantCultureIgnoreCase) },
    };

    private static object DefaultOrderSelector(Group it) => it.Id;

    private static Func<Group, bool> DefaultFilterSelector(string query)
        => it => it.Id.ToString().Contains(query, StringComparison.InvariantCultureIgnoreCase)
                 || it.Name.Contains(query, StringComparison.InvariantCultureIgnoreCase)
                 || $"{it.ResponsibleTeacher.LastName} {it.ResponsibleTeacher.FirstName}".Contains(
                     query, StringComparison.InvariantCultureIgnoreCase)
                 || it.Course.Title.Contains(query, StringComparison.InvariantCultureIgnoreCase)
                 || it.Members.Count.ToString().Contains(query, StringComparison.InvariantCultureIgnoreCase);

    private List<Group> DatabaseGetter() {
        return DatabaseContext.InstanceFor(this).Groups
                              .Include(x => x.Course)
                              .Include(x => x.Members)
                              .Include(x => x.ResponsibleTeacher)
                              .ToList();
    }
}