using Chirp.Core;

namespace Chirp.Infrastructure.Chirp.Repositories;

public interface IAuthorRepository
{
    public Task<Author> GetAuthorByName(string name);
    public Task<Author> GetAuthorByEmail(string email); 
    
    public Task CreateAuthor(string name, string email);
    public Task AddFollowing(string followerAuthorName, string followsAuthorName);
    public Task RemoveFollowing(string followerAuthorName, string followsAuthorName);
    public  Task<List<Follow>> GetFollowed(string followerAuthorName);
}