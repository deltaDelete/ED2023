using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive.Linq;
using ED2023.Database;
using ED2023.Database.Models;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;

namespace ED2023.App.ViewModels; 

public class AttendanceReportViewModel : ViewModelBase {
    private Group? _group;
    private ObservableAsPropertyHelper<ImmutableList<Client>> _items;
    public List<Group> Groups { get; }

    public Group? Group {
        get => _group;
        set => this.RaiseAndSetIfChanged(ref _group, value);
    }

    public ImmutableList<Client> Items => _items.Value;

    public AttendanceReportViewModel() {
        Groups = DatabaseContext.InstanceFor(this).Groups.ToList();
        _items = this.WhenAnyValue(x => x.Group)
            .Select(g => {
                if (g is null) {
                    return new List<Client>().ToImmutableList();
                }
                return GetReport(g.Id);
            })
            .ToProperty(this, x => x.Items);
    }

    public ImmutableList<Client> GetReport(int groupId) {
        return DatabaseContext.InstanceFor(this).Groups
            .Include(x => x.Members)
            .ThenInclude(x => x.Attendance)
            .Where(it => it.Id == groupId)
            .SelectMany(
                x => x.Members.Where(it => it.Attendance.Count > 0)
            )
            .ToImmutableList();
    }
}