using System.ComponentModel.DataAnnotations;

namespace ED2023.Database.Models; 

public class Schedule {
    [Key]
    public int Id { get; set; }
    public DateTimeOffset Start { get; set; }
    public DateTimeOffset End { get; set; }

    public Course Course { get; set; }
    public Group Group { get; set; }
}