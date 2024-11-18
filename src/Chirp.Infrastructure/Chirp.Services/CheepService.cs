using Chirp.Core;
using Chirp.Infrastructure.Chirp.Repositories;

namespace Chirp.Infrastructure.Chirp.Services;

public interface ICheepService
{
    public Task<List<CheepDto>> GetCheeps(int limit);
    public Task<List<CheepDto>> GetCheeps(int limit, string authorName);
    public Task<List<CheepDto>> GetCheepsFromAuthor(int page, string author);
    public Task AddCheep(string text, string name, string email);
    public Task<Author> GetAuthorByEmail(string email);
    public Task<Author?> GetAuthorByName(string name);
    public Task AddAuthor(string name, string email);
    public Task AddFollowing(int authorId, string followerAuthorName, int followingId, string followsAuthorName);
    public Task RemoveFollowing(int authorId, string followerAuthorName, string followsAuthorName);
    public Task<List<Follow>> GetFollowing(int authorId, string followerAuthorName);

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
                   Timestamp = cheep.Timestamp,
                   Follows = false
               };
               result.Add(dto);
           }
           return result;
       }
    
    public async Task<List<CheepDto>> GetCheeps(int page, string authorName)
    {
           
        Author author = await _authorRepository.GetAuthorByName(authorName);
           
        if ( page == 0 )
        {
            page = 1;
        }
        var queryresult = await _cheepRepository.GetCheeps(page);
           
        //Gets a list over which Authors the current author follows
        var follows = await GetFollowing(author.Id, author.Name);
        
           
        var result = new List<CheepDto>();
        foreach (var cheep in queryresult)
        {
            bool isFollowing = false;
            foreach ( var follow in follows )
            {
                Console.WriteLine("FOLLOWS:");
                Console.WriteLine(follow.AuthorName);
                if ( follow.FollowsAuthorName == cheep.Author.Name )
                {
                    isFollowing = true;
                }
            }
               
            var dto = new CheepDto
            {
                Author = cheep.Author.Name,
                Message = cheep.Text,
                Timestamp = cheep.Timestamp,
                Follows = isFollowing
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
        await _cheepRepository.AddCheep(text, await GetAuthorByEmail(email));
    }


    public async Task AddFollowing(int authorId, string followerAuthorName, int followingId, string followsAuthorName)
    {
        await _authorRepository.AddFollowing(authorId, followerAuthorName, followingId, followsAuthorName);
    }


    public async Task RemoveFollowing(int authorId, string followerAuthorName, string followsAuthorName)
    {
        await _authorRepository.RemoveFollowing(authorId, followerAuthorName, followsAuthorName);
    }


    public async Task<List<Follow>> GetFollowing(int authorId, string followerAuthorName)
    {
        return await _authorRepository.GetFollowing(authorId, followerAuthorName); 
    }
    
    
}
