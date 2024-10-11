using Chirp.Core;
using Chirp.Infrastructure.Chirp.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Web.Tests;

public static class TestUtilities
{
    public static async Task<ICheepRepository> createInMemoryDB()
    {
        using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        var builder = new DbContextOptionsBuilder<CheepDbContext>().UseSqlite(connection);

        var context = new CheepDbContext(builder.Options);
        await context.Database.EnsureCreatedAsync(); // Applies the schema to the database

        ICheepRepository repository = new CheepRepository(context);
        DbInitializer.SeedDatabase(context);
        return repository;
    }
}