using System.ComponentModel.DataAnnotations;
using Avalonia.Collections;

namespace ED2023.Database.Models; 

public class Course : ModelBase {
    private int _id;
    private string _title = string.Empty;
    private string? _description;
    private Teacher _teacher;
    private readonly AvaloniaList<Group> _groups;

    [Key]
    public int Id {
        get => _id;
        set => this.RaiseAndSetIfChanged(ref _id, value);
    }

    public string Title {
        get => _title;
        set => this.RaiseAndSetIfChanged(ref _title, value);
    }

    public string? Description {
        get => _description;
        set => this.RaiseAndSetIfChanged(ref _description, value);
    }

    public Teacher Teacher {
        get => _teacher;
        set => this.RaiseAndSetIfChanged(ref _teacher, value);
    }

    public AvaloniaList<Group> Groups {
        get => _groups;
        init => this.RaiseAndSetIfChanged(ref _groups, value);
    }

    public override Course Clone() {
        return new Course() {
            Id = Id,
            Title = Title,
            Description = Description,
            Teacher = Teacher
        };
    }
}