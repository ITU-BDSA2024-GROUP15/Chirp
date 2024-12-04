using Chirp.Core;
using Chirp.Infrastructure.Chirp.Repositories;
using Chirp.Infrastructure.Chirp.Services;
using Chirp.Infrastructure.data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection"); //Takes default connection from appsettings.json to use for db


builder.Services.AddDbContext<CheepDbContext>(options => options.UseSqlite(connectionString));

builder.Services.AddDefaultIdentity<Author>(options =>   
        options.SignIn.RequireConfirmedAccount = true)            
    .AddEntityFrameworkStores<CheepDbContext>(); 

builder.Services.AddSession();

builder.Services.AddAuthentication()
    .AddCookie()
    .AddGitHub(o =>
    {
        o.ClientId = builder.Configuration["authentication:github:clientId"] ?? string.Empty;
        o.ClientSecret = builder.Configuration["authentication:github:clientSecret"] ?? string.Empty;
        o.CallbackPath = "/signin-github";
        o.Scope.Add("user:email");
        o.Scope.Add("read:user");
    });


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<ICheepRepository, CheepRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IFollowRepository, FollowRepository>();
builder.Services.AddScoped<IChirpService, ChirpService>();

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
app.UseSession();

app.MapRazorPages();

app.Run();

public partial class Program {}