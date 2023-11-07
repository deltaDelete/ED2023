using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Avalonia.Collections;
using Avalonia.Controls;
using ED2023.App.Views.Dialogs;
using ED2023.Database;
using ED2023.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MsBox.Avalonia;
using ReactiveUI;

namespace ED2023.App.ViewModels;

public class EditScheduleViewModel : ViewModelBase {
    private Schedule _item;
    private Client? _memberToAdd;
    private ObservableAsPropertyHelper<List<Client>?> _clientsHelper;

    public Schedule Item {
        get => _item;
        set => this.RaiseAndSetIfChanged(ref _item, value);
    }

    public IList<Course> Courses => DatabaseContext.InstanceFor(this).Courses.ToList();
    public IList<Group> Groups => DatabaseContext.InstanceFor(this).Groups.ToList();
    public List<Client>? Clients => _clientsHelper.Value;

    public ReactiveCommand<Unit, Unit> CloseCommand { get; private set; }
    public ReactiveCommand<Unit, Unit> AcceptCommand { get; private set; }

    public ReactiveCommand<Unit, Unit> AddMemberCommand { get; private set; }
    public ReactiveCommand<Attendance?, Unit> RemoveMemberCommand { get; private set; }

    public EditScheduleViewModel(Window parent, Action<Schedule?> acceptAction) {
        Item = new() {
            Attendances = new()
        };
        CloseCommand = ReactiveCommand.Create(parent.Close);
        AcceptCommand = ReactiveCommand.Create(() => {
            bool isValid = Item.Course is not null
                           && Item.Group is not null;
            if (!isValid) {
                MessageBoxManager.GetMessageBoxStandard(
                        "Ошибка", "Проверьте заполненность полей и правильность введенных значений")
                    .ShowAsPopupAsync(parent);
                return;
            }

            acceptAction(Item);
            parent.Close();
        });

        _clientsHelper = this
            .WhenAnyValue(x => x.Item,
                x => x.Item.Group,
                x => x.Item.Group.Members)
            .Select(tuple => {
                if (tuple.Item3 is null) {
                    return DatabaseContext.InstanceFor(this).Groups
                        .Include(it => it.Members)
                        .FirstOrDefault(it => it.Id == tuple.Item2.Id)?
                        .Members?.ToList();
                }

                return tuple.Item3.ToList();
            })
            .ToProperty(this, x => x.Clients);
            
        
        AddMemberCommand = ReactiveCommand.Create(AddMember);
        RemoveMemberCommand = ReactiveCommand.Create<Attendance?>(RemoveMember);
    }

    public Client? MemberToAdd {
        get => _memberToAdd;
        set => this.RaiseAndSetIfChanged(ref _memberToAdd, value);
    }

    public void AddMember() {
        if (MemberToAdd is null) {
            return;
        }
        
        if (Item.Attendances is null) {
            Item.Attendances = new();
        }
        
        Item.Attendances.Add(new Attendance() {
            Schedule = Item,
            Client = MemberToAdd
        });
    }

    public void RemoveMember(Attendance? arg) {
        if (arg is null) {
            return;
        }
        
        if (Item.Attendances is null) {
            Item.Attendances = new();
        }

        Item.Attendances.Remove(arg);
    }
}