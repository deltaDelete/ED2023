using System.ComponentModel.DataAnnotations;

namespace ED2023.Database.Models; 

public class Attendance {
    [Key]
    public int Id { get; set; }

    public Client Client { get; set; }
    public Schedule Schedule { get; set; }
}