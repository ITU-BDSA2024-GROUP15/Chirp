﻿using System.ComponentModel.DataAnnotations;
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
        
        if ( result.Count == 0 )
        {
            return null;
        }
        return result[0];
    }


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
    /// Gets a list of Authors that an author follows. FollowerAuthorName is the id and name of
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