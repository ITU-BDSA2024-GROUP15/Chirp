using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Chirp.Core;

public class Author : IdentityUser<int>
{
    
    //TODO Our identity now has two ID's. Gotta fix
    /*
    [StringLength(100)]
    [Required]
    public required int AuthorId { get; set; }
    */
    
    [StringLength(100)]
    [Required]
    public required string Name { get; set; }

    [StringLength(100)]
    [Required]
    public new required string Email { get; set; } //TODO make email and name unique 
    
    public List<Cheep>? Cheeps { get; set; }
    
}