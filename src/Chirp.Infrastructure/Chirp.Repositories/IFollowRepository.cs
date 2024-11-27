using Chirp.Core;

namespace Chirp.Infrastructure.Chirp.Repositories;


/// <summary>
/// Defines method signatures for handling follows
/// </summary>
public interface IFollowRepository
{
    public Task AddFollowing(string follower, string followed);
    public Task RemoveFollowing(string follower, string followed);
    public Task<List<Follow>> GetFollowed(string follower);
}