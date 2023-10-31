using System.ComponentModel.DataAnnotations;

namespace ED2023.Database.Models; 

public class Language {
    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
}