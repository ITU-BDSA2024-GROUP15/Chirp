namespace Chirp.Razor;

public interface ICheepService
{
    public Task<List<CheepDTO>> GetCheeps(int limit);
    public Task<List<CheepDTO>> GetCheepsFromAuthor(int page, string author);
}

public class CheepService : ICheepService
{
    private ICheepRepository _repository;


    public CheepService(ICheepRepository repository)
    {
        this._repository = repository;
    }
    
    public async Task<List<CheepDTO>> GetCheeps(int page)
    {
        if ( page == 0 )
        {
            page = 1;
        }
        var queryresult = await _repository.GetCheeps(page);
        var result = new List<CheepDTO>();
        foreach (var cheep in queryresult)
        {    
            var dto = new CheepDTO();
            dto.author = cheep.Author.Name;
            dto.message = cheep.Text;
            dto.timestamp = cheep.Timestamp;
            result.Add(dto);
        }
        return result;
    }

    public async Task<List<CheepDTO>> GetCheepsFromAuthor(int page, string author)
    {
        if ( page == 0 )
        {
            page = 1;
        }
        var queryresult = await _repository.GetCheepsFromAuthor(page, author);
        var result = new List<CheepDTO>();
        foreach (var cheep in queryresult)
        {    
            var dto = new CheepDTO();
            dto.author = cheep.Author.Name;
            dto.message = cheep.Text;
            dto.timestamp = cheep.Timestamp;
            result.Add(dto);
        }
        return result;
    }
 
}