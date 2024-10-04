﻿using Microsoft.EntityFrameworkCore;

namespace Chirp.Razor.Datamodel;

public class CheepDBContext : DbContext //Used to give context to db
{
    public DbSet<Author> Authors { get; set; }
    public DbSet<Cheep> Cheeps { get; set; }


    //Constructor
    public CheepDBContext(DbContextOptions<CheepDBContext> options) : base(options)
    {
        
    }
}