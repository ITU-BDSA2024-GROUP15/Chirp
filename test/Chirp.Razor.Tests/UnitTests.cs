using Xunit;
using Chirp.Razor;

namespace Chirp.Razor.Tests;

public class UnitTests
{
    [Fact]
    public void testsomething()
    {
        
    }
    
    [Fact]
    public void TestParseDateTime()
    {
        Assert.Equal("23:21:56 07-09-2024", DBFacade<Cheep>.parseDateTime(1725744116));
    }
}