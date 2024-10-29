using Chirp.Core.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Chirp.Core;

using Microsoft.EntityFrameworkCore;


public class CheepDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Author> Authors { get; set; }
    public DbSet<Cheep> Cheeps { get; set; }


    //Constructor
    public CheepDbContext(DbContextOptions<CheepDbContext> options) : base(options)
    {   
        
    }
}