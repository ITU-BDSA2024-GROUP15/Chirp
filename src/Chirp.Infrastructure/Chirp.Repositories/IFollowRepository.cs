using Chirp.Core;

namespace Chirp.Infrastructure.Chirp.Repositories;


/// <summary>
/// Defines method signatures for handling follows
/// </summary>
public interface IFollowRepository
{
    
    /// <summary>
    /// Used to add a new Follow - A Author follows another Author
    /// </summary>
    /// <param name="follower">The Author who is following</param>
    /// <param name="followed">The Author who is getting followed</param>
    public Task AddFollowing(string follower, string followed);
    
    /// <summary>
    /// Used to remove a Follow - An author no longer follows another author
    /// </summary>
    /// <param name="follower">The Author who is following</param>
    /// <param name="followed">The Author who is getting followed</param>
    public Task RemoveFollowing(string follower, string followed);
    
    /// <summary>
    /// Gets a list of Authors that an author follows.
    /// </summary>
    /// <param name="follower">The Author who is following</param>
    /// <returns>List of follows</returns>
    public Task<List<Follow>> GetFollowed(string follower);
}