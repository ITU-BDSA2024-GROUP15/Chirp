using SimpleDB;

namespace Chirp.CLI.Client.Tests;

public class UnitTests
{
    [Fact]
    public void TestPrintCheeps()
    {
        IEnumerable<Cheep> cheeps = new List<Cheep>();
        Cheep cheep = new Cheep("Bruger", "test test test w", 1725744116);
        
        Assert.Equal("Bruger @ 07-09-2024 23:21:56: test test test w",(cheep.Author + " @ " + UserInterface.parseDateTime(cheep.Timestamp) + ": " + cheep.Message));
        
        // 
    }


    [Fact]
    public void TestParseDateTime()
    {
        Assert.Equal("07-09-2024 23:21:56", UserInterface.parseDateTime(1725744116));
    }
    
}