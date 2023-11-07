using System.ComponentModel.DataAnnotations;
using Avalonia.Collections;

namespace ED2023.Database.Models;

public class Service : ModelBase {
    private string _name = string.Empty;
    private decimal _price;
    private int _id;

    [Key]
    public int Id {
        get => _id;
        set => this.RaiseAndSetIfChanged(ref _id, value);
    }

    public string Name {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    public decimal Price {
        get => _price;
        set => this.RaiseAndSetIfChanged(ref _price, value);
    }

    public AvaloniaList<Payment>? Payments { get; set; }
}