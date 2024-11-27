using System.ComponentModel;
using System.Net;
using Chirp.Core;
using Chirp.Web;
using Chirp.Infrastructure.Chirp.Repositories;
using Chirp.Infrastructure.Chirp.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;
using Xunit.Abstractions;

namespace Chirp.Web.Tests;


public class IntegrationTests
{
    
    [Fact]
    public async Task TestAddCheep()
    {
        var utils = new TestUtilities();
        var context = await utils.CreateInMemoryDb();
        IAuthorRepository authorrepo = new AuthorRepository(context); 
        ICheepRepository cheeprepo = new CheepRepository(context); 
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
        var utils = new TestUtilities();
        var context = await utils.CreateInMemoryDb();
        IAuthorRepository authorrepo = new AuthorRepository(context); 
        ICheepRepository cheeprepo = new CheepRepository(context);
        IFollowRepository followrepo = new FollowRepository(context);

        ICheepService service = new CheepService(cheeprepo, authorrepo, followrepo);
        
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
    public async Task TestDeleteFollows()
    {
        var utils = new TestUtilities();
        var context = await utils.CreateInMemoryDb();
        
       
        IAuthorRepository authorrepo = new AuthorRepository(context);
        IFollowRepository followrepo = new FollowRepository(context);
        ICheepRepository cheeprepo = new CheepRepository(context);
        ICheepService cheepService = new CheepService(cheeprepo, authorrepo, followrepo);
        await authorrepo.CreateAuthor("erik", "hahaemail@gmail.com");
        await authorrepo.CreateAuthor("lars", "bahaemail@gmail.com");
        await followrepo.AddFollowing("erik", "lars");
        var oldfollows = await cheepService.GetFollowed("erik");
        Assert.True(oldfollows.Count == 1);
        await cheepService.DeleteFromFollows("lars");
        
        var newfollows = await cheepService.GetFollowed("erik");
        
        Assert.True(newfollows.Count == 0);


    }
    
}
