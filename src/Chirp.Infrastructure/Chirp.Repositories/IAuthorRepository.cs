using Chirp.Core;

namespace Chirp.Infrastructure.Chirp.Repositories;

/// <summary>
/// Defines method signatures for handling authors
/// </summary>
public interface IAuthorRepository
{
    public Task<Author> GetAuthorByName(string name);
    public Task<Author> GetAuthorByEmail(string email); 
    
    public Task CreateAuthor(string name, string email);
    
}