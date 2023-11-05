using System.ComponentModel.DataAnnotations;

namespace ED2023.Database.Models; 

public class Language : ModelBase {
    private int _id;
    private string _name = string.Empty;

    [Key]
    public int Id {
        get => _id;
        set => this.RaiseAndSetIfChanged(ref _id, value);
    }

    public string Name {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }
}