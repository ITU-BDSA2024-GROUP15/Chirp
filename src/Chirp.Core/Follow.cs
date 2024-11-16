using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Core;

[PrimaryKey(nameof(AuthorId), nameof(FollowsId), nameof(AuthorName))]
public class Follow
{
    [Required]
    public int AuthorId { get; set; }
    [Required]
    [StringLength(100)]
    public required string AuthorName { get; set; }
    
    [Required]
    public int FollowsId { get; set; }
    [Required]
    [StringLength(100)]
    public required string FollowsAuthorName { get; set; }
}