using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Core;

[PrimaryKey(nameof(AuthorId), nameof(FollowsId))]
public class Follow
{
    [Required]
    public int AuthorId { get; set; }
    [Required]
    public required Author Author { get; set; }
    
    [Required]
    public int FollowsId { get; set; }
    [Required]
    public required Author Follows { get; set; }
}