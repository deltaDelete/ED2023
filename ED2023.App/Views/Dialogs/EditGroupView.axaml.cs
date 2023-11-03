using System;
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
        ViewModel = new(this, acceptAction) {
            Item = new(item)
        };
    }
}