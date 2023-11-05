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

public class EditGroupViewModel : ViewModelBase {
    private Client? _memberToAdd;
    private Group _item;

    public Group Item {
        get => _item;
        set => this.RaiseAndSetIfChanged(ref _item, value);
    }

    public IList<Teacher> Teachers => DatabaseContext.InstanceFor(this).Teachers.ToList();
    public IList<Client> Clients => DatabaseContext.InstanceFor(this).Clients.ToList();

    public IList<Course> Courses => DatabaseContext.InstanceFor(this).Courses.ToList();

    public Client? MemberToAdd {
        get => _memberToAdd;
        set => this.RaiseAndSetIfChanged(ref _memberToAdd, value);
    }

    public ReactiveCommand<Unit, Unit> AddMemberCommand { get; private set; }
    public ReactiveCommand<Unit, Unit> CloseCommand { get; private set; }
    public ReactiveCommand<Unit, Unit> AcceptCommand { get; private set; }
    public ReactiveCommand<Client?, Unit> RemoveMemberCommand { get; private set; }
    
    public EditGroupViewModel(Window parent, Action<Group?> acceptAction) {
        Item = new() {
            Members = new AvaloniaList<Client>()
        };
        var canAddMember = this.WhenAnyValue(
            x => x.MemberToAdd,
            selector: i => i is not null
        );
        AddMemberCommand = ReactiveCommand.Create(AddMember, canAddMember);
        CloseCommand = ReactiveCommand.Create(parent.Close);
        AcceptCommand = ReactiveCommand.Create(() => {
            bool isValid = !string.IsNullOrWhiteSpace(Item.Name)
                           && Item.ResponsibleTeacher is not null
                           && Item.Course is not null;
            if (!isValid) {
                MessageBoxManager.GetMessageBoxStandard(
                                     "Ошибка", "Проверьте заполненность полей и правильность введенных значений")
                                 .ShowAsPopupAsync(parent);
                return;
            }

            acceptAction(Item);
            parent.Close();
        });
        RemoveMemberCommand = ReactiveCommand.Create<Client?>(RemoveMember);
    }

    private void AddMember() {
        if (MemberToAdd is null) return;
        this.RaisePropertyChanging(nameof(Item.Members));
        Item.Members?.Add(MemberToAdd);
        this.RaisePropertyChanged(nameof(Item.Members));
        this.RaisePropertyChanged(nameof(Item));
    }

    private void RemoveMember(Client? item) {
        if (item is null) return;
        this.RaisePropertyChanging(nameof(Item.Members));
        Item.Members?.Remove(item);
        this.RaisePropertyChanged(nameof(Item.Members));
        this.RaisePropertyChanged(nameof(Item));
    }
}
