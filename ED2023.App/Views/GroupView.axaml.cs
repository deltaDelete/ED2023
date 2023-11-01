using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.ReactiveUI;
using ED2023.App.ViewModels;
using ED2023.Database;
using ED2023.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace ED2023.App.Views;

public partial class GroupView : ReactiveUserControl<TableViewModelBase<Group>> {
    public GroupView() {
        InitializeComponent();
        ViewModel = new TableViewModelBase<Group>(
            DatabaseGetter,
            OrderSelectors,
            DefaultOrderSelector,
            FilterSelectors,
            DefaultFilterSelector,
            _ => {},
            () => Task.CompletedTask,
            _ => {}
        );
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
        query => it => it.Id.ToString().Contains(query);
    
    private static List<Group> DatabaseGetter() {
        return DatabaseContext.Instance.Groups
            .Include(x => x.Course)
            .Include(x => x.Members)
            .Include(x => x.ResponsibleTeacher)
            .ToList();
    }
}