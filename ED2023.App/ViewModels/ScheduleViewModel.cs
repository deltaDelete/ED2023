using System.ComponentModel;
using System.Linq;
using Avalonia.Data;
using ED2023.Database;
using ED2023.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace ED2023.App.ViewModels;

public class ScheduleViewModel : ViewModelBase {
    private int _pageItems = 10;
    
    public BindingList<Schedule> Items { get; set; }

    public ScheduleViewModel() {
        GetItems();
    }

    private void GetItems() {
        Items = new BindingList<Schedule>(
            DatabaseContext.Instance.Schedules.Take(_pageItems)
                .Include(x => x.Course)
                .Include(x => x.Group)
                .ToList()
        );
    }
}