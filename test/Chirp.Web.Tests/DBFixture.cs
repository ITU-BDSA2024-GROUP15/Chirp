using Chirp.Core;
using Chirp.Infrastructure.Chirp.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Chirp.Web.Tests;

public class DBFixture : IAsyncLifetime
{
    //Inspiration:
    //https://xunit.net/docs/shared-context
    //https://stackoverflow.com/questions/25519155/await-tasks-in-test-setup-code-in-xunit-net

    public ICheepRepository CheepRepository { get; private set; }
    //we can then add a author repo
    
    public SqliteConnection DatabaseConnection { get; private set; }
    


    public async Task InitializeAsync()
    {
        DatabaseConnection = new SqliteConnection("Filename=:memory:");
        await DatabaseConnection.OpenAsync();
        var builder = new DbContextOptionsBuilder<CheepDbContext>().UseSqlite(DatabaseConnection);

        
        
        var context = new CheepDbContext(builder.Options);
        await context.Database.EnsureCreatedAsync(); // Applies the schema to the database
        DbInitializer.SeedDatabase(context); //Here we could make our own data

        CheepRepository = new CheepRepository(context);
        
    }


    
    public Task? DisposeAsync()
    {
        //throw new NotImplementedException();
        DatabaseConnection.Dispose();
        return null;
    }

    
}