using System.ComponentModel.DataAnnotations;
using Avalonia.Collections;
using ReactiveUI;

namespace ED2023.Database.Models;

public class Teacher : ModelBase {
    private string _lastName = string.Empty;
    private string _firstName = string.Empty;
    private string _email = string.Empty;
    private string _phone = string.Empty;
    private DateTimeOffset _birthDate;
    private int _id;
    private readonly AvaloniaList<Course> _courses;

    [Key]
    public int Id {
        get => _id;
        set => this.RaiseAndSetIfChanged(ref _id, value);
    }

    public string LastName {
        get => _lastName;
        set => this.RaiseAndSetIfChanged(ref _lastName, value);
    }

    public string FirstName {
        get => _firstName;
        set => this.RaiseAndSetIfChanged(ref _firstName, value);
    }

    public string Email {
        get => _email;
        set => this.RaiseAndSetIfChanged(ref _email, value);
    }

    public string Phone {
        get => _phone;
        set => this.RaiseAndSetIfChanged(ref _phone, value);
    }

    public DateTimeOffset BirthDate {
        get => _birthDate;
        set => this.RaiseAndSetIfChanged(ref _birthDate, value);
    }

    public AvaloniaList<Course> Courses {
        get => _courses;
        init => _courses = value;
    }

    public override Teacher Clone() {
        return new Teacher() {
            Id = Id,
            LastName = LastName,
            FirstName = FirstName,
            Email = Email,
            Phone = Phone,
            BirthDate = BirthDate
        };
    }
}