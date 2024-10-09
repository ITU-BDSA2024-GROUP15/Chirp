using Chirp.Razor.Datamodel;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Razor;

public class AuthorRepository : IAuthorRepository
{
    
    //This should handle data logic for cheep
    
    private readonly CheepDbContext _context;


    public AuthorRepository(CheepDbContext context)
    {
        this._context = context;
    }
    
    
    public async Task<Author> GetAuthorByName(string name)
    {
        var query = (from author in _context.Authors
            where author.Name == name
            select author);
        var result = await query.ToListAsync();
        Console.WriteLine(query);
        return result[0];
    }


    public async Task<Author?> GetAuthorByEmail(string email)
    {
        var query = (from author in _context.Authors
                where author.Email == email
                select author);
        var result = await query.ToListAsync();
        Console.WriteLine(query);
        return result[0];
    }


    public Task<Author> CreateAuthor(string name, string email)
    {
        
        
        throw new NotImplementedException();
    }
}