using System;
using System.Linq;
using Avalonia.ReactiveUI;
using ED2023.App.ViewModels;
using ED2023.Database.Models;

namespace ED2023.App.Views.Dialogs; 

public partial class EditClientView : ReactiveWindow<EditClientViewModel> {
    public EditClientView(Action<Client?> acceptAction) {
        InitializeComponent();
        ViewModel = new(this, acceptAction);
    }

    public EditClientView(Action<Client?> acceptAction, Client item) {
        InitializeComponent();
        ViewModel = new(this, acceptAction);
        var itemClone = item.Clone();
        ViewModel.Item = itemClone;
    }
}