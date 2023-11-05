using System.ComponentModel.DataAnnotations;
using Avalonia.Collections;

namespace ED2023.Database.Models; 

public class Payment : ModelBase {
    private int _id;
    private DateTimeOffset _date;
    private decimal _cost;
    private Client _client;
    private Course _course;
    private readonly AvaloniaList<Service>? _services;

    [Key]
    public int Id {
        get => _id;
        set => this.RaiseAndSetIfChanged(ref _id, value);
    }

    public DateTimeOffset Date {
        get => _date;
        set => this.RaiseAndSetIfChanged(ref _date, value);
    }

    public decimal Cost {
        get => _cost;
        set => this.RaiseAndSetIfChanged(ref _cost, value);
    }

    public Client Client {
        get => _client;
        set => this.RaiseAndSetIfChanged(ref _client, value);
    }

    public Course Course {
        get => _course;
        set => this.RaiseAndSetIfChanged(ref _course, value);
    }

    public AvaloniaList<Service>? Services => _services;
}