using System.ComponentModel.DataAnnotations;
using Avalonia.Collections;

namespace ED2023.Database.Models; 

public class Client : ModelBase {
    private int _id;
    private string _lastName = string.Empty;
    private string _firstName = string.Empty;
    private string _email = string.Empty;
    private string _phone = string.Empty;
    private DateTimeOffset _birthDate;
    private readonly AvaloniaList<Attendance> _attendance;
    private readonly AvaloniaList<Group> _groups;

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

    public AvaloniaList<Attendance> Attendance {
        get => _attendance;
        init => this.RaiseAndSetIfChanged(ref _attendance, value);
    }

    public AvaloniaList<Group> Groups {
        get => _groups;
        init => this.RaiseAndSetIfChanged(ref _groups, value);
    }

    public override Client Clone() {
        return new Client {
            Id = Id,
            LastName = LastName,
            FirstName = FirstName,
            Email = Email,
            Phone = Phone,
            BirthDate = BirthDate
        };
    }
}