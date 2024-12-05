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

    private IAuthorRepository authorrepo;
    private ICheepRepository cheeprepo;
    private IFollowRepository followrepo;

    private TestUtilities utils;
    private CheepDbContext context;
    
    private IChirpService service;
    
    public async Task InitializeAsync()
    {
        utils = new TestUtilities();
        context = await utils.CreateInMemoryDb();
        authorrepo = new AuthorRepository(context); 
        cheeprepo = new CheepRepository(context);
        followrepo = new FollowRepository(context);

        service = new ChirpService(cheeprepo, authorrepo, followrepo);
    }


    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
    
    
    [Fact]
    public async Task TestAddCheep()
    {
        var cheepsBefore = await cheeprepo.GetCheepsFromAuthor(0, "Mellie Yost");
        
        Author author = await authorrepo.GetAuthorByName("Mellie Yost");
        await cheeprepo.AddCheep("hejj", author);
        
        var cheepsAfter = await cheeprepo.GetCheepsFromAuthor(0, "Mellie Yost");
        
        Assert.True(cheepsBefore.Count != cheepsAfter.Count);
        
        await utils.CloseConnection();
    }
    


    
    [Fact]
    public async Task AddCheepCheepServiceNonExistingAuthor()
    {
        await service.AddCheep("testest", "NewAuthor", "@newauthor.com");

        Author author = await authorrepo.GetAuthorByName("NewAuthor");
        
        Assert.NotNull(author);
        
        await utils.CloseConnection();
        
    }
    
    
    [Fact]
    public async Task TestUsernameCannotContainSlash()
    {
        var utils = new TestUtilities();
        var context = await utils.CreateInMemoryDb();
        
        ICheepRepository cheeprepo = new CheepRepository(context);
        IAuthorRepository authorrepo = new AuthorRepository(context);

        await Assert.ThrowsAsync<ArgumentException>(() =>
            authorrepo.CreateAuthor("/Haha", "hahaemail@gmail.com"));

    }


    [Fact]
    public async Task TestGetAllCheepsFromTimelineAndFollow()
    {
        //We first make a user follow another user. Show that the cheeps should now be the sum of their cheeps in their private timeline
        //Octavio Wagganer(15) follow Mellie Yost(7)

        string authorname1 = "Octavio Wagganer";
        string authorname2 = "Mellie Yost";

        await service.AddFollowing(authorname1, authorname2);
        var cheeps = await service.GetCheepsForTimeline(authorname1, 1);
        
        Assert.Equal(22, cheeps.Count());

    }


    [Fact]
    public async Task TestCheepHasFollowIfAuthorFollows()
    {
        string authorname1 = "Octavio Wagganer";
        string authorname2 = "Mellie Yost";

        await service.AddFollowing(authorname1, authorname2);

        var cheeps = await service.GetCheepsForTimeline(authorname1, 1);
        var cheep = cheeps.First();
        
        Assert.True(cheep.Follows);
    }


    [Fact]
    public async Task TestDeleteFollows()
    {
        await authorrepo.CreateAuthor("erik", "hahaemail@gmail.com");
        await authorrepo.CreateAuthor("lars", "bahaemail@gmail.com");
        await followrepo.AddFollowing("erik", "lars");
        var oldfollows = await service.GetFollowedDtos("erik");
        Assert.True(oldfollows.Count == 1);
        await service.DeleteFromFollows("lars");
        
        var newfollows = await service.GetFollowedDtos("erik");
        
        Assert.True(newfollows.Count == 0);


    }

    [Fact]
    public async Task TestCountingOfLikesOnCheep()
    {
        await service.AddLike("Octavio Wagganer", 1);
        await service.AddLike("Mellie Yost", 1);

        var likes = await service.CountLikes(1);
        
        Assert.Equal(2, likes);
    }


    [Fact]
    public async Task CanRemoveLike()
    {
        await service.AddLike("Octavio Wagganer", 1);
        var likes1Amount = await service.CountLikes(1);
        await service.RemoveLike("Octavio Wagganer", 1);
        var likes2Amount = await service.CountLikes(1);
        
        Assert.True(likes1Amount != likes2Amount);
    }


    [Fact]
    public async Task CanRemoveLikeData()
    {
        string author = "Octavio Wagganer";
        await service.AddLike(author, 1);
        var likes1Amount = await service.CountLikes(1);
        await service.DeleteAllLikes(author);
        var likes2Amount = await service.CountLikes(1);
        
        Assert.True(likes1Amount != likes2Amount);
        Assert.True(likes2Amount == 0);
    }

}
