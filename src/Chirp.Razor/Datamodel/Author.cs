﻿using System.ComponentModel.DataAnnotations;

namespace Chirp.Razor.Datamodel;

public class Author
{
    [StringLength(100)]
    [Required]
    public required int AuthorId { get; set; }
    
    [StringLength(100)]
    [Required]
    public required string Name { get; set; }
    
    [StringLength(100)]
    [Required]
    public required string Email { get; set; }
    
    public List<Cheep>? Cheeps { get; set; }
    
}