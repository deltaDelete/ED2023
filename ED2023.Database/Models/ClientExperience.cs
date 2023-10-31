using System.ComponentModel.DataAnnotations;

namespace ED2023.Database.Models; 

public class ClientExperience {
    [Key]
    public int Id { get; set; }
    public Client Client { get; set; }
    public Language Language { get; set; }
    public LanguageLevel Level { get; set; }
}