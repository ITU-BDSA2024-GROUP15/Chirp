using System.Runtime.InteropServices.JavaScript;
using Chirp.Core;
using Chirp.Infrastructure.Chirp.Repositories;
using Chirp.Infrastructure.Chirp.Services;
using Chirp.Web.Pages;
using Microsoft.AspNetCore.Identity.UI.V5.Pages.Account.Manage.Internal;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Chirp.Web.Tests;

public class UnitTests
{
    
    //CheepRepository
    [Fact]
    public async Task TestGetCheepsAmount()
    {
        var utils = new TestUtilities();
        var context = await utils.CreateInMemoryDb();
        
        ICheepRepository repository = new CheepRepository(context);        
        var cheeps = await repository.GetCheeps(0);
        Assert.Equal(32,cheeps.Count);
        await utils.CloseConnection();
        
    }
    
    [Fact]
    public async Task TestGetCheepsPage1()
    {
        var utils = new TestUtilities();
        var context = await utils.CreateInMemoryDb();
        ICheepRepository repository = new CheepRepository(context);     
        var cheeps = await repository.GetCheeps(0);
        var cheep = cheeps[0];
        Assert.Equal("Starbuck now is what we hear the worst.", cheep.Text);
        await utils.CloseConnection();
    }
    
    [Fact]
    public async Task TestGetCheepsPage2()
    {
        var utils = new TestUtilities();
        var context = await utils.CreateInMemoryDb();
        ICheepRepository repository = new CheepRepository(context);     
        var cheeps = await repository.GetCheeps(2);
        var cheep = cheeps[0];
        Assert.Equal("In the morning of the wind, some few splintered planks, of what present avail to him.", cheep.Text);  
        await utils.CloseConnection();
    }
    
    
    [Fact]
    public async Task TestGetCheepsLastPage() 
    {
        var utils = new TestUtilities();
        var context = await utils.CreateInMemoryDb();
        ICheepRepository repository = new CheepRepository(context);     
        var cheeps = await repository.GetCheeps(21);
        
        Assert.True(cheeps.Count == 17);
        await utils.CloseConnection();
    }
    
    [Fact]
    public async Task TestGetCheepsBeyondLimit() 
    {   
        var utils = new TestUtilities();
        var context = await utils.CreateInMemoryDb();
        ICheepRepository repository = new CheepRepository(context);     
        var cheeps = await repository.GetCheeps(32);
        
        Assert.True(cheeps.Count == 0);    
        await utils.CloseConnection();
    }
    
    
    [Fact]
    public async Task TestGetCheepsFromAuthor() 
    {   
        var utils = new TestUtilities();
        var context = await utils.CreateInMemoryDb();
        ICheepRepository repository = new CheepRepository(context);     
        var cheeps = await repository.GetCheepsFromAuthor(0, "Jacqualine Gilcoine");
        var cheep = cheeps[0];
        //we know Jacqualine is the first author on the public timeline.
        Assert.True(cheep.Author.Name == "Jacqualine Gilcoine"  && cheeps.Count == 32);
        await utils.CloseConnection();
    }
    
    [Fact]
    public async Task TestGetCheepsFromAuthorPage2()
    {
        var utils = new TestUtilities();
        var context = await utils.CreateInMemoryDb();
        ICheepRepository repository = new CheepRepository(context);     
        var cheeps = await repository.GetCheepsFromAuthor(2, "Jacqualine Gilcoine");
        var cheep = cheeps[0];
        
        Assert.Equal("What a relief it was the place examined.", cheep.Text);
        await utils.CloseConnection();
    }    
    
    [Fact]
    public async Task TestGetCheepsFromAuthorLastPage()
    {
        var utils = new TestUtilities();
        var context = await utils.CreateInMemoryDb();
        ICheepRepository repository = new CheepRepository(context);     
        var cheeps = await repository.GetCheepsFromAuthor(12, "Jacqualine Gilcoine");
        var cheep = cheeps[0];
        
        Assert.True(cheeps.Count == 7 && cheep.Author.Name == "Jacqualine Gilcoine"); 
        await utils.CloseConnection();
    }
    
    [Fact]
    public async Task TestGetCheepsFromAuthorBeyondLimit()
    {
        var utils = new TestUtilities();
        var context = await utils.CreateInMemoryDb();
        ICheepRepository repository = new CheepRepository(context);     
        var cheeps = await repository.GetCheepsFromAuthor(20, "Jacqualine Gilcoine");
        
        Assert.True(cheeps.Count == 0);   
        await utils.CloseConnection();
    }
    

    [Fact]
    public async Task TestCreateAuthor()
    {
        var utils = new TestUtilities();
        var context = await utils.CreateInMemoryDb();
        IAuthorRepository repository = new AuthorRepository(context);

        await repository.CreateAuthor("Filifjonken", "fili@mail.com");
        
        Author author = await repository.GetAuthorByName("Filifjonken");
        
        Assert.True(author.Name == "Filifjonken");   
        await utils.CloseConnection();
        
    }


    [Fact]
    public async Task TestGetAllCheepsFromAuthor()
    {
        var utils = new TestUtilities();
        var context = await utils.CreateInMemoryDb();
        ICheepRepository repository = new CheepRepository(context);
        var result = ( await repository.GetAllCheepsFromAuthor("Adrian") ).Count;
        
        Assert.Equal(1, result);
    }


    [Fact]
    public async Task TestCheepConstraintOnDataModel()
    {
        var utils = new TestUtilities();
        var context = await utils.CreateInMemoryDb();
        IAuthorRepository authorrepo = new AuthorRepository(context); 
        ICheepRepository cheeprepo = new CheepRepository(context);

        var cheepsBefore = (await cheeprepo.GetAllCheepsFromAuthor("Mellie Yost")).Count;
        Author author = await authorrepo.GetAuthorByName("Mellie Yost");
        var invalidCheep = new String('a', 161);
        await cheeprepo.AddCheep(invalidCheep, author);
        
        var cheepsAfter = (await cheeprepo.GetAllCheepsFromAuthor("Mellie Yost")).Count;

        Assert.Equal(cheepsBefore, cheepsAfter);
    }


    [Fact]
    public async Task CanAddFolowerToDb() 
    {
        var utils = new TestUtilities();
        var context = await utils.CreateInMemoryDb();
        
        IAuthorRepository authorrepo = new AuthorRepository(context);

        Author author1 = new Author()
        {
            Id = 1,
            Name = "Test1",
            Email = "test1@mail.com",
        };
        
        Author author2 = new Author()
        {
            Id = 2,
            Name = "Test2",
            Email = "test2@mail.com",
        };

        await authorrepo.AddFollowing(author1.Id, author1.Name, author2.Id, author2.Name);

        var follow = await context.Follows.FirstOrDefaultAsync();
        
        Assert.NotNull(follow);
        Assert.Equal(author1.Id, follow.AuthorId);

    }

    [Fact]
    public async Task CanRemoveFollowerFromDb()
    {
        var utils = new TestUtilities();
        var context = await utils.CreateInMemoryDb();
        
        IAuthorRepository authorrepo = new AuthorRepository(context);

        Author author1 = new Author()
        {
            Id = 1,
            Name = "Test1",
            Email = "test1@mail.com",
        };
        
        Author author2 = new Author()
        {
            Id = 2,
            Name = "Test2",
            Email = "test2@mail.com",
        };

        await authorrepo.AddFollowing(author1.Id, author1.Name, author2.Id, author2.Name);

        var follow = await context.Follows.FirstOrDefaultAsync();
        
        //Check that the Follow has been added
        Assert.NotNull(follow);

        await authorrepo.RemoveFollowing(author1.Id, author1.Name);
        
        //Check that the follow has been removed
        var followRemoved = await context.Follows.FirstOrDefaultAsync();
        Assert.Null(followRemoved);
        
    }


    [Fact]
    public async Task CanGetFollowFromDb()
    {
        var utils = new TestUtilities();
        var context = await utils.CreateInMemoryDb();
        
        IAuthorRepository authorrepo = new AuthorRepository(context);

        Author author1 = new Author()
        {
            Id = 1,
            Name = "Test1",
            Email = "test1@mail.com",
        };
        
        Author author2 = new Author()
        {
            Id = 2,
            Name = "Test2",
            Email = "test2@mail.com",
        };

        await authorrepo.AddFollowing(author1.Id, author1.Name, author2.Id, author2.Name);
        
        var follows = await authorrepo.GetFollowing(author1.Id, author1.Name);
        
        Assert.NotNull(follows);
        Assert.Equal(author2.Name, follows[0].FollowsAuthorName);
    }
    
    
    [Fact]
    public async Task CanGetFollowsFromDb()
    {
        var utils = new TestUtilities();
        var context = await utils.CreateInMemoryDb();
        
        IAuthorRepository authorrepo = new AuthorRepository(context);

        Author author1 = new Author()
        {
            Id = 1,
            Name = "Test1",
            Email = "test1@mail.com",
        };
        
        Author author2 = new Author()
        {
            Id = 2,
            Name = "Test2",
            Email = "test2@mail.com",
        };
        
        Author author3 = new Author()
        {
            Id = 3,
            Name = "Test3",
            Email = "test3@mail.com",
        };

        await authorrepo.AddFollowing(author1.Id, author1.Name, author2.Id, author2.Name);
        await authorrepo.AddFollowing(author1.Id, author1.Name, author3.Id, author3.Name);
        
        var follows = await authorrepo.GetFollowing(author1.Id, author1.Name);
        
        Assert.NotNull(follows);
        Assert.Equal(author2.Name, follows[0].FollowsAuthorName);
        Assert.Equal(author3.Name, follows[1].FollowsAuthorName);
    }

    [Fact]
    public async Task CanAddFolowerToDbWithCheepService() 
    {
        var utils = new TestUtilities();
        var context = await utils.CreateInMemoryDb();
        
        IAuthorRepository authorrepo = new AuthorRepository(context); 
        ICheepRepository cheeprepo = new CheepRepository(context);
        
        ICheepService service = new CheepService(cheeprepo, authorrepo);

        Author author1 = new Author()
        {
            Id = 1,
            Name = "Test1",
            Email = "test1@mail.com",
        };
        
        Author author2 = new Author()
        {
            Id = 2,
            Name = "Test2",
            Email = "test2@mail.com",
        };

        await service.AddFollowing(author1.Id, author1.Name, author2.Id, author2.Name);

        var follow = await context.Follows.FirstOrDefaultAsync();
        
        Assert.NotNull(follow);
        Assert.Equal(author1.Id, follow.AuthorId);

    }
    
    [Fact]
    public async Task TestQueryCheepsFromFollow()
    {
        var utils = new TestUtilities();
        var context = await utils.CreateInMemoryDb();
        
        ICheepRepository cheeprepo = new CheepRepository(context);
        
        
    }
    
    
}