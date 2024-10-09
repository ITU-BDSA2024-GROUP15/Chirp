using Chirp.Razor.Datamodel;

namespace Chirp.Razor;

public interface ICheepRepository
{
    public Task<List<Cheep>> GetCheeps(int page);
    public Task<List<Cheep>> GetCheepsFromAuthor(int page, string author); 
}