using Chirp.Core;

namespace Chirp.Infrastructure.Chirp.Repositories;

public interface IAuthorRepository
{
    public Task<Author> GetAuthorByName(string name);
    public Task<Author> GetAuthorByEmail(string email); 
    
    public Task CreateAuthor(string name, string email);
    public Task AddFollowing(int authorId, Author followerAuthor, int followingId, Author followsAuthor);
    public Task RemoveFollowing(int authorId, Author followerAuthor, int followingId, Author followsAuthor);
}