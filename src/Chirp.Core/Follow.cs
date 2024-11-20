using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Core;

[PrimaryKey(nameof(AuthorName),nameof(FollowsAuthorName))]
public class Follow
{

    [Required]
    [StringLength(100)]
    public required string AuthorName { get; set; }
    [Required]
    [StringLength(100)]
    public required string FollowsAuthorName { get; set; }
}