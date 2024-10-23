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
        IAuthorRepository repository1 = new AuthorRepository(context); 
        ICheepRepository repository2 = new CheepRepository(context); 
        var cheepsBefore = await repository2.GetCheepsFromAuthor(0, "Mellie Yost");
        
        Author author = await repository1.GetAuthorByName("Mellie Yost");
        await repository2.AddCheep("hejj", author);
        
        var cheepsAfter = await repository2.GetCheepsFromAuthor(0, "Mellie Yost");
        
        Assert.True(cheepsBefore.Count != cheepsAfter.Count);
        
        TestUtilities.CloseConnection();
    }
    
}
