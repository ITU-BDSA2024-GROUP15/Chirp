using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace Chirp.Web.Tests;


public class IntegrationTests
{   
    /*
    [Fact]
    public async Task TestAddCheep()
    {
        var repository = await TestUtilities.createInMemoryDB();

        var cheepsBefore = await repository.GetCheepsFromAuthor(0, "Mellie Yost");
        
        await repository.AddCheep("hiii" );
        
        var cheepsAfter = await repository.GetCheepsFromAuthor(0, "Mellie Yost");
        
        Assert.True(cheepsBefore.Count != cheepsAfter.Count);
    } */
}
