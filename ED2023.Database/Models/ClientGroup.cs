namespace ED2023.Database.Models; 

public class ClientGroup : ModelBase {
    private int _id;
    private Client? _client;
    private Group? _group;

    [Key]
    public int Id {
        get => _id;
        set => this.RaiseAndSetIfChanged(ref _id, value);
    }

    public Client? Client {
        get => _client;
        set => this.RaiseAndSetIfChanged(ref _client, value);
    }

    public Group? Group {
        get => _group;
        set => this.RaiseAndSetIfChanged(ref _group, value);
    }
}