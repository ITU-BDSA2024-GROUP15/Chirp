using Xunit;
using Chirp.Razor;

namespace Chirp.Razor.Tests;

public class UnitTests
{
    //CheepService
    [Fact]
    public void TestGetCheeps()
    {
        ICheepService service = new CheepService();
        List<Cheep> Cheeps = service.GetCheeps(0);
        
        Assert.True(Cheeps.Count == 32);
    }

    
    

    [Fact]
    public void TestGetCheepsPage2()
    {
        
    }
    
    [Fact]
    public void TestGetCheepsLastPage() 
    {
        
    }
    
    [Fact]
    public void TestGetCheepsBeyondLimit() 
    {
        
    }
    
    
    [Fact]
    public void TestGetCheepsFromAuthor() 
    {
        
    }
    
    [Fact]
    public void TestGetCheepsFromAuthorPage2()
    {
        
    }    
    
    [Fact]
    public void TestGetCheepsFromAuthorLastPage()
    {
        
    }
    
    [Fact]
    public void TestGetCheepsFromAuthorBeyondLimit()
    {
        
    }    



    //DBFacade
    [Fact]
    public void TestParseDateTime()
    {
        Assert.Equal("23:21:56 07-09-2024", DBFacade<Cheep>.parseDateTime(1725744116));
    }
}