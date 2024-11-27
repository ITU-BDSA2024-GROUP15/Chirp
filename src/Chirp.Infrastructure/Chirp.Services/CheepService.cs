using Chirp.Core;
using Chirp.Infrastructure.Chirp.Repositories;

namespace Chirp.Infrastructure.Chirp.Services;

public interface ICheepService
{
    /// <summary>
    /// This method is for use on the public timeline.
    /// It returns a list of 0-32 cheeps, ordered from newest to oldest, by any author.
    /// The first 32*limit cheeps are skipped, to allow pagination
    /// </summary>
    /// <param name="limit"> The page number </param>
    /// <returns> A list of CheepDto objects </returns>
    public Task<List<CheepDto>> GetCheeps(int limit);
    /// <summary>
    /// This method is for use on the public timeline.
    /// It returns a list of 0-32 cheeps, ordered from newest to oldest, by any author.
    /// The first 32*limit cheeps are skipped, to allow pagination
    /// This method sets the "following" attribute on the returned cheepdtos correctly, in order to show the correct follow/ unfollow button
    /// </summary>
    /// <param name="limit"> The page number </param>
    /// <param name="follower"> The user who is viewing the timeline </param>
    /// <returns> A list of CheepDto objects </returns>
    public Task<List<CheepDto>> GetCheeps(int limit, string follower);
    /// <summary>
    /// This method is for use on a private timeline.
    /// It returns a list of 0-32 cheeps, ordered from newest to oldest, by the specified author.
    /// The first 32*limit cheeps are skipped, to allow pagination
    /// </summary>
    /// <param name="page"> The page number </param>
    /// <param name="author"> The name of the author whose cheeps you want </param>
    /// <returns> A list of CheepDto objects </returns>
    public Task<List<CheepDto>> GetCheepsFromAuthor(int page, string author);
    /// <summary>
    /// This method allows adding new cheeps to the database
    /// </summary>
    /// <param name="text"> The contents of the cheep </param>
    /// <param name="name"> The username of the author </param>
    /// <param name="email"> The email of the author </param>
    /// <returns> Task </returns>
    public Task AddCheep(string text, string name, string email);
    /// <summary>
    /// This method allows for getting an Author object from their username
    /// </summary>
    /// <param name="name"> The username of the author </param>
    /// <returns> The Author object matching the username </returns>
    public Task<Author?> GetAuthorByName(string name);
    /// <summary>
    /// This method allows adding new tuples to the Follow relation
    /// </summary>
    /// <param name="follower"> The username of the author that should follow another </param>
    /// <param name="followed"> The username of the author that should be followed </param>
    /// <returns> Task </returns>
    public Task AddFollowing(string follower, string followed);
    /// <summary>
    /// This method allows removing tuples from the Follow relation
    /// </summary>
    /// <param name="follower"> The username of the author that should follow another </param>
    /// <param name="followed"> The username of the author that should be followed </param>
    /// <returns> Task </returns>
    public Task RemoveFollowing(string follower, string followed);
    /// <summary>
    /// This method allows getting all the users followed by a specified author
    /// </summary>
    /// <param name="follower"> The username of the author </param>
    /// <returns> A list of all follow relations containing the author as follower </returns>
    public Task<List<Follow>> GetFollowed(string follower);
    /// <summary>
    /// This method allows getting all cheeps by a specified author
    /// </summary>
    /// <param name="author"> The username of the author</param>
    /// <returns> A list of all cheeps by the author </returns>
    public Task<List<CheepDto>> GetAllCheepsFromAuthor(string author);
    /// <summary>
    /// This method allows getting cheeps for an authors timeline
    /// </summary>
    /// <param name="author"> The username of the author </param>
    /// <param name="page"> The page number </param>
    /// <returns></returns>
    public Task<List<CheepDto>> GetCheepsForTimeline(string author, int page);
    public Task DeleteFromFollows(string username);


    public Task<List<Follow>> GetFollowers(string followed);
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
           
        Author? author = await _authorRepository.GetAuthorByName(follower);
           
        if ( page == 0 )
        {
            page = 1;
        }
        var queryresult = await _cheepRepository.GetCheeps(page);

        var result = await ConvertCheepsToCheepDtos(queryresult, follower);
        return result;
    }

    public async Task<List<CheepDto>> GetCheepsFromAuthor(int page, string author)
    {
        if ( page == 0 )
        {
            page = 1;
        }
        var queryresult = await _cheepRepository.GetCheepsFromAuthor(page, author);
        var result = await ConvertCheepsToCheepDtos(queryresult, author);
        return result;
    }


    public async Task<Author?> GetAuthorByEmail(string email)
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
        var author = await GetAuthorByEmail(email);
        if (author == null)
        {
            return;
        }
        await _cheepRepository.AddCheep(text, author);
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


    public async Task<List<Follow>> GetFollowers(string followed)
    {
        return await _followRepository.GetFollowers(followed);
    }


    public async Task DeleteFromFollows(string username)
    {
        //Delete all relations where user is followed by others
        var follows = await GetFollowers(username);
        foreach (var follow in follows)
        {
            await RemoveFollowing(follow.Follower, follow.Followed);        
        } 
        //Delete all relations where others follow the user
        follows = await GetFollowed(username);
        foreach (var follow in follows)
        {
            await RemoveFollowing(follow.Follower, follow.Followed);        
        } 
        
    }


    public async Task<List<CheepDto>> GetAllCheepsFromAuthor(string author)
    {
        var cheeps = await _cheepRepository.GetAllCheepsFromAuthor(author);
        var dtos = await ConvertCheepsToCheepDtos(cheeps, author);
        return dtos;
    }


    private async Task<List<CheepDto>> GetAllCheepsForTimeline(string author)
    {
        
        var cheepsByAuthor = _cheepRepository.GetAllCheepsFromAuthor(author);
        var cheepsByFollowed = _cheepRepository.GetAllCheepsFromFollowed(author);
       
        var cheepsByAuthorDtos = ConvertCheepsToCheepDtos(await cheepsByAuthor, author);
        var cheepsByFollowedDtos = ConvertCheepsToCheepDtos(await cheepsByFollowed, author);
        await Task.WhenAll(cheepsByAuthorDtos, cheepsByFollowedDtos);
        
        //combine the lists inelegantly
        cheepsByAuthorDtos.Result.AddRange(cheepsByFollowedDtos.Result);
        var allCheeps = cheepsByAuthorDtos.Result;
        
        //sort it by time
        var result = allCheeps.OrderByDescending(c => c.Timestamp).ToList();
        return result;
        
    }
    public async Task<List<CheepDto>> GetCheepsForTimeline(string author, int page) //ensures only 32 cheeps are returned
    {
        var allDtos = await GetAllCheepsForTimeline(author);
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


    private async Task<List<CheepDto>> ConvertCheepsToCheepDtos(List<Cheep> cheeps, string author)
    {
        //Gets a list over which Authors the current author follows
        var follows = await GetFollowed(author);
        
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
