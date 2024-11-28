using Chirp.Core;

namespace Chirp.Infrastructure.Chirp.Repositories;

public class LikeRepository : ILikeRepository
{
    private readonly CheepDbContext _context;
    
    
    public LikeRepository(CheepDbContext context)
    {
        _context = context;
    }
    
    
    public async Task AddLike(string authorName, Cheep cheep)
    {
        var like = new Like()
        {
            AuthorName = authorName,
            CheepId = cheep.CheepId
        };
        
        await _context.Likes.AddAsync(like);
        await _context.SaveChangesAsync();
    }


    public async Task RemoveLike(string authorName, Cheep cheep)
    {
        var like = _context.Likes.FirstOrDefault(l => l.AuthorName == authorName && l.CheepId == cheep.CheepId);

        if (like != null)
        {
            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();
        }
    }
}