using Chirp.Core;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure.Chirp.Repositories;

public class FollowRepository : IFollowRepository
{
    private readonly CheepDbContext _context;


    public FollowRepository(CheepDbContext context)
    {
        _context = context;
    }
    
    public async Task AddFollowing(string followerAuthorName, string followsAuthorName)
    {

        var follow = new Follow()
        {
            AuthorName = followerAuthorName,
            FollowsAuthorName = followsAuthorName
        };
        
        await _context.Follows.AddAsync(follow);
        await _context.SaveChangesAsync();
    }


    public async Task RemoveFollowing(string followerAuthorName, string followsAuthorName)
    {
        //https://stackoverflow.com/questions/30928566/how-to-delete-a-row-from-database-using-lambda-linq
        
        
        //We first find the follow that we want to remove
        var follow = _context.Follows.FirstOrDefault(follow => follow.AuthorName == followerAuthorName && follow.FollowsAuthorName == followsAuthorName);

        if ( follow != null )
        {
            _context.Follows.Remove(follow);
            await _context.SaveChangesAsync();
        }
        
    }


    /// <summary>
    /// Gets a list of Authors that an author follows. FollowerAuthorName is the name of
    /// the author that follows.
    /// </summary>
    /// <param name="authorId"></param>
    /// <param name="followerAuthorName"></param>
    /// <returns></returns>
    public async Task<List<Follow>> GetFollowed(string followerAuthorName)
    {
        var query = (from follow in _context.Follows
            where follow.AuthorName == followerAuthorName
            select follow);
        var result = await query.ToListAsync();
        return result;

    }
}