using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Avalonia.Collections;
using Avalonia.Controls;
using DynamicData;
using DynamicData.Binding;
using ED2023.Database;
using ED2023.Database.Models;
using Microsoft.EntityFrameworkCore.Query.Internal;
using ReactiveUI;

namespace ED2023.App.ViewModels;

public class EditGroupViewModel : ViewModelBase {
    private Client? _memberToAdd;

    public GroupAvalonia Item { get; set; } = new() {
        Members = new AvaloniaList<Client>()
    };

    public IList<Teacher> Teachers => DatabaseContext.Instance.Teachers.ToList();
    public IList<Client> Clients => DatabaseContext.Instance.Clients.ToList();

    public IList<Course> Courses => DatabaseContext.Instance.Courses.ToList();

    public Client? MemberToAdd {
        get => _memberToAdd;
        set => this.RaiseAndSetIfChanged(ref _memberToAdd, value);
    }

    public ReactiveCommand<Unit, Unit> AddMemberCommand { get; }
    public ReactiveCommand<Unit, Unit> CloseCommand { get; }
    public ReactiveCommand<Unit, Unit> AcceptCommand { get; }

    public EditGroupViewModel(Window parent, Action<Group?> acceptAction) {
        var canAddMember = this.WhenAnyValue(
            x => x.MemberToAdd,
            selector: i => i is not null
        );
        var canSave = this.WhenAnyValue(
            x => x.Item,
            x => x.Item.Name,
            x => x.Item.Members,
            x => x.Item.ResponsibleTeacher,
            selector: (i1, i2, i3, i4) =>
                i1 is not null
                && i2 is not null
                && i3 is not null
                && i4 is not null
        );
        AddMemberCommand = ReactiveCommand.Create(AddMember, canAddMember);
        CloseCommand = ReactiveCommand.Create(parent.Close);
        AcceptCommand = ReactiveCommand.Create(() => {
            acceptAction(Item);
            parent.Close();
        }, canSave);
    }

    private void AddMember() {
        if (MemberToAdd is null) return;
        this.RaisePropertyChanging(nameof(Item.Members));
        Item.Members.Add(MemberToAdd);
        this.RaisePropertyChanged(nameof(Item.Members));
        this.RaisePropertyChanged(nameof(Item));
    }
}

public class GroupAvalonia : Group {
    public new AvaloniaList<Client>? Members { get; set; }

    public GroupAvalonia(Group g) {
        Id = g.Id;
        Course = g.Course;
        Members = new AvaloniaList<Client>(g.Members);
        ResponsibleTeacher = g.ResponsibleTeacher;
        Name = g.Name;
    }

    public GroupAvalonia() : base() {
        
    }
}