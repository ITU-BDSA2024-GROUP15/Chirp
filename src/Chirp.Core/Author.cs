using System.ComponentModel.DataAnnotations;

namespace Chirp.Core;

public class Author
{
    [StringLength(100)]
    [Required]
    public required int AuthorId { get; set; }
    
    [StringLength(100)]
    [Required]
    public required string Name { get; set; }

    [StringLength(100)]
    [Required]
    public required string Email { get; set; } //TODO make email and name unique 
    
    public List<Cheep>? Cheeps { get; set; }
    
}