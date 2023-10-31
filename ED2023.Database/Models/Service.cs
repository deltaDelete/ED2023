using System.ComponentModel.DataAnnotations;

namespace ED2023.Database.Models;

public class Service {
    [Key] public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}