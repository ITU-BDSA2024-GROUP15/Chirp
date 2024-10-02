using Microsoft.EntityFrameworkCore;

namespace Chirp.Razor.Datamodel;

public class CheepDBContext : DbContext //Used to give context to db
{
    DbSet<Author> Authors { get; set; }
    DbSet<Cheep> Cheeps { get; set; }


    //Constructor
    public CheepDBContext(DbContextOptions<CheepDBContext> options) : base(options)
    {
        
    }
}