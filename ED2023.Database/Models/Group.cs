using System.ComponentModel.DataAnnotations;
using Avalonia.Collections;
using DynamicData;

namespace ED2023.Database.Models; 

public class Group : ModelBase {
    private int _id;
    private string _name = string.Empty;
    private Teacher _responsibleTeacher;
    private readonly AvaloniaList<Client> _members;
    private Course _course;

    [Key]
    public int Id {
        get => _id;
        set => this.RaiseAndSetIfChanged(ref _id, value);
    }

    public string Name {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    public Teacher ResponsibleTeacher {
        get => _responsibleTeacher;
        set => this.RaiseAndSetIfChanged(ref _responsibleTeacher, value);
    }

    public AvaloniaList<Client> Members {
        get => _members;
        init => this.RaiseAndSetIfChanged(ref _members, value);
    }

    public Course Course {
        get => _course;
        set => this.RaiseAndSetIfChanged(ref _course, value);
    }

    public override Group Clone() {
        return new Group() {
            Id = Id,
            Name = Name,
            Course = Course.Clone(),
            ResponsibleTeacher = ResponsibleTeacher.Clone(),
            Members = Members
        };
    }
}