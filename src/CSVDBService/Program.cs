var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//Returns cheeps to the user
app.MapGet("/cheeps", () => { ... });

//Add a cheep to the database
app.MapPost("/cheep", (Cheep cheep) => { ... });

app.Run();
