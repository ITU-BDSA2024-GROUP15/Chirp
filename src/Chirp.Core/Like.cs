using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Core;

/// <summary>
/// Represents a like in the Chirp application
/// </summary>
[PrimaryKey(nameof(CheepId),nameof(AuthorName))]
public class Like
{
    [Required]
    public required int CheepId { get; set; } 
    
    /// <summary>
    /// Name of the author who liked the cheep
    /// </summary>
    [Required]
    public required string AuthorName { get; set; }
}