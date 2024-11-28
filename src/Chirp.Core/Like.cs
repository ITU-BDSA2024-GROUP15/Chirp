using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Core;

[PrimaryKey(nameof(CheepId),nameof(AuthorName))]
public class Like
{
    [Required]
    public required int CheepId { get; set; } 
    [Required]
    public required string AuthorName { get; set; } 
}