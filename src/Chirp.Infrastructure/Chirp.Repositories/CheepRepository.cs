﻿using Chirp.Core;
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


 
    public async Task<List<Cheep>> GetCheepsFromAuthor(int page, string authorName)
    {
        var query = (from cheep in _context.Cheeps
                where cheep.Author.Name == authorName
                orderby cheep.Timestamp descending
                select cheep)
            .Include(c => c.Author)
            .Skip((page - 1) * 32).Take(32);
        var result = await query.ToListAsync();
        
        
        return result;
    }
    
    
    
    public async Task<List<Cheep>> GetAllCheepsFromAuthor(string authorName)
    {
        var query = ( from cheep in _context.Cheeps
                where cheep.Author.Name == authorName
                orderby cheep.Timestamp descending
                select cheep )
            .Include(c => c.Author);
        var result = await query.ToListAsync();
        return result;
    }
    
    
    public async Task<List<Cheep>> GetAllCheepsFromFollowed(string authorName) //Made with the help of ChatGPT
    {
        var query = (from cheep in _context.Cheeps
            where (from follow in _context.Follows
                    where follow.Follower == authorName
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
            throw new ArgumentException("Text must be between 0 and 160 characters");
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


    public async Task AddLike(string authorName, int cheepId)
    {
        var currentLikes = await _context.Cheeps.FirstAsync(cheep =>cheep.CheepId == cheepId);
        if (!currentLikes.Likes.Contains(authorName))
        {
            currentLikes.Likes.Add(authorName);
            _context.SaveChanges();
        }
        
        
    }
    
    public async Task RemoveLike(string authorName, int cheepId)
    {
        var currentLikes = await _context.Cheeps.FirstAsync(cheep =>cheep.CheepId == cheepId);
        if (currentLikes.Likes.Contains(authorName))
        {
            currentLikes.Likes.Remove(authorName);
            _context.SaveChanges();
        }
    }

    
    public async Task<int> CountLikes(int cheepId)
    {
        var likes = await _context.Cheeps.FirstAsync(cheep =>cheep.CheepId == cheepId);
        return likes.Likes.Count;
    }
    
    public async Task<List<Cheep>> GetAllLiked(string authorName)
    {
        var likedCheeps = await _context.Cheeps.Where(cheep => cheep.Likes.Contains(authorName)).Include(c => c.Author).ToListAsync();
        
        return likedCheeps;
        
    }


    public async Task DeleteAllLikes(string authorName)
    {
        //https://stackoverflow.com/questions/1586013/how-to-do-select-all-in-linq-to-sql
        var likedCheeps = await _context.Cheeps.Where(cheep => cheep.Likes.Contains(authorName)).ToListAsync();

        foreach (var likes in likedCheeps)
        {
            likes.Likes.Remove(authorName);
        }
        _context.SaveChanges();
    }


    public async Task<List<Cheep>> GetTopLikedCheeps(int page) //This is not a great way to do it. But Keep It Simple Stupid
    {
        //https://stackoverflow.com/questions/5344805/linq-orderby-descending-query
        var query = (from cheep in _context.Cheeps
                .OrderByDescending(cheep => cheep.Likes.Count)
            select cheep).Skip((page -1) * 32).Take(32).Include(c => c.Author);
        
        var cheeps = await query.ToListAsync();

        return cheeps;
    }
    
    
}