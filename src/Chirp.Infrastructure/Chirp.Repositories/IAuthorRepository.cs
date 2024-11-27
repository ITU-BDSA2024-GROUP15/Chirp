using Chirp.Core;

namespace Chirp.Infrastructure.Chirp.Repositories;

/// <summary>
/// Defines method signatures for handling authors
/// </summary>
public interface IAuthorRepository
{
    /// <summary>
    /// Gets an Author form a given name.
    /// </summary>
    /// <param name="name">Name of the Author</param>
    /// <returns>An Author</returns>
    public Task<Author> GetAuthorByName(string name);
    
    /// <summary>
    /// Gets an Author form a given email.
    /// </summary>
    /// <param name="email">Email of the Author</param>
    /// <returns>An Author</returns>
    public Task<Author> GetAuthorByEmail(string email); 
    
    /// <summary>
    /// Creates a new Author
    /// </summary>
    /// <param name="name">Name of Author</param>
    /// <param name="email">Email of Author</param>
    /// <exception cref="ArgumentException">thrown if name contains illegal characters</exception>
    public Task CreateAuthor(string name, string email);
    
}