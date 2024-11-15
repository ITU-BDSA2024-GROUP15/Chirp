using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Chirp.Core;

public class Author : IdentityUser<int>
{
    
    [StringLength(100)]
    [RegularExpression(@"^[^\/]*$")] //TODO Backend validation - check if this works
    [Required]
    public required string Name { get; set; }

    [StringLength(100)]
    [Required]
    public new required string Email { get; set; } //TODO make email and name unique 
    
    public List<Cheep>? Cheeps { get; set; }
    
}