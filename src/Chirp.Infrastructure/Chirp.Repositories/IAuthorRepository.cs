using Chirp.Core;

namespace Chirp.Infrastructure.Chirp.Repositories;

public interface IAuthorRepository
{
    public Task<Author> GetAuthorByName(string name);
    public Task<Author?> GetAuthorByEmail(string email); 
    
    public Task<Author> CreateAuthor(string name, string email);
    
}