using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Core;

[PrimaryKey(nameof(Follower),nameof(Followed))]
public class Follow
{

    [Required]
    [StringLength(100)]
    public required string Follower { get; set; } //AuthorName
    [Required]
    [StringLength(100)]
    public required string Followed { get; set; } //FollowsAuthorName
}