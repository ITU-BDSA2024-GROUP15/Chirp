using Xunit;
using Chirp.Razor;

namespace Chirp.Razor.Tests;

public class UnitTests
{
    //CheepService
    [Fact]
    public void TestGetCheepsAmount()
    {
        ICheepService service = new CheepService();
        service.ChangeDB(new DBFacade<Cheep>("test"));
        List<Cheep> Cheeps = service.GetCheeps(0);
        
        Assert.True(Cheeps.Count == 32);
    }
    
    

    [Fact]
    public void TestGetCheepsPage1()
    {
        ICheepService service = new CheepService();
        service.ChangeDB(new DBFacade<Cheep>("test"));
        List<Cheep> Cheeps = service.GetCheeps(1);
        var cheep = Cheeps[0];
        Assert.Equal("Starbuck now is what we hear the worst.", cheep.Message);    
    }
    
    [Fact]
    public void TestGetCheepsPage2()
    {
        ICheepService service = new CheepService();
        service.ChangeDB(new DBFacade<Cheep>("test"));
        List<Cheep> Cheeps = service.GetCheeps(2);
        var cheep = Cheeps[0];
        Assert.Equal("To him it had done a great fish to swallow up the steel head of the cetacea.", cheep.Message);  
    }
    
    [Fact]
    public void TestGetCheepsLastPage() 
    {
        ICheepService service = new CheepService();
        service.ChangeDB(new DBFacade<Cheep>("test"));
        List<Cheep> Cheeps = service.GetCheeps(22);
        
        Assert.True(Cheeps.Count == 6);
    }
    
    [Fact]
    public void TestGetCheepsBeyondLimit() 
    {
        ICheepService service = new CheepService();
        service.ChangeDB(new DBFacade<Cheep>("test"));
        List<Cheep> Cheeps = service.GetCheeps(32);
        
        Assert.True(Cheeps.Count == 0);    
    }
    
    
    [Fact]
    public void TestGetCheepsFromAuthor() 
    {
        ICheepService service = new CheepService();
        service.ChangeDB(new DBFacade<Cheep>("test"));
        List<Cheep> Cheeps = service.GetCheepsFromAuthor("Jacqualine Gilcoine", (0));
        //we know Jacqualine isn't the first author on the public timeline.
        Assert.True(Cheeps[0].Author == "Jacqualine Gilcoine" && Cheeps.Count == 32);
    }
    
    [Fact]
    public void TestGetCheepsFromAuthorPage2()
    {
        ICheepService service = new CheepService();
        service.ChangeDB(new DBFacade<Cheep>("test"));
        List<Cheep> Cheeps = service.GetCheepsFromAuthor("Jacqualine Gilcoine", (2));
        
        Assert.Equal("No, it's no go.", Cheeps[0].Message);
    }    
    
    [Fact]
    public void TestGetCheepsFromAuthorLastPage()
    {
        ICheepService service = new CheepService();
        service.ChangeDB(new DBFacade<Cheep>("test"));
        List<Cheep> Cheeps = service.GetCheepsFromAuthor("Jacqualine Gilcoine", (12));
        
        Assert.True(Cheeps.Count == 19 && Cheeps[0].Author == "Jacqualine Gilcoine");    
    }
    
    [Fact]
    public void TestGetCheepsFromAuthorBeyondLimit()
    {
        ICheepService service = new CheepService();
        service.ChangeDB(new DBFacade<Cheep>("test"));
        List<Cheep> Cheeps = service.GetCheepsFromAuthor("Jacqualine Gilcoine", (15));
        
        Assert.True(Cheeps.Count == 0 && Cheeps[0].Author == "Jacqualine Gilcoine");   
    }    



    //DBFacade
    [Fact]
    public void TestParseDateTime()
    {
        Assert.Equal("23:21:56 07-09-2024", DBFacade<Cheep>.parseDateTime(1725744116));
    }
}