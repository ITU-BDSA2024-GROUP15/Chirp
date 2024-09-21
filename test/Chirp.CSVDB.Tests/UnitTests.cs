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
    public void testReadLimitCheeps()
    {
        // TODO: Fix reference
        IDatabaseRepository<Cheep> database = CSVDatabase<Cheep>.GetInstance();
        //Assert.Single(database.Read(1)); 
    }
    
    
    [Fact]
    public void testStoreCheep()
    {
        IDatabaseRepository<Cheep> database = CSVDatabase<Cheep>.GetInstance();
        //var before = UserInterface.PrintCheeps(null).ToString().Length;
        var before = database.Read(null).Count();
        database.Store(new Cheep("User", "message", 1725744116));
        var after = database.Read(null).Count();
        //var after = UserInterface.PrintCheeps(null).ToString().Length;
        //Assert.Equal(before + 1, after);
    }
}