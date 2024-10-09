

using Chirp.Razor;

public interface ICheepService
{
    public Task<List<CheepDto>> GetCheeps(int limit);
    public Task<List<CheepDto>> GetCheepsFromAuthor(int page, string author);
}

public class CheepService : ICheepService
{
    private ICheepRepository _repository;


    public CheepService(ICheepRepository repository)
    {
        this._repository = repository;
    }
    
    public async Task<List<CheepDto>> GetCheeps(int page) //TODO ASK TA IF THIS IS ILLEGAL
       {
           if ( page == 0 )
           {
               page = 1;
           }
           var queryresult = await _repository.GetCheeps(page);
           var result = new List<CheepDto>();
           foreach (var cheep in queryresult)
           {    
               var dto = new CheepDto();
               dto.Author = cheep.Author.Name;
               dto.Message = cheep.Text;
               dto.Timestamp = cheep.Timestamp;
               result.Add(dto);
           }
           return result;
       }

    public async Task<List<CheepDto>> GetCheepsFromAuthor(int page, string author)
    {
        if ( page == 0 )
        {
            page = 1;
        }
        var queryresult = await _repository.GetCheepsFromAuthor(page, author);
        var result = new List<CheepDto>();
        foreach (var cheep in queryresult)
        {    
            var dto = new CheepDto();
            dto.Author = cheep.Author.Name;
            dto.Message = cheep.Text;
            dto.Timestamp = cheep.Timestamp;
            result.Add(dto);
        }
        return result;
    }
 
}
