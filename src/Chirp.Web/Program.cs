using Chirp.Core;
using Chirp.Core.Data;
using Chirp.Infrastructure.Chirp.Repositories;
using Chirp.Infrastructure.Chirp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);


string? connectionString = builder.Configuration.GetConnectionString("CheepDbContextConnection"); //Takes default connection from appsettings.json to use for db


builder.Services.AddDbContext<CheepDbContext>(options => options.UseSqlite(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>   
        options.SignIn.RequireConfirmedAccount = true)            
    .AddEntityFrameworkStores<CheepDbContext>(); 


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<ICheepRepository, CheepRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<ICheepService, CheepService>();

var app = builder.Build();


using ( var serviceScope = app.Services.CreateScope() )
{
    var context = serviceScope.ServiceProvider.GetRequiredService<CheepDbContext>();

    context.Database.EnsureCreated();
    
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

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();

public partial class Program {}