using System.ComponentModel.DataAnnotations;
using Chirp.Core;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure.Chirp.Repositories;

/// <summary>
/// Used to handle data logic for author.
/// Includes methods for accessing and handling author data.
/// </summary>
public class AuthorRepository : IAuthorRepository
{
    
    private readonly CheepDbContext _context;


    public AuthorRepository(CheepDbContext context)
    {
        _context = context;
    }
    
    
    /// <summary>
    /// Gets an Author form a given name.
    /// </summary>
    /// <param name="name">Name of the Author</param>
    /// <returns>An Author</returns>
    public async Task<Author> GetAuthorByName(string name)
    {
        var query = (from author in _context.Authors
            where author.Name == name
            select author);
        var result = await query.ToListAsync();
        
        if ( result.Count == 0 )
        {
            return null;
        }
        return result[0];
    }


    /// <summary>
    /// Gets an Author form a given email.
    /// </summary>
    /// <param name="email">Email of the Author</param>
    /// <returns>An Author</returns>
    public async Task<Author> GetAuthorByEmail(string email)
    {
        var query = (from author in _context.Authors
                where author.Email == email
                select author);
        var result = await query.ToListAsync();
        
        if ( result.Count == 0 )
        {
            return null;
        }
        return result[0];
    }

    /// <summary>
    /// Creates a new Author
    /// </summary>
    /// <param name="name">Name of Author</param>
    /// <param name="email">Email of Author</param>
    /// <exception cref="ArgumentException">thrown if name contains illegal characters</exception>
    public async Task CreateAuthor(string name, string email)
    {

        //Extra check for input validation
        if ( name.Contains('/') || name.Contains('\\') )
        {
            throw new ArgumentException();
        }
       
        //Should get id for new author 1 bigger than the current max 
        int maxId = _context.Authors.Max(author => author.Id);
        
        //Create new author
        var newAuthor = new Author()
        {
            Id = maxId + 1,
            Name = name,
            Email = email
        };
        
        await _context.Authors.AddAsync(newAuthor);
        await _context.SaveChangesAsync();
    }
    
}