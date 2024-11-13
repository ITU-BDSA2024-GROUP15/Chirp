using System.Net;
using Chirp.Core;
using Chirp.Web;
using Chirp.Infrastructure.Chirp.Repositories;
using Chirp.Infrastructure.Chirp.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace Chirp.Web.Tests;


public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private WebApplicationFactory<Program> _factory;
    
    public IntegrationTests(WebApplicationFactory<Program> factory){
      
        _factory = factory;
    }
    
    /* TODO: What the fuck is this test?
    [Fact]
    public async Task test()
    {
        var client = _factory.CreateClient();
        
        //act 
        HttpResponseMessage response = await client.GetAsync("/Identity/Account/Manage");
        string content = await response.Content.ReadAsStringAsync();
        
        //Assert
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
    }
    */
    
    
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

        ICheepService service = new CheepService(cheeprepo, authorrepo);
        
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

        authorrepo.CreateAuthor("/Haha", "hahaemail@gmail.com");

        Author author = await authorrepo.GetAuthorByName("/Haha");
        //Assert.False(author.GetType() != Author.GetType());
        
    }
    
    
}
