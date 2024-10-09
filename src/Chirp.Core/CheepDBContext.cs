namespace Chirp.Core;

using Microsoft.EntityFrameworkCore;


public class CheepDbContext : DbContext //Used to give context to db
{
    public DbSet<Author> Authors { get; set; }
    public DbSet<Cheep> Cheeps { get; set; }


    //Constructor
    public CheepDbContext(DbContextOptions<CheepDbContext> options) : base(options)
    {
        
    }
}