using System.Runtime.InteropServices.JavaScript;
using Chirp.Core;
using Chirp.Infrastructure.Chirp.Repositories;
using Chirp.Infrastructure.Chirp.Services;
using Chirp.Web.Pages;
using Microsoft.AspNetCore.Identity.UI.V5.Pages.Account.Manage.Internal;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Sdk;

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
        //Act
        var cheeps = await cheepRepository.GetCheeps(-1);
        var cheep = cheeps[0];
        
        //Assert
        Assert.Equal("Starbuck now is what we hear the worst.", cheep.Text); //returns to page 1
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
    public async Task TestGetCheepsFromNullAuthor() 
    {   
       
        //Assert and act
        await Assert.ThrowsAsync<NullReferenceException>(() => cheepRepository.GetCheepsFromAuthor(0, null));
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
    public async Task TestGetCheepsFromAuthorNegativePage()
    {
        //Act
        var cheeps = await cheepRepository.GetCheepsFromAuthor(-1, "Jacqualine Gilcoine");
        var cheep = cheeps[0];
        
        //Assert
        Assert.Equal("Starbuck now is what we hear the worst.", cheep.Text);
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
        //Act
        var result = ( await cheepRepository.GetAllCheepsFromAuthor("Adrian") ).Count;
        
        //Assert
        Assert.Equal(1, result);
        await utils.CloseConnection();
    }
    
    [Fact]
    public async Task TestGetAllCheepsFromNonexistingAuthor()
    {
        //Act
        var result = ( await cheepRepository.GetAllCheepsFromAuthor("migg") ).Count;
        
        //Assert
        Assert.Equal(0, result);
        await utils.CloseConnection();
    }
    
    [Fact]
    public async Task TestGetAllCheepsFromNullAuthor()
    {
        //Assert and act
        await Assert.ThrowsAsync<NullReferenceException>(() => cheepRepository.GetAllCheepsFromAuthor(null));
        await utils.CloseConnection();
    }
    
    
    [Fact]
    public async Task TestAddCheepCorrectInput()
    {
        //Arrange
        var cheepsBefore = context.Cheeps.Count();
        Author author = new Author()
        {
            Id = context.Authors.Count() + 1,
            Name = "Test1",
            Email = "test1@mail.com",
        };
        
        //Act
        await cheepRepository.AddCheep("Hejsa", author);
        var cheepsAfter = context.Cheeps.Count();
        
        //Assert
        Assert.True(cheepsAfter == cheepsBefore+1);
        await utils.CloseConnection();
    }


    [Fact]
    public async Task TestAddCheepNullAuthor()
    {
        //Assert and act
        await Assert.ThrowsAsync<NullReferenceException>(() => cheepRepository.AddCheep("Hejsa", null));
        await utils.CloseConnection();
    }
    
    [Fact]
    public async Task TestAddCheepLengthConstraintl()
    {
        //Arrange
        Author author = new Author()
        {
            Id = 200,
            Name = "Test1",
            Email = "test1@mail.com",
        };
        var invalidCheep = new String('a', 161);
        
        
        //Assert and act
        await Assert.ThrowsAsync<ArgumentException>(() => cheepRepository.AddCheep(invalidCheep, author));
        await utils.CloseConnection();
    }
    
    [Fact]
    public async Task TestAddLike()
    {   
        
        var likesBefore = context.Cheeps.ToList()[1].Likes.Count();
        await cheepRepository.AddLike("Mellie Yost", 2);

        context.SaveChanges();
        
        var likesAfter = context.Cheeps.ToList()[1].Likes.Count();
        
        Assert.True(likesAfter == likesBefore +1);
        await utils.CloseConnection();
    }
    
     
    [Fact]
    public async Task TestAddLikeNullAuthor()
    {
        //Assert and act
        await Assert.ThrowsAsync<NullReferenceException>(() => cheepRepository.AddLike(null, 2));
        await utils.CloseConnection();
    }
    
      
    [Fact]
    public async Task TestAddLikeNonexistingCheepID()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(() => cheepRepository.AddLike("hej", 800000));
        await utils.CloseConnection();
    } 
    
    //TODO continue looking at unit tests from here
    

    [Fact]

    public async Task TestRemoveLike()
    {
        context.Cheeps.ToList()[1].Likes.Add("Mellie Yost");
        var likesBefore = context.Cheeps.ToList()[1].Likes.Count();
        
        await cheepRepository.RemoveLike("Mellie Yost", 2);
       
       
        var likesAfter = context.Cheeps.ToList()[1].Likes.Count();
       
        Assert.True(likesAfter == likesBefore -1);
        await utils.CloseConnection();
        
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
    
    [Fact]
    public async Task TestUsernameCannotContainSlash()
    {
        //Assert and act
        await Assert.ThrowsAsync<ArgumentException>(() =>
            authorRepository.CreateAuthor("/Haha", "hahaemail@gmail.com"));
    }
    
    //Followrepository
    
    [Fact]
    public async Task CanAddFollowerToDb() 
    {
        //Arrange
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
        
        //Act
        await followRepository.AddFollowing(author1.Name, author2.Name);
        var follow = await context.Follows.FirstOrDefaultAsync();
        
        //Assert
        Assert.NotNull(follow);
        Assert.Equal(author1.Name, follow.Follower);

    }

    [Fact]
    public async Task CanRemoveFollowerFromDb()
    {
        //Arrange
        var author1 = "hej";
        var author2 = "meddig";
        var follow = new Follow()
        {
            Follower = author1,
            Followed = author2
        };
        
        await context.Follows.AddAsync(follow);
        await context.SaveChangesAsync();
        var followbefore = context.Follows.ToList().Count;
        
        //Act
        await followRepository.RemoveFollowing(author1, author2);
        var followafter = context.Follows.ToList().Count;
        
        //Assert
        Assert.True(followafter == followbefore -1);
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
    public async Task CanAddFollowerToDbWithchirpService() 
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