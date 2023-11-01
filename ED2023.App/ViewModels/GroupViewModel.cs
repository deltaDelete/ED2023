using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using ED2023.Database;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Group = ED2023.Database.Models.Group;

namespace ED2023.App.ViewModels;

public partial class GroupViewModel {
    
    // public GroupViewModel() : base(
    //     DatabaseGetter,
    //     OrderSelectors,
    //     DefaultOrderSelector,
    //     FilterSelectors,
    //     DefaultFilterSelector,
    //     _ => {},
    //     () => Task.CompletedTask,
    //     _ => {}
    //     ) {
    //     
    // }

    // private static readonly Dictionary<int,Func<Group,object>> OrderSelectors = new() {
    //     {1, it => it.Id},
    //     {2, it => it.Name},
    //     {3, it => $"{it.ResponsibleTeather.LastName} {it.ResponsibleTeather.FirstName}" },
    //     {4, it => it.Course.Title},
    // };
    //
    // private static readonly Dictionary<int, Func<string, Func<Group, bool>>> FilterSelectors = new() {
    //     { 1, query => it => it.Id.ToString().Contains(query) },
    //     { 2, query => it => it.Name.ToLower().Contains(query) },
    //     { 3, query => it => $"{it.ResponsibleTeather.LastName} {it.ResponsibleTeather.FirstName}".ToLower().Contains(query) },
    //     { 4, query => it => it.Course.Title.ToLower().Contains(query) },
    // };
    //
    // private static readonly Func<Group, object> DefaultOrderSelector = it => it.Id;
    //
    // private static readonly Func<string, Func<Group, bool>> DefaultFilterSelector =
    //     query => it => it.Id.ToString().Contains(query);
    //
    // private static List<Group> DatabaseGetter() {
    //     return DatabaseContext.Instance.Groups
    //         .Include(x => x.Course)
    //         .Include(x => x.Members)
    //         .Include(x => x.ResponsibleTeather)
    //         .ToList();
    // }
}