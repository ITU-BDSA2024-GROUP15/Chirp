namespace Chirp.Razor;

public class CheepRepository : ICheepRepository
{

    //This should handle data logic for cheep
    
    private readonly DBFacade dbFacade;


    public CheepRepository(DBFacade dbFacade)
    {
        this.dbFacade = dbFacade;
    }
    
    
    public Task<List<Cheep>> GetCheeps()
    {
        return null;
    }


    public Task<List<Cheep>> GetCheepsFromAuthor(string author)
    {
        return null;
    }
}