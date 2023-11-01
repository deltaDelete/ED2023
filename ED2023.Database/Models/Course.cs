using System.ComponentModel.DataAnnotations;

namespace ED2023.Database.Models; 

public class Course {
    [Key]
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    public Teacher Teacher { get; set; }

    public ICollection<Group> Groups { get; }
}