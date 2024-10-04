using Chirp.Razor.Datamodel;

namespace Chirp.Razor;

public interface ICheepRepository
{
    public Task<List<Cheep>> GetCheeps();
    public Task<List<Cheep>> GetCheepsFromAuthor(string author);
}