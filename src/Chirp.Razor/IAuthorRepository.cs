using Chirp.Razor.Datamodel;

namespace Chirp.Razor;

public interface IAuthorRepository
{
    public Task<Author> GetAuthorByName(string name);
    public Task<Author?> GetAuthorByEmail(string email); 
    
    public Task<Author> CreateAuthor(string name, string email);
    
}