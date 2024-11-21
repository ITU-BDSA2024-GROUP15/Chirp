using Chirp.Core;
using Chirp.Infrastructure.Chirp.Repositories;

namespace Chirp.Infrastructure.Chirp.Services;

public interface ICheepService
{
    public Task<List<CheepDto>> GetCheeps(int limit);
    public Task<List<CheepDto>> GetCheeps(int limit, string follower);
    public Task<List<CheepDto>> GetCheepsFromAuthor(int page, string author);
    public Task AddCheep(string text, string name, string email);
    public Task<Author> GetAuthorByEmail(string email);
    public Task<Author?> GetAuthorByName(string name);
    public Task AddAuthor(string name, string email);
    public Task AddFollowing(string follower, string followed);
    public Task RemoveFollowing(string follower, string followed);
    public Task<List<Follow>> GetFollowed(string follower);
    public Task<List<CheepDto>> GetAllCheepsFromAuthor(string author);
    public Task<List<CheepDto>> GetCheepsForTimeline(string author, int page);
    //TODO: Remove all unused or privately used methods
}

public class CheepService : ICheepService
{
    private ICheepRepository _cheepRepository;
    private IAuthorRepository _authorRepository;
    private IFollowRepository _followRepository;


    public CheepService(ICheepRepository cheepRepository, IAuthorRepository authorRepository, IFollowRepository followRepository)
    {
        this._cheepRepository = cheepRepository;
        this._authorRepository = authorRepository;
        this._followRepository = followRepository;
    }
    
    public async Task<List<CheepDto>> GetCheeps(int page)
       {
           
           
           
           if ( page == 0 )
           {
               page = 1;
           }
           var queryresult = await _cheepRepository.GetCheeps(page);

           
           
           var result = await ConvertCheepsToCheepDtos(queryresult);
           return result;
       }
    
    public async Task<List<CheepDto>> GetCheeps(int page, string follower) //for use when logged in, allows us to display the correct button, either follow or unfollow
    {
           
        Author author = await _authorRepository.GetAuthorByName(follower);
           
        if ( page == 0 )
        {
            page = 1;
        }
        var queryresult = await _cheepRepository.GetCheeps(page);
           
        //Gets a list over which Authors the current author follows
        var follows = await GetFollowed(author.Name);
        
        var result = new List<CheepDto>();
        foreach (var cheep in queryresult)
        {
            bool isFollowing = false;
            foreach ( var follow in follows )
            {
                if ( follow.Followed == cheep.Author.Name )
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
        var result = await ConvertCheepsToCheepDtos(queryresult);
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


    public async Task AddFollowing(string follower, string followed)
    {
        await _followRepository.AddFollowing(follower, followed);
    }


    public async Task RemoveFollowing(string follower, string followed)
    {
        await _followRepository.RemoveFollowing(follower, followed);
    }


    public async Task<List<Follow>> GetFollowed(string follower)
    {
        return await _followRepository.GetFollowed(follower); 
    }


    public async Task<List<CheepDto>> GetAllCheepsFromAuthor(string author)
    {
        var cheeps = await _cheepRepository.GetAllCheepsFromAuthor(author);
        var Dtos = await ConvertCheepsToCheepDtos(cheeps);
        return Dtos;
    }


    private async Task<List<CheepDto>> GetAllCheepsForTimeline(string author)
    {
        var cheepsByAuthor = _cheepRepository.GetAllCheepsFromAuthor(author);
        var cheepsByFollowed = _cheepRepository.GetAllCheepsFromFollowed(author);
        await Task.WhenAll(cheepsByAuthor, cheepsByFollowed);
        
        Console.Write("Cheeps by followed: " + cheepsByFollowed.Result.Count + " Cheeps by author: " + cheepsByAuthor.Result.Count);
        //combine the lists inelegantly
        cheepsByAuthor.Result.AddRange(cheepsByFollowed.Result);
        var allCheeps = await ConvertCheepsToCheepDtos(cheepsByAuthor.Result);
        
        //sort it by time
        var result = allCheeps.OrderBy(c => c.Timestamp).ToList();
        return result;
        
    }


    public async Task<List<CheepDto>> GetCheepsForTimeline(string author, int page) //untested
    {
        var allDtos = await GetAllCheepsForTimeline(author);
        var result = allDtos.Slice((page-1)*32, 32);
        return result;
    }


    private async Task<List<CheepDto>> ConvertCheepsToCheepDtos(List<Cheep> cheeps)
    {
        var result = new List<CheepDto>();
        foreach (var cheep in cheeps)
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
    
}
