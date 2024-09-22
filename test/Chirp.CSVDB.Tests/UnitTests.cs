using Chirp.CLI;
using SimpleDB;

public class UnitTests
{
    [Fact]
    public void testReadAllCheeps() 
    {
        // TODO: Find a way to test ReadAll functionality
        IDatabaseRepository<Cheep> database = CSVDatabase<Cheep>.GetInstance();
        //ass database.Read(null);

    }
    
    [Fact]
    public async Task testReadLimitCheeps() //Should these be moved to UserInterface test?
    {
        var cheepsPrinted = await UserInterface.PrintCheeps(1);
        Assert.True(cheepsPrinted == 1);

    }
    
    
    [Fact]
    public async Task testSendCheep()
    {
        var before = await UserInterface.PrintCheeps();
        await UserInterface.SendCheep(new Cheep("TestSendCheep", "testing...", 1725744116));
        var after = await UserInterface.PrintCheeps();
        
        Assert.Equal(before + 1, after);
    }
    
}