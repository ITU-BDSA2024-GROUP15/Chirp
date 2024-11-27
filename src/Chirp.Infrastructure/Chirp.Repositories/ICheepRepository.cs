using Chirp.Core;

namespace Chirp.Infrastructure.Chirp.Repositories;

/// <summary>
/// Defines method signatures for handling cheeps
/// </summary>
public interface ICheepRepository
{
    public Task<List<Cheep>> GetCheeps(int page);
    public Task<List<Cheep>> GetCheepsFromAuthor(int page, string author);

    public Task<List<Cheep>> GetAllCheepsFromAuthor(string author);
    public Task<List<Cheep>> GetAllCheepsFromFollowed(string author);
    
    public Task AddCheep(string text, Author author);
   
}