using Chirp.Core;
using Chirp.Infrastructure.Chirp.Repositories;
using Chirp.Infrastructure.Chirp.Services;
using Chirp.Web.Pages;
using Microsoft.AspNetCore.Identity.UI.V5.Pages.Account.Manage.Internal;
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
    public async Task TestCheepConstraintOnEndpoint()
    {
        var utils = new TestUtilities();
        var context = await utils.CreateInMemoryDb();
        IAuthorRepository authorrepository = new AuthorRepository(context);
        ICheepRepository cheeprepository = new CheepRepository(context);
        var cheepservice = new CheepService(cheeprepository, authorrepository);
        string message = new String('c', 161);
        var pagemodel = new PublicModel(cheepservice)
        {
            CheepMessage = message
        };

        var result = await pagemodel.OnPost();
        var cheeps = await cheeprepository.GetCheeps(1);
        var cheep = cheeps[0];
        
        Assert.NotEqual(message, cheep.Text);
        
    }  
    
}