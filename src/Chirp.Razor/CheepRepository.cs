using Chirp.Razor.Datamodel;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Razor;

public class CheepRepository : ICheepRepository
{

    //This should handle data logic for cheep
    
    private readonly CheepDBContext _context;


    public CheepRepository(CheepDBContext _context)
    {
        this._context = _context;
    }
    
    
    public async Task<List<Cheep>> GetCheeps(int page)
    {
        var query = (from cheep in _context.Cheeps
                orderby cheep.Timestamp descending
                select cheep)
            .Include(c => c.Author)
            .Skip(page - 1 * 32).Take(32);
        Console.WriteLine(query.Count());
        var result = await query.ToListAsync();
        return result;
    }


    public async Task<List<Cheep>> GetCheepsFromAuthor(int page, string author)
    {
        var query = (from cheep in _context.Cheeps
                where cheep.Author.Name == author
                orderby cheep.Timestamp descending
                select cheep)
            .Include(c => c.Author)
            .Skip(page - 1 * 32).Take(32);
        Console.WriteLine(query.Count());
        var result = await query.ToListAsync();
        return result;
    }
}