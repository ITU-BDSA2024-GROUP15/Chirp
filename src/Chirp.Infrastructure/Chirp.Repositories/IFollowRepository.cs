using Chirp.Core;

namespace Chirp.Infrastructure.Chirp.Repositories;

public interface IFollowRepository
{
    public Task AddFollowing(string followerAuthorName, string followsAuthorName);
    public Task RemoveFollowing(string followerAuthorName, string followsAuthorName);
    public Task<List<Follow>> GetFollowed(string followerAuthorName);
}