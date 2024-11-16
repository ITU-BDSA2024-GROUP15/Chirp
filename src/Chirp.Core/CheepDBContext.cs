
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Chirp.Core;

using Microsoft.EntityFrameworkCore;


public class CheepDbContext : IdentityDbContext<Author, IdentityRole<int>, int> //Overriden method to make primary key int
{
    public DbSet<Author> Authors { get; set; }
    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Follow> Follows { get; set; }


    //Constructor
    public CheepDbContext(DbContextOptions<CheepDbContext> options) : base(options)
    {   
        
    }
}