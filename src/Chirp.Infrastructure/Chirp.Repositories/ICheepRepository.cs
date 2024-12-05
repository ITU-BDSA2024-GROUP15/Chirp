using Chirp.Core;

namespace Chirp.Infrastructure.Chirp.Repositories;

/// <summary>
/// Defines method signatures for handling cheeps
/// </summary>
public interface ICheepRepository
{
    
    /// <summary>
    /// Gets cheeps for a given public page
    /// </summary>
    /// <param name="page">The page number</param>
    /// <returns>List of cheeps</returns>
    public Task<List<Cheep>> GetCheeps(int page);
    
    /// <summary>
    /// Gets cheeps made by a specific author and page
    /// </summary>
    /// <param name="page">The given page</param>
    /// <param name="author">The given Author </param>
    /// <returns>List of cheeps made the author for the specific page</returns>
    public Task<List<Cheep>> GetCheepsFromAuthor(int page, string author);

    /// <summary>
    /// Gets all cheeps from an author
    /// </summary>
    /// <param name="author"></param>
    /// <returns>List of all cheeps made by the author</returns>
    public Task<List<Cheep>> GetAllCheepsFromAuthor(string author);
    
    /// <summary>
    /// Gets all the cheeps from all the different authors that an author follows
    /// </summary>
    /// <param name="author">Name of author who follows</param>
    /// <returns>List of cheeps</returns>
    public Task<List<Cheep>> GetAllCheepsFromFollowed(string author);
    
    /// <summary>
    /// Used to create a new cheep
    /// </summary>
    /// <param name="text">The cheep message</param>
    /// <param name="author">Author of cheep</param>
    public Task AddCheep(string text, Author author);


    public Task AddLike(string author, int cheepId);
    public Task RemoveLike(string author, int cheepId);
    
    /// <summary>
    /// Counts the likes a cheep has
    /// </summary>
    /// <param name="cheepId">Id of cheep</param>
    /// <returns>Amount of likes</returns>
    public Task<int> CountLikes(int cheepId);

    /// <summary>
    /// Gets a list of all liked cheeps for a given author
    /// </summary>
    /// <param name="author">The name of the author in question</param>
    /// <returns>A list of cheeps</returns>
    public Task<List<Cheep>> GetAllLiked(string author);

    /// <summary>
    /// Finds and removes all instances of the authors name from the liked list of cheeps
    /// </summary>
    /// <param name="authorName"></param>
    /// <returns></returns>
    public Task DeleteAllLikes(string authorName);
    /// <summary>
    /// Gets a list of the 32 most liked cheeps
    /// </summary>
    /// <returns>A list of 32 cheeps</returns>
    public Task<List<Cheep>> GetTopLikedCheeps();

}