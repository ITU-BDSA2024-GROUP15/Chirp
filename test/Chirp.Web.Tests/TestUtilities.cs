using Chirp.Core;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Web.Tests;

public static class TestUtilities
{
    
    public static SqliteConnection? Connection { get; set; }
    public static async Task<CheepDbContext> CreateInMemoryDb()
    {
        Connection = new SqliteConnection("Filename=:memory:");
        await Connection.OpenAsync();
        var builder = new DbContextOptionsBuilder<CheepDbContext>().UseSqlite(Connection);

        var context = new CheepDbContext(builder.Options);
        await context.Database.EnsureCreatedAsync(); // Applies the schema to the database
        DbInitializer.SeedDatabase(context);
      
        
        return context;
    }
    
    
    


    public static void CloseConnection()
    {
        if ( Connection != null ) Connection.Close();
    }
    
    
    
}