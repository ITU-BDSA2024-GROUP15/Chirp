using Chirp.Core;
using Chirp.Infrastructure.Chirp.Repositories;
using Xunit;

namespace Chirp.Web.Tests;


public class IntegrationTests
{   
    
    [Fact]
    public async Task TestAddCheep()
    {
        var context = await TestUtilities.CreateInMemoryDb();
        IAuthorRepository authorrepo = new AuthorRepository(context); 
        ICheepRepository cheeprepo = new CheepRepository(context); 
        var cheepsBefore = await cheeprepo.GetCheepsFromAuthor(0, "Mellie Yost");
        
        Author author = await authorrepo.GetAuthorByName("Mellie Yost");
        await cheeprepo.AddCheep("hejj", author);
        
        var cheepsAfter = await cheeprepo.GetCheepsFromAuthor(0, "Mellie Yost");
        
        Assert.True(cheepsBefore.Count != cheepsAfter.Count);
        
        TestUtilities.CloseConnection();
    }
    
}
