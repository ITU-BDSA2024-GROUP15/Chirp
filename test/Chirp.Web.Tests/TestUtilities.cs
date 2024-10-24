﻿using Chirp.Core;
using Chirp.Infrastructure.Chirp.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Web.Tests;

public static class TestUtilities
{
    
    public static SqliteConnection Connection { get; set; }
    public static async Task<ICheepRepository> createInMemoryDB()
    {
        Connection = new SqliteConnection("Filename=:memory:");
        await Connection.OpenAsync();
        var builder = new DbContextOptionsBuilder<CheepDbContext>().UseSqlite(Connection);

        var context = new CheepDbContext(builder.Options);
        await context.Database.EnsureCreatedAsync(); // Applies the schema to the database

        ICheepRepository repository = new CheepRepository(context);
        DbInitializer.SeedDatabase(context);
        return repository;
    }


    public static void closeConnection()
    {
        Connection.Close();
    }
    
    
    
}