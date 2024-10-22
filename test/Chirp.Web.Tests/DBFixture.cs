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
    


    public async Task InitializeAsync()
    {
        using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        var builder = new DbContextOptionsBuilder<CheepDbContext>().UseSqlite(connection);

        var context = new CheepDbContext(builder.Options);
        await context.Database.EnsureCreatedAsync(); // Applies the schema to the database

        CheepRepository = new CheepRepository(context);
        DbInitializer.SeedDatabase(context);
    }


    
    public Task DisposeAsync()
    {
        throw new NotImplementedException();
    }

    
}