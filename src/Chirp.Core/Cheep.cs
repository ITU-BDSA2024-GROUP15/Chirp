namespace Chirp.Core;

using System.ComponentModel.DataAnnotations;


public class Cheep
{
    public int CheepId { get; set; }
    [StringLength(160)]
    [Required]
    public required string Text { get; set; }
    public DateTime Timestamp { get; set; }
    [Required]
    public required Author Author { get; set; }
    public int AuthorId { get; set; }
    
}