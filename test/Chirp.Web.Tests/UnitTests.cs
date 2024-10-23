using Chirp.Core;
using Chirp.Infrastructure.Chirp.Repositories;
using Xunit;

namespace Chirp.Web.Tests;

public class UnitTests
{
    
    //CheepRepository
    [Fact]
    public async Task TestGetCheepsAmount()
    {   
        var context = await TestUtilities.createInMemoryDB();
        ICheepRepository repository = new CheepRepository(context);        
        var cheeps = await repository.GetCheeps(0);
        Assert.Equal(32,cheeps.Count);
        TestUtilities.closeConnection();
        
    }
    
    [Fact]
    public async Task TestGetCheepsPage1()
    {
        var context = await TestUtilities.createInMemoryDB();
        ICheepRepository repository = new CheepRepository(context);     
        var cheeps = await repository.GetCheeps(0);
        var cheep = cheeps[0];
        Assert.Equal("Starbuck now is what we hear the worst.", cheep.Text);
        TestUtilities.closeConnection();
    }
    
    [Fact]
    public async Task TestGetCheepsPage2()
    {
        
        var context = await TestUtilities.createInMemoryDB();
        ICheepRepository repository = new CheepRepository(context);     
        var cheeps = await repository.GetCheeps(2);
        var cheep = cheeps[0];
        Assert.Equal("In the morning of the wind, some few splintered planks, of what present avail to him.", cheep.Text);  
        TestUtilities.closeConnection();
    }
    
    
    [Fact]
    public async Task TestGetCheepsLastPage() 
    {
        var context = await TestUtilities.createInMemoryDB();
        ICheepRepository repository = new CheepRepository(context);     
        var cheeps = await repository.GetCheeps(21);
        
        Assert.True(cheeps.Count == 17);
        TestUtilities.closeConnection();
    }
    
    [Fact]
    public async Task TestGetCheepsBeyondLimit() 
    {   
        var context = await TestUtilities.createInMemoryDB();
        ICheepRepository repository = new CheepRepository(context);     
        var cheeps = await repository.GetCheeps(32);
        
        Assert.True(cheeps.Count == 0);    
        TestUtilities.closeConnection();
    }
    
    
    [Fact]
    public async Task TestGetCheepsFromAuthor() 
    {   
        var context = await TestUtilities.createInMemoryDB();
        ICheepRepository repository = new CheepRepository(context);     
        var cheeps = await repository.GetCheepsFromAuthor(0, "Jacqualine Gilcoine");
        var cheep = cheeps[0];
        //we know Jacqualine is the first author on the public timeline.
        Assert.True(cheep.Author.Name == "Jacqualine Gilcoine"  && cheeps.Count == 32);
        TestUtilities.closeConnection();
    }
    
    [Fact]
    public async Task TestGetCheepsFromAuthorPage2()
    {
        var context = await TestUtilities.createInMemoryDB();
        ICheepRepository repository = new CheepRepository(context);     
        var cheeps = await repository.GetCheepsFromAuthor(2, "Jacqualine Gilcoine");
        var cheep = cheeps[0];
        
        Assert.Equal("What a relief it was the place examined.", cheep.Text);
        TestUtilities.closeConnection();
    }    
    
    [Fact]
    public async Task TestGetCheepsFromAuthorLastPage()
    {
        var context = await TestUtilities.createInMemoryDB();
        ICheepRepository repository = new CheepRepository(context);     
        var cheeps = await repository.GetCheepsFromAuthor(12, "Jacqualine Gilcoine");
        var cheep = cheeps[0];
        
        Assert.True(cheeps.Count == 7 && cheep.Author.Name == "Jacqualine Gilcoine"); 
        TestUtilities.closeConnection();
    }
    
    [Fact]
    public async Task TestGetCheepsFromAuthorBeyondLimit()
    {
        var context = await TestUtilities.createInMemoryDB();
        ICheepRepository repository = new CheepRepository(context);     
        var cheeps = await repository.GetCheepsFromAuthor(20, "Jacqualine Gilcoine");
        
        Assert.True(cheeps.Count == 0);   
        TestUtilities.closeConnection();
    }


    [Fact]
    public async Task TestGetAuthorFail()
    {
        var context = await TestUtilities.createInMemoryDB();
        IAuthorRepository repository = new AuthorRepository(context);
        
        await Assert.ThrowsAsync<Exception>(() =>  repository.GetAuthorByName("Filifjonken"));
    }


    [Fact]
    public async Task TestCreateAuthor()
    {
        var context = await TestUtilities.createInMemoryDB();
        IAuthorRepository repository = new AuthorRepository(context);

        await repository.CreateAuthor("Filifjonken", "fili@mail.com");
        
        Author author = await repository.GetAuthorByName("Filifjonken");
        
        Assert.True(author.Name == "Filifjonken");   
        
    }
    
}