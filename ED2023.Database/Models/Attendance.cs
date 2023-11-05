using System.ComponentModel.DataAnnotations;

namespace ED2023.Database.Models; 

public class Attendance : ModelBase {
    private int _id;
    private Client _client;
    private Schedule _schedule;

    [Key]
    public int Id {
        get => _id;
        set => _id = value;
    }

    public Client Client {
        get => _client;
        set => _client = value;
    }

    public Schedule Schedule {
        get => _schedule;
        set => _schedule = value;
    }
}