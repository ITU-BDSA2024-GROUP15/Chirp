﻿using Chirp.Core;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure.Chirp.Repositories;

public class CheepRepository : ICheepRepository
{

    //This should handle data logic for cheep
    
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
        var query = from cheep in _context.Cheeps
            where (from follow in _context.Follows
                    where follow.FollowsAuthorName == author
                    select follow.FollowsAuthorName)
                .Contains(cheep.Author.Name)
            select cheep;
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
}