using System.ComponentModel.DataAnnotations;

namespace ED2023.Database.Models; 

public class Client {
    [Key] public int Id { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTimeOffset BirthDate { get; set; }

    public ICollection<Attendance> Attendance { get; }
    public ICollection<Group> Groups { get; }
}