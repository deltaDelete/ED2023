using System.ComponentModel.DataAnnotations;

namespace ED2023.Database.Models; 

public class Schedule : ModelBase {
    private int _id;
    private DateTimeOffset _start;
    private DateTimeOffset _end;
    private Course _course;
    private Group _group;

    [Key]
    public int Id {
        get => _id;
        set => this.RaiseAndSetIfChanged(ref _id, value);
    }

    public DateTimeOffset Start {
        get => _start;
        set => this.RaiseAndSetIfChanged(ref _start, value);
    }

    public DateTimeOffset End {
        get => _end;
        set => this.RaiseAndSetIfChanged(ref _end, value);
    }

    public Course Course {
        get => _course;
        set => this.RaiseAndSetIfChanged(ref _course, value);
    }

    public Group Group {
        get => _group;
        set => this.RaiseAndSetIfChanged(ref _group, value);
    }
}