
namespace Chirp.Core;

/// <summary>
/// Used to transfer author data to the frontend.
/// Ensures that sensitive data is not exposed to the front end.
/// </summary>
public class AuthorDTO
{
    public required string Username { get; set; }
    public required string Email { get; set; }
}