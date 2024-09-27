

using Chirp.Razor;

public interface ICheepService
{
    public List<Cheep> GetCheeps();
    public List<Cheep> GetCheepsFromAuthor(string author);
}

public class CheepService : ICheepService
{
    

    public List<Cheep> GetCheeps()
    {
        var facade = new DBFacade<Cheep>(null); //TODO ASK TA ABOUT ENVIRONMENT AND DEPENDENCY INJECTION
        return facade.read(@"Select U.username, M.text, M.pub_date from message M join user U on M.author_id = U.user_id ORDER BY M.pub_date DESC");
    }

    public List<Cheep> GetCheepsFromAuthor(string author)
    {
        // filter by the provided author name
        var facade = new DBFacade<Cheep>(null);
        return facade.read($"Select U.username, M.text, M.pub_date from message M join user U on M.author_id = U.user_id where U.username = \"{author}\" ORDER BY M.pub_date DESC"); 
    }

}