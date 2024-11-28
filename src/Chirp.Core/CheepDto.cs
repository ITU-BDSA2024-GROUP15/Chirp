namespace Chirp.Core;


/// <summary>
/// Used to transfer cheep data to the frontend.
/// Ensures that sensitive data is not exposed to the front end.
/// </summary>
public class CheepDto
{
    
    public required string Message { get; set; } 
    public DateTime Timestamp { get; set; }
    public required string Author { get; set; }
    public bool Follows { get; set; }
}