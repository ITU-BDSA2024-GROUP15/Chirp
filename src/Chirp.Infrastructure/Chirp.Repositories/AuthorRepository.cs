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


    public async Task AddFollowing(int authorId, Author followerAuthor, int followingId, Author followsAuthor)
    {

        var follow = new Follow()
        {
            AuthorId = authorId,
            Author = followerAuthor,
            FollowsId = followingId,
            Follows = followsAuthor
        };
        
        await _context.Follows.AddAsync(follow);
        await _context.SaveChangesAsync();
    }


    public async Task RemoveFollowing(int authorId, Author followerAuthor, int followingId, Author followsAuthor)
    {
        var follow = new Follow()
        {
            AuthorId = authorId,
            Author = followerAuthor,
            FollowsId = followingId,
            Follows = followsAuthor
        };

        _context.Follows.Remove(follow);
        await _context.SaveChangesAsync();
    }
}