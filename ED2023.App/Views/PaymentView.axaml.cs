using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Logging;
using Avalonia.ReactiveUI;
using ED2023.App.Utils;
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

public partial class PaymentView : ReactiveUserControl<TableViewModelBase<Payment>>, IEnableLogger {
    public PaymentView() {
        InitializeComponent();
        ViewModel = new TableViewModelBase<Payment>(
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

    private async void RemoveItem(Payment? i) {
        if (i is null) {
            return;
        }
        
        var mbox = MessageBoxUtils.CreateConfirmMessageBox("Подтверждение",
            $"Вы действительно хотите удалить оплату №{i.Id}");
        var result = await mbox.ShowAsPopupAsync(this);
        if (result is not "Да") return;

        DatabaseContext.InstanceFor(this).Payments.Remove(i);
        await DatabaseContext.InstanceFor(this).SaveChangesAsync();
        ViewModel?.RemoveLocal(i);
    }

    private async Task NewItem() {
        // var window = new EditPaymentView(payment => {
        //     if (payment is null) return;
        //     using var db = DatabaseContext.NewInstance();
        //     db.Payments.Add(payment);
        //     db.SaveChanges();
        //     this.Log().Error(payment.Id);
        //     ViewModel!.AddLocal(payment);
        // });
        // await window.ShowDialog(Application.Current!.MainWindow());
    }

    private async void EditItem(Payment? i) {
        // if (i is null) return;
        // var window = new EditPaymentView(payment => {
        //     if (payment is null) return;
        //     using var db = DatabaseContext.NewInstance();
        //     // TODO: РЕШИТЬ КАКУЮ ТО МАГИЮ С КЛЮЧАМИ В СВЯЗИ МНОГИЕ КО МНОГИМ, может сделать третью таблицу не ключ-ключ, а ключ-ссылка-ссылка
        //     db.Payments.Update(i);
        //     db.SaveChanges();
        //     this.Log().Error(payment.Id);
        //     ViewModel!.ReplaceItem(i, payment);
        // }, i);
        // await window.ShowDialog(Application.Current!.MainWindow());
    }

    private static readonly Dictionary<int, Func<Payment, object>> OrderSelectors = new() {
        { 1, it => it.Id },
        { 2, it => it.Course.Title },
        { 3, it => it.Client },
        { 4, it => it.Services?.Count ?? 0 },
        { 5, it => it.Date },
        { 6, it => it.Cost },
    };

    private static readonly Dictionary<int, Func<string, Func<Payment, bool>>> FilterSelectors = new() {
        { 1, query => it => it.Id.ToString().Contains(query) },
        { 2, query => it => it.Course.Title.Contains(query, StringComparison.InvariantCultureIgnoreCase) }, 
        {
            3,
            query => it =>
                $"{it.Client.LastName} {it.Client.FirstName}".Contains(
                    query, StringComparison.InvariantCultureIgnoreCase)
        },
        { 4, query => it => it.Services?.Count.ToString().Contains(query, StringComparison.InvariantCultureIgnoreCase) ?? false },
        { 5, query => it => it.Date.ToString().Contains(query, StringComparison.InvariantCultureIgnoreCase) },
        { 6, query => it => it.Cost.ToString(CultureInfo.InvariantCulture).Contains(query, StringComparison.InvariantCultureIgnoreCase) },
    };

    private static object DefaultOrderSelector(Payment it) => it.Id;

    private static Func<Payment, bool> DefaultFilterSelector(string query)
        => it => it.Id.ToString().Contains(query, StringComparison.InvariantCultureIgnoreCase)
                 || it.Course.Title.Contains(query, StringComparison.InvariantCultureIgnoreCase)
                 || $"{it.Client.LastName} {it.Client.FirstName}".Contains(
                     query, StringComparison.InvariantCultureIgnoreCase)
                 || (it.Services?.Count.ToString().Contains(query, StringComparison.InvariantCultureIgnoreCase) ??
                     false);

    private List<Payment> DatabaseGetter() {
        return DatabaseContext.InstanceFor(this).Payments
                              .Include(x => x.Course)
                              .Include(x => x.Client)
                              .Include(x => x.Services)
                              .ToList();
    }
}