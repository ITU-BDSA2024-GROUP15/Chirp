

using Chirp.Razor;

public interface ICheepService
{
    public List<Cheep> GetCheeps(int limit);
    public List<Cheep> GetCheepsFromAuthor(string author);
}

public class CheepService : ICheepService
{
    

    public List<Cheep> GetCheeps(int limit) //TODO ASK TA IF THIS IS ILLEGAL
       {
           if ( limit == 0 )
           {
               limit = 1;
           }
           
           var facade = new DBFacade<Cheep>(null); //TODO ASK TA ABOUT ENVIRONMENT AND DEPENDENCY INJECTION
           return facade.read($"Select U.username, M.text, M.pub_date from message M join user U on M.author_id = U.user_id WHERE M.message_id BETWEEN \"{limit}\" -1 * 32 AND \"{limit}\" * 32 ORDER BY M.pub_date DESC");
       }

    public List<Cheep> GetCheepsFromAuthor(string author)
    {
        // filter by the provided author name
        var facade = new DBFacade<Cheep>(null);
        return facade.read($"Select U.username, M.text, M.pub_date from message M join user U on M.author_id = U.user_id where U.username = \"{author}\" ORDER BY M.pub_date DESC"); 
    }
    
 
}
