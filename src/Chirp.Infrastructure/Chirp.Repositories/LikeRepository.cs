using Chirp.Core;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure.Chirp.Repositories;

public class LikeRepository : ILikeRepository
{
    private readonly CheepDbContext _context;
    
    
    public LikeRepository(CheepDbContext context)
    {
        _context = context;
    }
    
    
    public async Task AddLike(string authorName, int cheepId)
    {
        var like = new Like()
        {
            AuthorName = authorName,
            CheepId = cheepId
        };
        
        await _context.Likes.AddAsync(like);
        await _context.SaveChangesAsync();
    }


    public async Task RemoveLike(string authorName, int cheepId)
    {
        var like = _context.Likes.FirstOrDefault(l => l.AuthorName == authorName && l.CheepId == cheepId);

        if (like != null)
        {
            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();
        }
    }


    public Task GetLiked(string authorName)
    {
        var query = (from like in _context.Likes
                where like.AuthorName == authorName
                    select like);
        return query.ToListAsync();
        
    }
}