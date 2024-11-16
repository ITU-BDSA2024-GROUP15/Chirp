using Chirp.Core;

namespace Chirp.Infrastructure.Chirp.Repositories;

public interface IAuthorRepository
{
    public Task<Author> GetAuthorByName(string name);
    public Task<Author> GetAuthorByEmail(string email); 
    
    public Task CreateAuthor(string name, string email);
    public Task AddFollowing(int authorId, string followerAuthorName, int followingId, string followsAuthorName);
    public Task RemoveFollowing(int authorId, string followerAuthorName);
    public  Task<List<Follow>> GetFollowing(int authorId, string followerAuthorName, int followingId);
}