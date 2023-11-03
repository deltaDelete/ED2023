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
            new () {
                
                ButtonDefinitions = new ButtonDefinition[] {
                    new() {Name = "Да", IsDefault = true},
                    new() {Name = "Нет", IsCancel = true}
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
        
        DatabaseContext.Instance.Groups.Remove(i);
        await DatabaseContext.Instance.SaveChangesAsync();
        ViewModel?.RemoveLocal(i);
    }

    private async Task NewItem() {
        var window = new EditGroupView(group => {
            if (group is null) return;
            DatabaseContext.Instance.Groups.Add(group);
            DatabaseContext.Instance.SaveChanges();
            this.Log().Error(group.Id);
            ViewModel!.AddLocal(group);
        });
        await window.ShowDialog(Application.Current!.MainWindow());
    }

    private async void EditItem(Group? i) {
        if (i is null) return;
        var window = new EditGroupView(group => {
            if (group is null) return;
            DatabaseContext.Instance.Groups.Update(i);
            DatabaseContext.Instance.SaveChanges();
            this.Log().Error(group.Id);
            ViewModel!.ReplaceItem(i, group);
        }, i);
        await window.ShowDialog(Application.Current!.MainWindow());
    }

    private static readonly Dictionary<int,Func<Group,object>> OrderSelectors = new() {
        {1, it => it.Id},
        {2, it => it.Name},
        {3, it => $"{it.ResponsibleTeacher.LastName} {it.ResponsibleTeacher.FirstName}" },
        {4, it => it.Course.Title},
    };

    private static readonly Dictionary<int, Func<string, Func<Group, bool>>> FilterSelectors = new() {
        { 1, query => it => it.Id.ToString().Contains(query) },
        { 2, query => it => it.Name.ToLower().Contains(query) },
        { 3, query => it => $"{it.ResponsibleTeacher.LastName} {it.ResponsibleTeacher.FirstName}".ToLower().Contains(query) },
        { 4, query => it => it.Course.Title.ToLower().Contains(query) },
    };

    private static readonly Func<Group, object> DefaultOrderSelector = it => it.Id;

    private static readonly Func<string, Func<Group, bool>> DefaultFilterSelector =
        query => it => it.Id.ToString().Contains(query)
                       || it.Name.ToLower().Contains(query)
                       || $"{it.ResponsibleTeacher.LastName} {it.ResponsibleTeacher.FirstName}".ToLower().Contains(query)
                       || it.Course.Title.ToLower().Contains(query)
                       || it.Members.Count.ToString().Contains(query);
    
    private static List<Group> DatabaseGetter() {
        return DatabaseContext.Instance.Groups
            .Include(x => x.Course)
            .Include(x => x.Members)
            .Include(x => x.ResponsibleTeacher)
            .ToList();
    }
}