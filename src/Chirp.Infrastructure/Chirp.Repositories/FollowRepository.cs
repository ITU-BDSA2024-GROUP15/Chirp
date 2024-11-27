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
    
    public async Task AddFollowing(string follower, string Followed)
    {

        var follow = new Follow()
        {
            Follower = follower,
            Followed = Followed
        };
        
        await _context.Follows.AddAsync(follow);
        await _context.SaveChangesAsync();
    }


    public async Task RemoveFollowing(string follower, string follwed)
    {
        //https://stackoverflow.com/questions/30928566/how-to-delete-a-row-from-database-using-lambda-linq
        
        
        //We first find the follow that we want to remove
        var follow = _context.Follows.FirstOrDefault(follow => follow.Follower == follower && follow.Followed == follwed);

        if ( follow != null )
        {
            _context.Follows.Remove(follow);
            await _context.SaveChangesAsync();
        }
        
    }


    /// <summary>
    /// Gets a list of Authors that an author follows. Follower is the name of
    /// the author that follows.
    /// </summary>
    /// <param name="follower"></param>
    /// <returns></returns>
    public async Task<List<Follow>> GetFollowed(string follower)
    {
        var query = (from follow in _context.Follows
            where follow.Follower == follower
            select follow);
        var result = await query.ToListAsync();
        return result;

    }
    
    public async Task<List<Follow>> GetFollowers(string followed)
    {
        var query = (from follow in _context.Follows
            where follow.Followed == followed
            select follow);
        var result = await query.ToListAsync();
        return result;

    }
}