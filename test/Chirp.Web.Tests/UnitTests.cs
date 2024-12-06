using Chirp.Core;
using Chirp.Infrastructure.Chirp.Repositories;
using Chirp.Infrastructure.Chirp.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Chirp.Web.Tests;

public class UnitTests : IAsyncLifetime
{

    private TestUtilities? _utils;
    private CheepDbContext? _context;
    private ICheepRepository? _cheepRepository;
    private IAuthorRepository? _authorRepository;
    private IFollowRepository? _followRepository;
    private IChirpService? _chirpService;

    
    
    public async Task InitializeAsync()
    {
        //Arrange
        _utils = new TestUtilities();
        _context = await _utils.CreateInMemoryDb();
        _cheepRepository = new CheepRepository(_context);
        _authorRepository = new AuthorRepository(_context);
        _followRepository = new FollowRepository(_context);
        _chirpService = new ChirpService(_cheepRepository, _authorRepository, _followRepository);
        
    }


    public Task DisposeAsync()
    {
        _utils?.CloseConnection();
        return Task.CompletedTask;
    }
    
    
    
    // ------- CheepRepository --------
    [Fact]
    public async Task TestGetCheepsAmount()
    {
        if (_cheepRepository == null || _utils == null)
        {
            return;
        }
        
        //Act
        var cheeps = await _cheepRepository.GetCheeps(0);
        
        //Assert
        Assert.Equal(32,cheeps.Count);
        await _utils.CloseConnection();
        
    }

    [Fact]
    public async Task TestWhenGetCheepsFromNegativePage()
    {
        if (_cheepRepository == null || _utils == null)
        {
            return;
        }
        //Act
        var cheeps = await _cheepRepository.GetCheeps(-1);
        var cheep = cheeps[0];
        
        //Assert
        Assert.Equal("Starbuck now is what we hear the worst.", cheep.Text); //returns to page 1
        await _utils.CloseConnection();
    }
    
    [Fact]
    public async Task TestGetCheepsPage1()
    {
        if (_cheepRepository == null || _utils == null)
        {
            return;
        }
        //Act
        var cheeps = await _cheepRepository.GetCheeps(0);
        var cheep = cheeps[0];
        
        //Assert
        Assert.Equal("Starbuck now is what we hear the worst.", cheep.Text);
        await _utils.CloseConnection();
    }
    
    [Fact]
    public async Task TestGetCheepsPage2()
    {
        if (_cheepRepository == null || _utils == null)
        {
            return;
        }
        //Act
        var cheeps = await _cheepRepository.GetCheeps(2);
        var cheep = cheeps[0];
        
        //Assert
        Assert.Equal("In the morning of the wind, some few splintered planks, of what present avail to him.", cheep.Text);  
        await _utils.CloseConnection();
    }
    
    [Fact]
    public async Task TestGetCheepsLastPage() 
    {
        if (_cheepRepository == null || _utils == null)
        {
            return;
        }
        //Act
        var cheeps = await _cheepRepository.GetCheeps(21);
        
        //Assert
        Assert.True(cheeps.Count == 17);
        await _utils.CloseConnection();
    }
    
    [Fact]
    public async Task TestGetCheepsBeyondLimit() 
    {   if (_cheepRepository == null || _utils == null)
        {
            return;
        }
        //Act
        var cheeps = await _cheepRepository.GetCheeps(32);
        
        //Assert
        Assert.True(cheeps.Count == 0);    
        await _utils.CloseConnection();
    }
    
    [Fact]
    public async Task TestGetCheepsFromExistingAuthor() 
    {   
        if (_cheepRepository == null || _utils == null)
        {
            return;
        }
        //Act
        var cheeps = await _cheepRepository.GetCheepsFromAuthor(0, "Jacqualine Gilcoine");
        var cheep = cheeps[0];
        
        //Assert
        Assert.True(cheep.Author.Name == "Jacqualine Gilcoine"  && cheeps.Count == 32); //we know Jacqualine is the first author on the public timeline.
        await _utils.CloseConnection();
    }
    
    [Fact]
    public async Task TestGetCheepsFromNonExistingAuthor() 
    {   if (_cheepRepository == null || _utils == null)
        {
            return;
        }
        //Act
        var cheeps = await _cheepRepository.GetCheepsFromAuthor(0, "Eksisterer Ikke");
        
        //Assert
        Assert.True(cheeps.Count == 0);
        await _utils.CloseConnection();
    }
    
    [Fact]
    public async Task TestGetCheepsFromAuthorNegativePage()
    {
        if (_cheepRepository == null || _utils == null)
        {
            return;
        }
        //Act
        var cheeps = await _cheepRepository.GetCheepsFromAuthor(-1, "Jacqualine Gilcoine");
        var cheep = cheeps[0];
        
        //Assert
        Assert.Equal("Starbuck now is what we hear the worst.", cheep.Text);
        await _utils.CloseConnection();
    }    
    
    [Fact]
    public async Task TestGetCheepsFromAuthorPage2()
    {
        if (_cheepRepository == null || _utils == null)
        {
            return;
        }
        //Act
        var cheeps = await _cheepRepository.GetCheepsFromAuthor(2, "Jacqualine Gilcoine");
        var cheep = cheeps[0];
        
        //Assert
        Assert.Equal("What a relief it was the place examined.", cheep.Text);
        await _utils.CloseConnection();
    }    
    
    [Fact]
    public async Task TestGetCheepsFromAuthorLastPage()
    {
        if (_cheepRepository == null || _utils == null)
        {
            return;
        }
        //Act
        var cheeps = await _cheepRepository.GetCheepsFromAuthor(12, "Jacqualine Gilcoine");
        var cheep = cheeps[0];
        
        //Assert
        Assert.True(cheeps.Count == 7 && cheep.Author.Name == "Jacqualine Gilcoine"); 
        await _utils.CloseConnection();
    }
    
    [Fact]
    public async Task TestGetCheepsFromAuthorBeyondLimit()
    {   
        if (_cheepRepository == null || _utils == null)
        {
            return;
        }
        //Act
        var cheeps = await _cheepRepository.GetCheepsFromAuthor(20, "Jacqualine Gilcoine");
        
        //Assert
        Assert.True(cheeps.Count == 0);   
        await _utils.CloseConnection();
        await _utils.CloseConnection();
    }

    [Fact]
    public async Task TestGetAllCheepsFromAuthor()
    {
        if (_cheepRepository == null || _utils == null)
        {
            return;
        }
        //Act
        var result = ( await _cheepRepository.GetAllCheepsFromAuthor("Adrian") ).Count;
        
        //Assert
        Assert.Equal(1, result);
        await _utils.CloseConnection();
    }
    
    [Fact]
    public async Task TestGetAllCheepsFromNonexistingAuthor()
    {
        if (_cheepRepository == null || _utils == null)
        {
            return;
        }
        //Act
        var result = ( await _cheepRepository.GetAllCheepsFromAuthor("migg") ).Count;
        
        //Assert
        Assert.Equal(0, result);
        await _utils.CloseConnection();
    }
    
    [Fact]
    public async Task TestAddCheepCorrectInput()
    {
        if (_cheepRepository == null || _utils == null || _context == null)
        {
            return;
        }
        //Arrange
        var cheepsBefore = _context.Cheeps.Count();
        Author author = new Author()
        {
            Id = _context.Authors.Count() + 1,
            Name = "Test1",
            Email = "test1@mail.com",
        };
        
        //Act
        await _cheepRepository.AddCheep("Hejsa", author);
        var cheepsAfter = _context.Cheeps.Count();
        
        //Assert
        Assert.True(cheepsAfter == cheepsBefore+1);
        await _utils.CloseConnection();
    }
    
    [Fact]
    public async Task TestAddCheepLengthConstraint()
    {
        if (_cheepRepository == null || _utils == null)
        {
            return;
        }
        //Arrange
        Author author = new Author()
        {
            Id = 200,
            Name = "Test1",
            Email = "test1@mail.com",
        };
        var invalidCheep = new String('a', 161);
        
        
        //Assert and act
        await Assert.ThrowsAsync<ArgumentException>(() => _cheepRepository.AddCheep(invalidCheep, author));
        await _utils.CloseConnection();
    }
    
    [Fact]
    public async Task TestAddLike()
    {   
        if (_cheepRepository == null || _utils == null || _context == null)
        {
            return;
        }
        var likesBefore = _context.Cheeps.ToList()[1].Likes.Count();
        await _cheepRepository.AddLike("Mellie Yost", 2);

        await _context.SaveChangesAsync();
        
        var likesAfter = _context.Cheeps.ToList()[1].Likes.Count();
        
        Assert.True(likesAfter == likesBefore +1);
        await _utils.CloseConnection();
    }
    
    [Fact]
    public async Task TestAddLikeInvalidCheepId()
    {
        if (_cheepRepository == null || _utils == null)
        {
            return;
        }
        await Assert.ThrowsAsync<InvalidOperationException>(() => _cheepRepository.AddLike("hej", 800000));
        await _utils.CloseConnection();
    } 
    
    [Fact]

    public async Task TestRemoveLike()
    {
        if (_cheepRepository == null || _utils == null || _context == null)
        {
            return;
        }
        //Arrange
        _context.Cheeps.ToList()[1].Likes.Add("Mellie Yost");
        var likesBefore = _context.Cheeps.ToList()[1].Likes.Count();
        
        //Act
        await _cheepRepository.RemoveLike("Mellie Yost", 2);
        var likesAfter = _context.Cheeps.ToList()[1].Likes.Count();
       
        //Assert
        Assert.True(likesAfter == likesBefore -1);
        await _utils.CloseConnection();
        
    }
    
    [Fact]
    public async Task TestRemoveLikeInvalidAuthor()
    {
        if (_cheepRepository == null || _utils == null || _context == null)
        {
            return;
        }
        //Arrange
       var cheep = _context.Cheeps.ToList()[1];
       cheep.Likes.Add("Mellie Yost");
       var before = cheep.Likes.Count();
       //Act
       await _cheepRepository.RemoveLike("TestAuthor", cheep.CheepId);
       var after = cheep.Likes.Count();
       
       //Assert
       Assert.Equal(before, after);
        await _utils.CloseConnection();
    }
    
    [Fact]
    public async Task TestRemoveLikeInvalidCheepId()
    {
        if (_cheepRepository == null || _utils == null)
        {
            return;
        }
        //Assert and act
        await Assert.ThrowsAsync<InvalidOperationException>(() => _cheepRepository.RemoveLike("Mellie Yost", 80000));
        await _utils.CloseConnection();
    }
    
    [Fact]
    public async Task TestCountLikes()
    {
        if (_cheepRepository == null || _utils == null || _context == null)
        {
            return;
        }
        //Arrange
        var before = await _cheepRepository.CountLikes(5);
        var currentLikes = await _context.Cheeps.FirstAsync(cheep =>cheep.CheepId == 5);
        currentLikes.Likes.Add("Mellie Yost");
        await _context.SaveChangesAsync();
        
        //Act
        var after = await _cheepRepository.CountLikes(5);
        
        //Assert
        Assert.True(after == before + 1);
        await _utils.CloseConnection();
    }
    
    [Fact]
    public async Task TestCountLikesInvalidCheepId()
    {
        if (_cheepRepository == null || _utils == null)
        {
            return;
        }
        //Assert and act
        await Assert.ThrowsAsync<InvalidOperationException>(() => _cheepRepository.CountLikes(80000));
        await _utils.CloseConnection();
    }
    
    [Fact]
    public async Task TestGetAllLikedNoLikes()
    {
        if (_cheepRepository == null || _utils == null)
        {
            return;
        }
        //Act
        var cheeps = await _cheepRepository.GetAllLiked("Mellie Yost");
        
        //Assert
        Assert.Empty(cheeps);
        await _utils.CloseConnection();
    }
    
    [Fact]
    public async Task TestGetAllLikedReturnsCorrectCheep()
    {
        if (_cheepRepository == null || _utils == null || _context == null)
        {
            return;
        }
        //Arrange 
        var currentLikes = await _context.Cheeps.FirstAsync(cheep =>cheep.CheepId == 5);
        currentLikes.Likes.Add("Mellie Yost");
        await _context.SaveChangesAsync();
        
        //Act
        var cheeps = await _cheepRepository.GetAllLiked("Mellie Yost");
        var cheep = cheeps[0].CheepId;
        
        //Assert
        Assert.Equal(5, cheep);
        await _utils.CloseConnection();
    }
    
    
    [Fact]
    public async Task TestDeleteAllLikes()
    {
        if (_cheepRepository == null || _utils == null || _context == null)
        {
            return;
        }
        //Arrange 
        var currentLikes = await _context.Cheeps.FirstAsync(cheep =>cheep.CheepId == 5);
        currentLikes.Likes.Add("Mellie Yost");
        await _context.SaveChangesAsync();
        var cheep = await _context.Cheeps.FirstAsync(cheep =>cheep.CheepId == 5);
        var before = cheep.Likes.Count();
        
        //Act
        await _cheepRepository.DeleteAllLikes("Mellie Yost");
        var after = cheep.Likes.Count();
        
        //Assert
        Assert.NotEqual(before, after);
        await _utils.CloseConnection();
    }


    [Fact]
    public async Task TestGetTopLikedCheeps()
    {
        if (_context == null || _cheepRepository == null || _utils == null)
        {
            return;
        }
        
        //Arrange 
        var cheep1 = await _context.Cheeps.FirstAsync(cheep =>cheep.CheepId == 5);
        cheep1.Likes.Add("Mellie Yost");
        cheep1.Likes.Add("Adrian");
        var cheep2 = await _context.Cheeps.FirstAsync(cheep =>cheep.CheepId == 1);
        cheep2.Likes.Add("Mellie Yost");
        await _context.SaveChangesAsync();
        
        //Act
        var result = await _cheepRepository.GetTopLikedCheeps(1);
        var topcheep = result[0];
        var secondtopcheep = result[1];
        
        //Assert
        Assert.Equal(5, topcheep.CheepId);
        Assert.Equal(1, secondtopcheep.CheepId);
        await _utils.CloseConnection();
    }
    
    [Fact]
    public async Task TestGetTopLikedCheepsNegativePage()
    {
        if (_context == null || _cheepRepository == null || _utils == null)
        {
            return;
        }
        
        //Arrange 
        var cheep1 = await _context.Cheeps.FirstAsync(cheep =>cheep.CheepId == 5);
        cheep1.Likes.Add("Mellie Yost");
        cheep1.Likes.Add("Adrian");
        var cheep2 = await _context.Cheeps.FirstAsync(cheep =>cheep.CheepId == 1);
        cheep2.Likes.Add("Mellie Yost");
        await _context.SaveChangesAsync();
        
        //Act
        var result = await _cheepRepository.GetTopLikedCheeps(-10);
        var topcheep = result[0];
        var secondtopcheep = result[1];
        
        //Assert
        Assert.Equal(5, topcheep.CheepId);
        Assert.Equal(1, secondtopcheep.CheepId);
        await _utils.CloseConnection();
    }
    
    
    
   
    
    // ------- Author Repository --------
    
    [Fact]
    public async Task TestCreateAuthor()
    {
        if (_authorRepository == null || _utils == null)
        {
            return;
        }
        //Act
        await _authorRepository.CreateAuthor("Filifjonken", "fili@mail.com");
        var author = await _authorRepository.GetAuthorByName("Filifjonken");
        
        //Assert
        if (author != null)
        {
            Assert.True(author.Name == "Filifjonken");   
            await _utils.CloseConnection();
        }
        
    }
   
    
    [Fact]
    public async Task TestUsernameCannotContainSlash()
    {
        if (_authorRepository == null)
        {
            return;
        }
        //Act and assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _authorRepository.CreateAuthor("/Haha", "hahaemail@gmail.com"));
    }
    
    
    
    
    
    // ----  Followrepository ----
    
    [Fact]
    public async Task CanAddFollowerToDb() 
    {
        if (_followRepository == null || _context == null)
        {
            return;
        }
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
        await _followRepository.AddFollowing(author1.Name, author2.Name);
        var follow = await _context.Follows.FirstOrDefaultAsync();
        
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

        //await _followRepository.AddFollowing(author1.Name, author2.Name);

        //var follow = await _context.Follows.FirstOrDefaultAsync();
        
        //Check that the Follow has been added
        Assert.NotNull(follow);

        //await _followRepository.RemoveFollowing(author1.Name, author2.Name);
        
        //Check that the follow has been removed
        var followRemoved = await _context.Follows.FirstOrDefaultAsync();
        Assert.Null(followRemoved);
        
        //Assert
        //Assert.True(followafter == followbefore -1);
    }
    
    [Fact]
    public async Task CanGetFollowFromDb()
    {
        if (_followRepository == null)
        {
            return;
        }
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
        if (_followRepository == null)
        {
            return;
        }
        
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
    
    
    // ------ Chirpservice -------
    [Fact]
    public async Task TestCanGetAllCheeps()
    {
        if (_chirpService == null)
        {
            return;
        }
        var cheeps = await _chirpService.GetAllCheepsFromAuthor("Octavio Wagganer");
        
        Assert.Equal(15, cheeps.Count);
    }
    
    [Fact]
    public async Task CanAddFollowerToDbWithchirpService() 
    {
        if (_chirpService == null || _context == null)
        {
            return;
        }
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
}