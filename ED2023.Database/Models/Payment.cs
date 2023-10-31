using System.ComponentModel.DataAnnotations;

namespace ED2023.Database.Models; 

public class Payment {
    [Key] public int Id { get; set; }
    public DateTimeOffset Date { get; set; }
    public decimal Cost { get; set; }

    public Client Client { get; set; }
    public Course Course { get; set; }

    public ICollection<Service>? Services { get; }
}