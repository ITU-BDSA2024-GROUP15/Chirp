using Chirp.Razor;
using Chirp.Razor.Datamodel;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection"); //Takes default connection from appsettings.json to use for db


builder.Services.AddDbContext<CheepDbContext>(options => options.UseSqlite(connectionString));


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<ICheepRepository, CheepRepository>();
builder.Services.AddScoped<ICheepService, CheepService>();

var app = builder.Build();


using ( var serviceScope = app.Services.CreateScope() )
{
    var context = serviceScope.ServiceProvider.GetRequiredService<CheepDbContext>();
    
    DbInitializer.SeedDatabase(context);
}
//DbInitializer.SeedDatabase( app.Services.GetService<CheepDBContext>() );

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();

app.Run();

public partial class Program {}