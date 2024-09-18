using SimpleDB;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

CSVDatabase<Cheep> database = CSVDatabase<Cheep>.GetInstance();

//Returns cheeps to the user
app.MapGet("/cheeps", (int? limit) =>
{
    return database.Read(limit);
});

//Add a cheep to the database
app.MapPost("/cheep", (Cheep cheep) =>
{
    database.Store(cheep); 
});

app.Run();

public record Cheep(string Author, string Message, long Timestamp);