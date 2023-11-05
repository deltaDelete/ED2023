using System;
using System.Collections.Generic;
using System.Linq;
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

namespace ED2023.App.Views;

public partial class ClientView : ReactiveUserControl<TableViewModelBase<Client>> {
    public ClientView() {
        InitializeComponent();
        ViewModel = new TableViewModelBase<Client>(
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

    private async void RemoveItem(Client? i) {
        if (i is null) {
            return;
        }

        var mbox = MessageBoxUtils.CreateConfirmMessageBox(
            "Подтверждение",
            $"Вы действительно хотите удалить клиента {i.LastName} {i.FirstName}"
        );
        var result = await mbox.ShowAsPopupAsync(this);
        if (result is not "Да") return;

        DatabaseContext.InstanceFor(this).Clients.Remove(i);
        await DatabaseContext.InstanceFor(this).SaveChangesAsync();
        ViewModel?.RemoveLocal(i);
    }

    private async Task NewItem() {
        var window = new EditClientView(group => {
            if (group is null) return;
            using var db = DatabaseContext.NewInstance();
            db.Clients.Add(group);
            db.SaveChanges();
            ViewModel!.AddLocal(group);
        });
        await window.ShowDialog(Application.Current!.MainWindow());
    }

    private async void EditItem(Client? i) {
        if (i is null) return;
        var window = new EditClientView(group => {
            if (group is null) return;
            using var db = DatabaseContext.NewInstance();
            db.Clients.Update(i);
            db.SaveChanges();
            ViewModel!.ReplaceItem(i, group);
        }, i);
        await window.ShowDialog(Application.Current!.MainWindow());
    }

    private static readonly Dictionary<int, Func<Client, object>> OrderSelectors = new() {
        { 1, it => it.Id },
        { 2, it => it.LastName },
        { 3, it => it.FirstName },
        { 4, it => it.Email },
        { 5, it => it.Phone },
        { 6, it => it.BirthDate },
    };

    private static readonly Dictionary<int, Func<string, Func<Client, bool>>> FilterSelectors = new() {
        { 1, query => it => it.Id.ToString().Contains(query) },
        { 2, query => it => it.LastName.Contains(query, StringComparison.InvariantCultureIgnoreCase) },
        { 3, query => it => it.FirstName.Contains(query, StringComparison.InvariantCultureIgnoreCase) },
        { 4, query => it => it.Email.Contains(query, StringComparison.InvariantCultureIgnoreCase) },
        { 5, query => it => it.Phone.Contains(query, StringComparison.InvariantCultureIgnoreCase) },
        { 6, query => it => it.BirthDate.ToString().Contains(query, StringComparison.InvariantCultureIgnoreCase) },
    };

    private static object DefaultOrderSelector(Client it) => it.Id;

    private static Func<Client, bool> DefaultFilterSelector(string query)
        => it => it.Id.ToString().Contains(query, StringComparison.InvariantCultureIgnoreCase)
                 || it.LastName.Contains(query, StringComparison.InvariantCultureIgnoreCase)
                 || it.FirstName.Contains(query, StringComparison.InvariantCultureIgnoreCase)
                 || it.Email.Contains(query, StringComparison.InvariantCultureIgnoreCase)
                 || it.Phone.Contains(query, StringComparison.InvariantCultureIgnoreCase)
                 || it.BirthDate.ToString().Contains(query, StringComparison.InvariantCultureIgnoreCase);

    private List<Client> DatabaseGetter() {
        return DatabaseContext.InstanceFor(this).Clients
                              .ToList();
    }
}