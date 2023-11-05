using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Collections;
using Avalonia.Controls;
using DynamicData;
using DynamicData.Binding;
using ED2023.Database;
using ED2023.Database.Models;
using Microsoft.EntityFrameworkCore.Query.Internal;
using MsBox.Avalonia;
using ReactiveUI;

namespace ED2023.App.ViewModels;

public class EditClientViewModel : ViewModelBase {
    private Client _item;

    public Client Item {
        get => _item;
        set => this.RaiseAndSetIfChanged(ref _item, value);
    }

    public ReactiveCommand<Unit, Unit> CloseCommand { get; private set; }
    public ReactiveCommand<Unit, Unit> AcceptCommand { get; private set; }

    public EditClientViewModel(Window parent, Action<Client?> acceptAction) {
        _item = new();
        CloseCommand = ReactiveCommand.Create(parent.Close);
        AcceptCommand = ReactiveCommand.Create(() => {
            bool isValid =
                !string.IsNullOrWhiteSpace(Item.FirstName)
                && !string.IsNullOrWhiteSpace(Item.LastName)
                && !string.IsNullOrWhiteSpace(Item.Phone)
                && !string.IsNullOrWhiteSpace(Item.Email);
            if (!isValid) {
                MessageBoxManager.GetMessageBoxStandard(
                                     "Ошибка", "Проверьте заполненность полей и правильность введенных значений")
                                 .ShowAsPopupAsync(parent);
                return;
            }

            acceptAction(Item);
            parent.Close();
        });
    }
}