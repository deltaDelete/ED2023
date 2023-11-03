using System.ComponentModel.DataAnnotations;

namespace ED2023.Database.Models; 

public class Group {
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    public Teacher ResponsibleTeacher { get; set; }
    
    public ICollection<Client> Members { get; init; }
    public Course Course { get; set; }
}