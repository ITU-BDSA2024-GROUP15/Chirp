using Chirp.Core;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure.Chirp.Repositories;

/// <summary>
/// Used to handle data logic for cheeps.
/// Includes methods for accessing and handling cheep data.
/// </summary>
public class CheepRepository : ICheepRepository
{
    
    private readonly CheepDbContext _context;


    public CheepRepository(CheepDbContext context)
    {
        this._context = context;
    }
    
    
   
    public async Task<List<Cheep>> GetCheeps(int page)
    {
        var query = (from cheep in _context.Cheeps
                orderby cheep.Timestamp descending
                select cheep)
            .Include(c => c.Author)
            .Skip((page -1) * 32).Take(32);
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
            .Skip((page - 1) * 32).Take(32);
        var result = await query.ToListAsync();
        
        
        return result;
    }
    
    
    
    public async Task<List<Cheep>> GetAllCheepsFromAuthor(string author)
    {
        var query = ( from cheep in _context.Cheeps
                where cheep.Author.Name == author
                orderby cheep.Timestamp descending
                select cheep )
            .Include(c => c.Author);
        var result = await query.ToListAsync();
        return result;
    }
    
    
    public async Task<List<Cheep>> GetAllCheepsFromFollowed(string author) //Made with the help of ChatGPT
    {
        var query = (from cheep in _context.Cheeps
            where (from follow in _context.Follows
                    where follow.Follower == author
                    select follow.Followed)
                .Contains(cheep.Author.Name)
            select cheep)
            .Include(c => c.Author);
        
        var result = await query.ToListAsync();
        return result;
    }
    

    
    public async Task AddCheep(string text, Author author)
    {

        if ( text.Length <= 0 || text.Length > 160 )
        {
            return;
        }
        int maxId = _context.Cheeps.Max(cheep => cheep.CheepId); 
        
        
        Cheep cheep = new Cheep()
        {
            Author = author,
            AuthorId = author.Id,
            CheepId = maxId + 1,
            Text = text,
            Timestamp = DateTime.Now
        };

        await _context.Cheeps.AddAsync(cheep);
        await _context.SaveChangesAsync();
    }


    public async Task AddLike(string author, int cheepId)
    {
        var currentLikes = await _context.Cheeps.FirstOrDefaultAsync(cheep =>cheep.CheepId == cheepId);
        if (currentLikes != null)
        {
            currentLikes.Likes.Add(author);
            _context.SaveChanges();
        }
    }
    
    public async Task RemoveLike(string author, int cheepId)
    {
        var currentLikes = await _context.Cheeps.FirstOrDefaultAsync(cheep =>cheep.CheepId == cheepId);
        if (currentLikes != null && currentLikes.Likes.Contains(author))
        {
            currentLikes.Likes.Remove(author);
            _context.SaveChanges();
        }
    }

    
    public async Task<int> CountLikes(int cheepId)
    {
        var likes = await _context.Cheeps.FirstOrDefaultAsync(cheep =>cheep.CheepId == cheepId);
        return likes!.Likes.Count;
    }
    
    public async Task<List<Cheep>> GetAllLiked(string author)
    {
        var likedCheeps = await _context.Cheeps.Where(cheep => cheep.Likes.Contains(author)).Include(c => c.Author).ToListAsync();
        
        return likedCheeps;
        
    }


    public async Task DeleteAllLikes(string author)
    {
        //https://stackoverflow.com/questions/1586013/how-to-do-select-all-in-linq-to-sql
        var likedCheeps = await _context.Cheeps.Where(cheep => cheep.Likes.Contains(author)).ToListAsync();

        foreach (var likes in likedCheeps)
        {
            likes.Likes.Remove(author);
        }
        _context.SaveChanges();
    }


    public async Task<List<Cheep>> GetTopLikedCheeps(string author)
    {
        //https://stackoverflow.com/questions/5344805/linq-orderby-descending-query
        var query = (from cheep in _context.Cheeps
            .OrderByDescending(cheep => cheep.Likes.Count) select cheep).Take(32);
        
        var cheeps = await query.ToListAsync();

        return cheeps;
    }
    
    
}