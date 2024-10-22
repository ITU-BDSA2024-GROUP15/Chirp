using Chirp.Core;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure.Chirp.Repositories;

public class AuthorRepository : IAuthorRepository
{
    
    //This should handle data logic for cheep
    
    private readonly CheepDbContext _context;


    public AuthorRepository(CheepDbContext context)
    {
        _context = context;
    }
    
    
    public async Task<Author> GetAuthorByName(string name)
    {
        var query = (from author in _context.Authors
            where author.Name == name
            select author);
        var result = await query.ToListAsync();
        Console.WriteLine(query);
        if ( result.Count == 0 )
        {
            return null;
        }
        return result[0];
    }


    public async Task<Author?> GetAuthorByEmail(string email)
    {
        var query = (from author in _context.Authors
                where author.Email == email
                select author);
        var result = await query.ToListAsync();
        Console.WriteLine(query);
        if ( result.Count == 0 )
        {
            return null;
        }
        return result[0];
    }


    public async Task CreateAuthor(string name, string email)
    {
        //Should get id for new author 1 bigger than the current max 
        int maxId = _context.Authors.Max(author => author.AuthorId);
        
        //Create new author
        var newAuthor = new Author()
        {
            AuthorId = maxId + 1,
            Name = name,
            Email = email
        };
        
        await _context.Authors.AddAsync(newAuthor);
        await _context.SaveChangesAsync();
    }
    
}