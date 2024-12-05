using System.ComponentModel;
using System.Net;
using Chirp.Core;
using Chirp.Web;
using Chirp.Infrastructure.Chirp.Repositories;
using Chirp.Infrastructure.Chirp.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using NUnit.Framework;
using Xunit;
using Xunit.Abstractions;
using Assert = Xunit.Assert;

namespace Chirp.Web.Tests;


public class IntegrationTests : IAsyncLifetime
{

    private IAuthorRepository _authorrepo;
    private ICheepRepository _cheeprepo;
    private IFollowRepository _followrepo;

    private TestUtilities _utils;
    private CheepDbContext _context;
    
    private IChirpService _service;
    
    public async Task InitializeAsync()
    {
        _utils = new TestUtilities();
        _context = await _utils.CreateInMemoryDb();
        _authorrepo = new AuthorRepository(_context); 
        _cheeprepo = new CheepRepository(_context);
        _followrepo = new FollowRepository(_context);

        _service = new ChirpService(_cheeprepo, _authorrepo, _followrepo);
    }


    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
    
    
    [Fact]
    public async Task TestAddCheep()
    {
        var cheepsBefore = await _cheeprepo.GetCheepsFromAuthor(0, "Mellie Yost");
        
        Author author = await _authorrepo.GetAuthorByName("Mellie Yost");
        await _cheeprepo.AddCheep("hejj", author);
        
        var cheepsAfter = await _cheeprepo.GetCheepsFromAuthor(0, "Mellie Yost");
        
        Assert.True(cheepsBefore.Count != cheepsAfter.Count);
        
        await _utils.CloseConnection();
    }
    


    
    [Fact]
    public async Task AddCheepchirpServiceNonExistingAuthor()
    {
        await _service.AddCheep("testest", "NewAuthor", "@newauthor.com");

        Author author = await _authorrepo.GetAuthorByName("NewAuthor");
        
        Assert.NotNull(author);
        
        await _utils.CloseConnection();
        
    }
    
    [Fact]
    public async Task TestGetAllCheepsFromTimelineAndFollow()
    {
        //We first make a user follow another user. Show that the cheeps should now be the sum of their cheeps in their private timeline
        //Octavio Wagganer(15) follow Mellie Yost(7)

        string authorname1 = "Octavio Wagganer";
        string authorname2 = "Mellie Yost";

        await _service.AddFollowing(authorname1, authorname2);
        var cheeps = await _service.GetCheepsForTimeline(authorname1, 1);
        
        Assert.Equal(22, cheeps.Count());

    }


    [Fact]
    public async Task TestCheepHasFollowIfAuthorFollows()
    {
        string authorname1 = "Octavio Wagganer";
        string authorname2 = "Mellie Yost";

        await _service.AddFollowing(authorname1, authorname2);

        var cheeps = await _service.GetCheepsForTimeline(authorname1, 1);
        var cheep = cheeps.First();
        
        Assert.True(cheep.Follows);
    }


    [Fact]
    public async Task TestDeleteFollows()
    {
        await _authorrepo.CreateAuthor("erik", "hahaemail@gmail.com");
        await _authorrepo.CreateAuthor("lars", "bahaemail@gmail.com");
        await _followrepo.AddFollowing("erik", "lars");
        var oldfollows = await _service.GetFollowedDtos("erik");
        Assert.True(oldfollows.Count == 1);
        await _service.DeleteFromFollows("lars");
        
        var newfollows = await _service.GetFollowedDtos("erik");
        
        Assert.True(newfollows.Count == 0);


    }

    [Fact]
    public async Task TestCountingOfLikesOnCheep()
    {
        await _service.AddLike("Octavio Wagganer", 1);
        await _service.AddLike("Mellie Yost", 1);

        var likes = await _service.CountLikes(1);
        
        Assert.Equal(2, likes);
    }


    [Fact]
    public async Task CanRemoveLike()
    {
        await _service.AddLike("Octavio Wagganer", 1);
        var likes1Amount = await _service.CountLikes(1);
        await _service.RemoveLike("Octavio Wagganer", 1);
        var likes2Amount = await _service.CountLikes(1);
        
        Assert.True(likes1Amount != likes2Amount);
    }


    [Fact]
    public async Task CanRemoveLikeData()
    {
        string author = "Octavio Wagganer";
        await _service.AddLike(author, 1);
        var likes1Amount = await _service.CountLikes(1);
        await _service.DeleteAllLikes(author);
        var likes2Amount = await _service.CountLikes(1);
        
        Assert.True(likes1Amount != likes2Amount);
        Assert.True(likes2Amount == 0);
    }

}
