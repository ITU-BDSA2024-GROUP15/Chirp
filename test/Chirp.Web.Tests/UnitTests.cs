using Xunit;
using Chirp.Core;
using Chirp.Infrastructure.Chirp.Repositories;
using Chirp.Infrastructure.Chirp.Services;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Web.Tests;

public class UnitTests
{

    public UnitTests()
    {
    }
    
    
    //CheepService
    [Fact]
    public async Task TestGetCheepsAmount()
    {
        var _repository = await TestUtilities.createInMemoryDB();
                
        var cheeps = await _repository.GetCheeps(0);
        Assert.Equal(32,cheeps.Count);
        TestUtilities.closeConnection();
        
    }
    
    
    /*
    [Fact]
    public async Task TestGetCheepsPage1()
    {
        
    }
    
    [Fact]
    public void TestGetCheepsPage2()
    {
        ICheepService service = new CheepService();
        service.ChangeDB(new DBFacade("test"));
        List<Cheep> cheeps = service.GetCheeps(2);
        var cheep = cheeps[0];
        Assert.Equal("In the morning of the wind, some few splintered planks, of what present avail to him.", cheep.Message);  
    }
    
    [Fact]
    public void TestGetCheepsLastPage() 
    {
        ICheepService service = new CheepService();
        service.ChangeDB(new DBFacade("test"));
        List<Cheep> cheeps = service.GetCheeps(21);
        
        Assert.True(cheeps.Count == 17);
    }
    
    [Fact]
    public void TestGetCheepsBeyondLimit() 
    {
        ICheepService service = new CheepService();
        service.ChangeDB(new DBFacade("test"));
        List<Cheep> cheeps = service.GetCheeps(32);
        
        Assert.True(cheeps.Count == 0);    
    }
    
    
    [Fact]
    public void TestGetCheepsFromAuthor() 
    {
        ICheepService service = new CheepService();
        service.ChangeDB(new DBFacade("test"));
        List<Cheep> cheeps = service.GetCheepsFromAuthor("Jacqualine Gilcoine", (0));
        //we know Jacqualine isn't the first author on the public timeline.
        Assert.True(cheeps[0].Author == "Jacqualine Gilcoine" && cheeps.Count == 32);
    }
    
    [Fact]
    public void TestGetCheepsFromAuthorPage2()
    {
        ICheepService service = new CheepService();
        service.ChangeDB(new DBFacade("test"));
        List<Cheep> cheeps = service.GetCheepsFromAuthor("Jacqualine Gilcoine", (2));
        
        Assert.Equal("What a relief it was the place examined.", cheeps[0].Message);
    }    
    
    [Fact]
    public void TestGetCheepsFromAuthorLastPage()
    {
        ICheepService service = new CheepService();
        service.ChangeDB(new DBFacade("test"));
        List<Cheep> cheeps = service.GetCheepsFromAuthor("Jacqualine Gilcoine", (12));
        
        Assert.True(cheeps.Count == 7 && cheeps[0].Author == "Jacqualine Gilcoine");    
    }
    
    [Fact]
    public void TestGetCheepsFromAuthorBeyondLimit()
    {
        ICheepService service = new CheepService();
        service.ChangeDB(new DBFacade("test"));
        List<Cheep> cheeps = service.GetCheepsFromAuthor("Jacqualine Gilcoine", (20));
        
        Assert.True(cheeps.Count == 0);   
    }    



    //DBFacade
    [Fact]
    public void TestParseDateTime()
    {
        Assert.Equal("23:21:56 07-09-2024", DBFacade.parseDateTime(1725744116));
    }
    */
    
}