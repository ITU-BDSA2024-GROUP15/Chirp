namespace Chirp.Razor;

public class CheepDto
{
    public required string Message { get; set; } 
    public DateTime Timestamp { get; set; }
    public required string Author { get; set; }
}