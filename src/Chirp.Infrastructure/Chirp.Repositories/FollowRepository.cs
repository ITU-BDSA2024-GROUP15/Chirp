using Chirp.Core;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure.Chirp.Repositories;

/// <summary>
/// Used to handle data logic for Follows.
/// Includes methods for accessing and handling follow data.
/// </summary>
public class FollowRepository : IFollowRepository
{
    private readonly CheepDbContext _context;


    public FollowRepository(CheepDbContext context)
    {
        _context = context;
    }
    
    
    public async Task AddFollowing(string followerName, string followedName)
    {

        var follow = new Follow()
        {
            Follower = followerName,
            Followed = followedName
        };
        
        await _context.Follows.AddAsync(follow);
        await _context.SaveChangesAsync();
    }


   
    public async Task RemoveFollowing(string followerName, string follwedName)
    {
        //lambda expression inspiration:https://stackoverflow.com/questions/30928566/how-to-delete-a-row-from-database-using-lambda-linq
        //We first find the follow that we want to remove
        var follow = _context.Follows.FirstOrDefault(follow => follow.Follower == followerName && follow.Followed == follwedName);

        if ( follow != null )
        {
            _context.Follows.Remove(follow);
            await _context.SaveChangesAsync();
        }
        
    }


    
    public async Task<List<Follow>> GetFollowed(string followerName)
    {
        var query = (from follow in _context.Follows
            where follow.Follower == followerName
            select follow);
        var result = await query.ToListAsync();
        return result;

    }
    
    public async Task<List<Follow>> GetFollowers(string followedName)
    {
        var query = (from follow in _context.Follows
            where follow.Followed == followedName
            select follow);
        var result = await query.ToListAsync();
        return result;

    }
}