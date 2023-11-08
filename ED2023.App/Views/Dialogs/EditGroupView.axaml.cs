using System;
using System.Linq;
using Avalonia.ReactiveUI;
using ED2023.App.ViewModels;
using ED2023.Database.Models;

namespace ED2023.App.Views.Dialogs; 

public partial class EditGroupView : ReactiveWindow<EditGroupViewModel> {
    public EditGroupView(Action<Group?> acceptAction) {
        InitializeComponent();
        ViewModel = new(this, acceptAction);
    }

    public EditGroupView(Action<Group?> acceptAction, Group item) {
        InitializeComponent();
        ViewModel = new(this, acceptAction);
        var itemClone = item.Clone();
        itemClone.ResponsibleTeacher = ViewModel.Teachers.FirstOrDefault(it => it.Id == item.ResponsibleTeacher.Id)!;
        itemClone.Course = ViewModel.Courses.FirstOrDefault(it => it.Id == item.Course.Id)!;
        itemClone.Members = item.Members;
        ViewModel.Item = itemClone;
    }
}