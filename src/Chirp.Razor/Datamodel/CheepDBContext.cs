using Microsoft.EntityFrameworkCore;

namespace Chirp.Razor.Datamodel;

public class CheepDBContext : DbContext
{
    DbSet<Author> Authors { get; set; }
    private DbSet<Cheep> Cheeps { get; set; }


    //Constructor
    public CheepDBContext(DbContextOptions<CheepDBContext> options) : base(options)
    {
        
    }
}