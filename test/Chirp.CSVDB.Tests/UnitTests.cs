using SimpleDB;

public class UnitTests
{
    [Fact]
    public void testReadAllCheeps()
    {
        public IDatabaseRepository<Cheep> database = new CSVDatabase<Cheep>();
        
        
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
        
    }
}