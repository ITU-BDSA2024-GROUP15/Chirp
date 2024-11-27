using Chirp.Core;
using Chirp.Infrastructure.Chirp.Repositories;

namespace Chirp.Infrastructure.Chirp.Services;

public interface ICheepService
{
    public Task<List<CheepDto>> GetCheeps(int limit);
    public Task<List<CheepDto>> GetCheeps(int limit, string follower);
    public Task<List<CheepDto>> GetCheepsFromAuthor(int page, string author, string spectator);
    public Task AddCheep(string text, string name, string email);
    public Task<Author> GetAuthorByEmail(string email);
    public Task<Author?> GetAuthorByName(string name);
    public Task AddAuthor(string name, string email);
    public Task AddFollowing(string follower, string followed);
    public Task RemoveFollowing(string follower, string followed);
    public Task<List<Follow>> GetFollowed(string follower);
    public Task<List<CheepDto>> GetAllCheepsFromAuthor(string author);
    public Task<List<CheepDto>> GetCheepsForTimeline(string author, int page, string spectator);
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

           
           
           var result = await ConvertCheepsToCheepDtos(queryresult, ""); // follower is empty since it doesnt matter for users that arent logged in
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

        var result = await ConvertCheepsToCheepDtos(queryresult, follower);
        return result;
    }

    public async Task<List<CheepDto>> GetCheepsFromAuthor(int page, string author, string spectator)
    {
        if ( page == 0 )
        {
            page = 1;
        }
        var queryresult = await _cheepRepository.GetCheepsFromAuthor(page, author);
        var result = await ConvertCheepsToCheepDtos(queryresult, spectator);
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
        var Dtos = await ConvertCheepsToCheepDtos(cheeps, author);
        return Dtos;
    }


    private async Task<List<CheepDto>> GetAllCheepsForTimeline(string author, string spectator)//follower is the person whose page we are on, spectator is the person looking at the page
    {
        
        var cheepsByAuthor = _cheepRepository.GetAllCheepsFromAuthor(author);
        var cheepsByFollowed = _cheepRepository.GetAllCheepsFromFollowed(author);
       
        var cheepsByAuthorDtos = ConvertCheepsToCheepDtos(await cheepsByAuthor, spectator); //using spectator ensures we get the correct value on "follows" bool
        var cheepsByFollowedDtos = ConvertCheepsToCheepDtos(await cheepsByFollowed, spectator);
        await Task.WhenAll(cheepsByAuthorDtos, cheepsByFollowedDtos);
        //combine the lists inelegantly
        cheepsByAuthorDtos.Result.AddRange(cheepsByFollowedDtos.Result);
        var allCheeps = cheepsByAuthorDtos.Result;
        
        //sort it by time
        var result = allCheeps.OrderByDescending(c => c.Timestamp).ToList();
        return result;
        
    }
    public async Task<List<CheepDto>> GetCheepsForTimeline(string author, int page, string spectator) //ensures only 32 cheeps are returned
    {
        var allDtos = await GetAllCheepsForTimeline(author, spectator);
        var result = new List<CheepDto>();
        var start = ( page - 1 ) * 32;
        if ( start < 0 ) start = 0;
        if ( start < allDtos.Count )
        {
            for ( int i = start; i < allDtos.Count && i - start < 32; i++ )
            {
                result.Add(allDtos[i]);
            }
        }
        return result;
    }


    private async Task<List<CheepDto>> ConvertCheepsToCheepDtos(List<Cheep> cheeps, string follower)
    {
        //Gets a list over which Authors the current author follows
        var follows = await GetFollowed(follower);
        
        var result = new List<CheepDto>();
        foreach (var cheep in cheeps)
        {
            bool isFollowing = false;
            foreach ( var follow in follows ) // this could be more efficient
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
    
    
    
    
}
