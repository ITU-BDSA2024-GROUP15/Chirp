using System.ComponentModel.DataAnnotations;

namespace Chirp.Core;

public class Follow
{
    [Required]
    public required Author Author { get; set; }
    
    [Required]
    public required Author Follows { get; set; }
}