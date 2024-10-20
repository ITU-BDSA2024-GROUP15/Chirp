using Chirp.Core;

namespace Chirp.Infrastructure.Chirp.Repositories;

public interface ICheepRepository
{
    public Task<List<Cheep>> GetCheeps(int page);
    public Task<List<Cheep>> GetCheepsFromAuthor(int page, string author); 
    
    public void AddCheepWithNoAuthor(string author, string text, string email);
    
    public void AddCheepWithAuthor(string author, string text);
}