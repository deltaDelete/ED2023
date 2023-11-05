using System.ComponentModel.DataAnnotations;

namespace ED2023.Database.Models; 

public class ClientExperience : ModelBase {
    private int _id;
    private Client _client;
    private Language _language;
    private LanguageLevel _level;

    [Key]
    public int Id {
        get => _id;
        set => this.RaiseAndSetIfChanged(ref _id, value);
    }

    public Client Client {
        get => _client;
        set => this.RaiseAndSetIfChanged(ref _client, value);
    }

    public Language Language {
        get => _language;
        set => this.RaiseAndSetIfChanged(ref _language, value);
    }

    public LanguageLevel Level {
        get => _level;
        set => this.RaiseAndSetIfChanged(ref _level, value);
    }
}