using Chirp.Razor.Datamodel;
using Microsoft.EntityFrameworkCore;

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


    public async Task<List<Cheep>> GetCheepsFromAuthor(string author)
    {
        
        var query = from cheep in CheepDBContext.Cheeps
            where cheep.Author.Name == author
            select cheep;
        // Execute the query and store the results
        var result = await query.ToListAsync();
        return(result);
        
        return null;
    }
}