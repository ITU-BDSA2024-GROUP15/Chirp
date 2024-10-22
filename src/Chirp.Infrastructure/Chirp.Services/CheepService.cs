using Chirp.Core;
using Chirp.Infrastructure.Chirp.Repositories;

namespace Chirp.Infrastructure.Chirp.Services;

public interface ICheepService
{
    public Task<List<CheepDto>> GetCheeps(int limit);
    public Task<List<CheepDto>> GetCheepsFromAuthor(int page, string author);
}

public class CheepService : ICheepService
{
    private ICheepRepository _cheepRepository;
    private IAuthorRepository _authorRepository;


    public CheepService(ICheepRepository cheepRepository, IAuthorRepository authorRepository)
    {
        this._cheepRepository = cheepRepository;
        this._authorRepository = authorRepository;
    }
    
    public async Task<List<CheepDto>> GetCheeps(int page)
       {
           if ( page == 0 )
           {
               page = 1;
           }
           var queryresult = await _cheepRepository.GetCheeps(page);
           var result = new List<CheepDto>();
           foreach (var cheep in queryresult)
           {    
               var dto = new CheepDto
               {
                   Author = cheep.Author.Name,
                   Message = cheep.Text,
                   Timestamp = cheep.Timestamp
               };
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
        var queryresult = await _cheepRepository.GetCheepsFromAuthor(page, author);
        var result = new List<CheepDto>();
        foreach (var cheep in queryresult)
        {    
            var dto = new CheepDto
            {
                Author = cheep.Author.Name,
                Message = cheep.Text,
                Timestamp = cheep.Timestamp
            };
            result.Add(dto);
        }
        return result;
    }


    public async Task<Author> GetAuthorByEmail(string email)
    {
       return await _authorRepository.GetAuthorByEmail(email);
        
    }

    public async Task<Author?> GetAuthorByName(string name)
    {
        return await _authorRepository.GetAuthorByName(name);
        
    }


    public async Task AddAuthor(string name, string email)
    {
        await _authorRepository.CreateAuthor(name, email);
    }


    public async Task AddCheep(string text, string name, string email)
    {
        if ( await GetAuthorByName(name) == null )
        {
            await AddAuthor(name, email);
        }
        await _cheepRepository.AddCheep(name, await GetAuthorByEmail(email));
    }
}
