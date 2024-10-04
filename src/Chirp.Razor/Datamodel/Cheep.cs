using System.Runtime.CompilerServices;

namespace Chirp.Razor.Datamodel;

public class Cheep
{
    public int CheepId { get; set; }
    public string Text { get; set; }
    public DateTime Timestamp { get; set; }
    public Author Author { get; set; }
    public int AuthorId { get; set; }
    
}