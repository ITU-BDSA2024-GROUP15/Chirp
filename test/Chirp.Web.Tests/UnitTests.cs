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

    private TestUtilities _utils;
    private CheepDbContext _context;
    private ICheepRepository _cheepRepository;
    private IAuthorRepository _authorRepository;
    private IFollowRepository _followRepository;
    private IChirpService _chirpService;

    public async Task InitializeAsync()
    {
        _utils = new TestUtilities();
        _context = await _utils.CreateInMemoryDb();
        _cheepRepository = new CheepRepository(_context);
        _authorRepository = new AuthorRepository(_context);
        _followRepository = new FollowRepository(_context);
        _chirpService = new ChirpService(_cheepRepository, _authorRepository, _followRepository);
        
    }


    public Task DisposeAsync()
    {
        _utils.CloseConnection();
        return Task.CompletedTask;
    }
    
    
    //CheepRepository
    [Fact]
    public async Task TestGetCheepsAmount()
    {
        var cheeps = await _cheepRepository.GetCheeps(0);
        Assert.Equal(32,cheeps.Count);
        await _utils.CloseConnection();
        
    }
    
    [Fact]
    public async Task TestGetCheepsPage1()
    {
        var cheeps = await _cheepRepository.GetCheeps(0);
        var cheep = cheeps[0];
        Assert.Equal("Starbuck now is what we hear the worst.", cheep.Text);
        await _utils.CloseConnection();
    }
    
    [Fact]
    public async Task TestGetCheepsPage2()
    {
        var cheeps = await _cheepRepository.GetCheeps(2);
        var cheep = cheeps[0];
        Assert.Equal("In the morning of the wind, some few splintered planks, of what present avail to him.", cheep.Text);  
        await _utils.CloseConnection();
    }
    
    
    [Fact]
    public async Task TestGetCheepsLastPage() 
    {
        var cheeps = await _cheepRepository.GetCheeps(21);
        
        Assert.True(cheeps.Count == 17);
        await _utils.CloseConnection();
    }
    
    [Fact]
    public async Task TestGetCheepsBeyondLimit() 
    {   
        var cheeps = await _cheepRepository.GetCheeps(32);
        
        Assert.True(cheeps.Count == 0);    
        await _utils.CloseConnection();
    }
    
    
    [Fact]
    public async Task TestGetCheepsFromAuthor() 
    {   
        var cheeps = await _cheepRepository.GetCheepsFromAuthor(0, "Jacqualine Gilcoine");
        var cheep = cheeps[0];
        //we know Jacqualine is the first author on the public timeline.
        Assert.True(cheep.Author.Name == "Jacqualine Gilcoine"  && cheeps.Count == 32);
        await _utils.CloseConnection();
    }
    
    [Fact]
    public async Task TestGetCheepsFromAuthorPage2()
    {
        var cheeps = await _cheepRepository.GetCheepsFromAuthor(2, "Jacqualine Gilcoine");
        var cheep = cheeps[0];
        
        Assert.Equal("What a relief it was the place examined.", cheep.Text);
        await _utils.CloseConnection();
    }    
    
    [Fact]
    public async Task TestGetCheepsFromAuthorLastPage()
    {
        var cheeps = await _cheepRepository.GetCheepsFromAuthor(12, "Jacqualine Gilcoine");
        var cheep = cheeps[0];
        
        Assert.True(cheeps.Count == 7 && cheep.Author.Name == "Jacqualine Gilcoine"); 
        await _utils.CloseConnection();
    }
    
    [Fact]
    public async Task TestGetCheepsFromAuthorBeyondLimit()
    {
        var cheeps = await _cheepRepository.GetCheepsFromAuthor(20, "Jacqualine Gilcoine");
        
        Assert.True(cheeps.Count == 0);   
        await _utils.CloseConnection();
    }
    

    [Fact]
    public async Task TestCreateAuthor()
    {

        await _authorRepository.CreateAuthor("Filifjonken", "fili@mail.com");
        
        var author = await _authorRepository.GetAuthorByName("Filifjonken");

        if (author != null)
        {
            Assert.True(author.Name == "Filifjonken");   
            await _utils.CloseConnection();
        }
    }


    [Fact]
    public async Task TestGetAllCheepsFromAuthor()
    {
        var result = ( await _cheepRepository.GetAllCheepsFromAuthor("Adrian") ).Count;
        
        Assert.Equal(1, result);
    }


    [Fact]
    public async Task TestCheepConstraintOnDataModel()
    {
        var cheepsBefore = (await _cheepRepository.GetAllCheepsFromAuthor("Mellie Yost")).Count;
        var author = await _authorRepository.GetAuthorByName("Mellie Yost");

        if (author != null)
        {
            var invalidCheep = new String('a', 161);
            await _cheepRepository.AddCheep(invalidCheep, author);
        
            var cheepsAfter = (await _cheepRepository.GetAllCheepsFromAuthor("Mellie Yost")).Count;

            Assert.Equal(cheepsBefore, cheepsAfter);
        } 
    }

    
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

        await _followRepository.AddFollowing(author1.Name, author2.Name);

        var follow = await _context.Follows.FirstOrDefaultAsync();
        
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

        await _followRepository.AddFollowing(author1.Name, author2.Name);

        var follow = await _context.Follows.FirstOrDefaultAsync();
        
        //Check that the Follow has been added
        Assert.NotNull(follow);

        await _followRepository.RemoveFollowing(author1.Name, author2.Name);
        
        //Check that the follow has been removed
        var followRemoved = await _context.Follows.FirstOrDefaultAsync();
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

        await _followRepository.AddFollowing(author1.Name, author2.Name);
        
        var follows = await _followRepository.GetFollowed(author1.Name);
        
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

        await _followRepository.AddFollowing(author1.Name, author2.Name);
        await _followRepository.AddFollowing(author1.Name, author3.Name);
        
        var follows = await _followRepository.GetFollowed(author1.Name);
        
        Assert.NotNull(follows);
        Assert.Equal(author2.Name, follows[0].Followed);
        Assert.Equal(author3.Name, follows[1].Followed);
    }

    [Fact]
    public async Task CanAddFolowerToDbWith_chirpService() 
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

        await _chirpService.AddFollowing(author1.Name, author2.Name);

        var follow = await _context.Follows.FirstOrDefaultAsync();
        
        Assert.NotNull(follow);
        Assert.Equal(author1.Name, follow.Follower);

    }
    
    [Fact]
    public async Task TestCanGetAllCheeps()
    {
        var cheeps = await _chirpService.GetAllCheepsFromAuthor("Octavio Wagganer");
        
        Assert.Equal(15, cheeps.Count);
    }

    [Fact]
    public async Task TestUsernameCannotContainSlash()
    {
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _authorRepository.CreateAuthor("/Haha", "hahaemail@gmail.com"));
    }
    
    [Fact]
    public async Task TestAddCheep()
    {
        var cheepsBefore = _context.Cheeps.Count();
        
        Author author = new Author()
        {
            Id = _context.Authors.Count() + 1,
            Name = "Test1",
            Email = "test1@mail.com",
        };
        
        await _cheepRepository.AddCheep("Hejsa", author);
        
        var cheepsAfter = _context.Cheeps.Count();
        
        Assert.True(cheepsAfter == cheepsBefore+1);
    }


    [Fact]
    public async Task TestAddLike()
    {
        var likesBefore = _context.Cheeps.ToList()[1].Likes.Count();
        await _cheepRepository.AddLike("Mellie Yost", 2);

        _context.SaveChanges();
        
        var likesAfter = _context.Cheeps.ToList()[1].Likes.Count();
        
        Assert.True(likesAfter == likesBefore +1);
    }


    [Fact]

    public async Task TestRemoveLike()
    {
        _context.Cheeps.ToList()[1].Likes.Add("Mellie Yost");
        var likesBefore = _context.Cheeps.ToList()[1].Likes.Count();
        
       await _cheepRepository.RemoveLike("Mellie Yost", 2);
       
       
       var likesAfter = _context.Cheeps.ToList()[1].Likes.Count();
       
       Assert.True(likesAfter == likesBefore -1);
        
    }

    
    
    
    
    
}