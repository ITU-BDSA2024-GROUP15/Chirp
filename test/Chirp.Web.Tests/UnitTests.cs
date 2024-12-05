using System.Runtime.InteropServices.JavaScript;
using Chirp.Core;
using Chirp.Infrastructure.Chirp.Repositories;
using Chirp.Infrastructure.Chirp.Services;
using Chirp.Web.Pages;
using Microsoft.AspNetCore.Identity.UI.V5.Pages.Account.Manage.Internal;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Chirp.Web.Tests;

public class UnitTests : IAsyncLifetime
{

    private TestUtilities utils;
    private CheepDbContext context;
    private ICheepRepository cheepRepository;
    private IAuthorRepository authorRepository;
    private IFollowRepository followRepository;
    private IChirpService chirpService;

    public async Task InitializeAsync()
    {
        //Arrange
        utils = new TestUtilities();
        context = await utils.CreateInMemoryDb();
        cheepRepository = new CheepRepository(context);
        authorRepository = new AuthorRepository(context);
        followRepository = new FollowRepository(context);
        chirpService = new ChirpService(cheepRepository, authorRepository, followRepository);
        
    }


    public Task DisposeAsync()
    {
        utils.CloseConnection();
        return Task.CompletedTask;
    }
    
    
    
    // ------- CheepRepository --------
    [Fact]
    public async Task TestGetCheepsAmount()
    {
        //Act
        var cheeps = await cheepRepository.GetCheeps(0);
        
        //Assert
        Assert.Equal(32,cheeps.Count);
        await utils.CloseConnection();
        
    }

    [Fact]
    public async Task TestWhenGetCheepsFromNegativePage()
    {
        //Act and assert
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => cheepRepository.GetCheeps(-1));
        await utils.CloseConnection();
    }
    
    [Fact]
    public async Task TestGetCheepsPage1()
    {
        //Act
        var cheeps = await cheepRepository.GetCheeps(0);
        var cheep = cheeps[0];
        
        //Assert
        Assert.Equal("Starbuck now is what we hear the worst.", cheep.Text);
        await utils.CloseConnection();
    }
    
    [Fact]
    public async Task TestGetCheepsPage2()
    {
        //Act
        var cheeps = await cheepRepository.GetCheeps(2);
        var cheep = cheeps[0];
        
        //Assert
        Assert.Equal("In the morning of the wind, some few splintered planks, of what present avail to him.", cheep.Text);  
        await utils.CloseConnection();
    }
    
    
    [Fact]
    public async Task TestGetCheepsLastPage() 
    {
        //Act
        var cheeps = await cheepRepository.GetCheeps(21);
        
        //Assert
        Assert.True(cheeps.Count == 17);
        await utils.CloseConnection();
    }
    
    [Fact]
    public async Task TestGetCheepsBeyondLimit() 
    {   
        //Act
        var cheeps = await cheepRepository.GetCheeps(32);
        
        //Assert
        Assert.True(cheeps.Count == 0);    
        await utils.CloseConnection();
    }
    
    
    [Fact]
    public async Task TestGetCheepsFromExistingAuthor() 
    {   
        //Act
        var cheeps = await cheepRepository.GetCheepsFromAuthor(0, "Jacqualine Gilcoine");
        var cheep = cheeps[0];
        
        //Assert
        Assert.True(cheep.Author.Name == "Jacqualine Gilcoine"  && cheeps.Count == 32); //we know Jacqualine is the first author on the public timeline.
        await utils.CloseConnection();
    }
    
    [Fact]
    public async Task TestGetCheepsFromNonExistingAuthor() 
    {   
        //Act
        var cheeps = await cheepRepository.GetCheepsFromAuthor(0, "Eksisterer Ikke");
        
        //Assert
        Assert.True(cheeps.Count == 0);
        await utils.CloseConnection();
    }
    
    [Fact]
    public async Task TestGetCheepsFromAuthorPage2()
    {
        //Act
        var cheeps = await cheepRepository.GetCheepsFromAuthor(2, "Jacqualine Gilcoine");
        var cheep = cheeps[0];
        
        //Assert
        Assert.Equal("What a relief it was the place examined.", cheep.Text);
        await utils.CloseConnection();
    }    
    
    [Fact]
    public async Task TestGetCheepsFromAuthorLastPage()
    {
        //Act
        var cheeps = await cheepRepository.GetCheepsFromAuthor(12, "Jacqualine Gilcoine");
        var cheep = cheeps[0];
        
        //Assert
        Assert.True(cheeps.Count == 7 && cheep.Author.Name == "Jacqualine Gilcoine"); 
        await utils.CloseConnection();
    }
    
    [Fact]
    public async Task TestGetCheepsFromAuthorBeyondLimit()
    {
        //Act
        var cheeps = await cheepRepository.GetCheepsFromAuthor(20, "Jacqualine Gilcoine");
        
        //Assert
        Assert.True(cheeps.Count == 0);   
        await utils.CloseConnection();
    }

    [Fact]
    public async Task TestGetAllCheepsFromAuthor()
    {
        var result = ( await cheepRepository.GetAllCheepsFromAuthor("Adrian") ).Count;
        
        Assert.Equal(1, result);
    }


    [Fact]
    public async Task TestCheepConstraintOnDataModel()
    {
        var cheepsBefore = (await cheepRepository.GetAllCheepsFromAuthor("Mellie Yost")).Count;
        Author author = await authorRepository.GetAuthorByName("Mellie Yost");
        var invalidCheep = new String('a', 161);
        await cheepRepository.AddCheep(invalidCheep, author);
        
        var cheepsAfter = (await cheepRepository.GetAllCheepsFromAuthor("Mellie Yost")).Count;

        Assert.Equal(cheepsBefore, cheepsAfter);
    }
    
    // ------- Authorrepository --------
    
    [Fact]
    public async Task TestCreateAuthor()
    {
        //Act
        await authorRepository.CreateAuthor("Filifjonken", "fili@mail.com");
        Author author = await authorRepository.GetAuthorByName("Filifjonken");
        
        //Assert
        Assert.True(author.Name == "Filifjonken");   
        await utils.CloseConnection();
        
    }
    
    public async Task TestCreateAlreadyExistingAuthor()
    {
        //Act
        await authorRepository.CreateAuthor("Filifjonken", "fili@mail.com");
        Author author = await authorRepository.GetAuthorByName("Filifjonken");
        
        //Assert
        Assert.True(author.Name == "Filifjonken");   
        await utils.CloseConnection();
        
    }
    
    //Followrepository
    
    [Fact]
    public async Task CanAddFolowerToDb() 
    {
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

        await followRepository.AddFollowing(author1.Name, author2.Name);

        var follow = await context.Follows.FirstOrDefaultAsync();
        
        Assert.NotNull(follow);
        Assert.Equal(author1.Name, follow.Follower);

    }

    [Fact]
    public async Task CanRemoveFollowerFromDb()
    {
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

        await followRepository.AddFollowing(author1.Name, author2.Name);

        var follow = await context.Follows.FirstOrDefaultAsync();
        
        //Check that the Follow has been added
        Assert.NotNull(follow);

        await followRepository.RemoveFollowing(author1.Name, author2.Name);
        
        //Check that the follow has been removed
        var followRemoved = await context.Follows.FirstOrDefaultAsync();
        Assert.Null(followRemoved);
        
    }


    [Fact]
    public async Task CanGetFollowFromDb()
    {
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

        await followRepository.AddFollowing(author1.Name, author2.Name);
        
        var follows = await followRepository.GetFollowed(author1.Name);
        
        Assert.NotNull(follows);
        Assert.Equal(author2.Name, follows[0].Followed);
    }
    
    
    [Fact]
    public async Task CanGetFollowsFromDb()
    {
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

        await followRepository.AddFollowing(author1.Name, author2.Name);
        await followRepository.AddFollowing(author1.Name, author3.Name);
        
        var follows = await followRepository.GetFollowed(author1.Name);
        
        Assert.NotNull(follows);
        Assert.Equal(author2.Name, follows[0].Followed);
        Assert.Equal(author3.Name, follows[1].Followed);
    }

    [Fact]
    public async Task CanAddFolowerToDbWithchirpService() 
    {
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

        await chirpService.AddFollowing(author1.Name, author2.Name);

        var follow = await context.Follows.FirstOrDefaultAsync();
        
        Assert.NotNull(follow);
        Assert.Equal(author1.Name, follow.Follower);

    }
    
    // ------ Chirpservice -------
    [Fact]
    public async Task TestCanGetAllCheeps()
    {
        var cheeps = await chirpService.GetAllCheepsFromAuthor("Octavio Wagganer");
        
        Assert.Equal(15, cheeps.Count);
    }


    
    
    
    
    
}