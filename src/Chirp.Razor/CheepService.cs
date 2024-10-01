

using Chirp.Razor;

public interface ICheepService
{
    public List<Cheep> GetCheeps(int limit);
    public List<Cheep> GetCheepsFromAuthor(string author, int limit);
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
           return facade.read($"Select U.username, M.text, M.pub_date from message M join user U on M.author_id = U.user_id WHERE M.message_id BETWEEN (\"{limit}\" -1) * 31 AND \"{limit}\" * 31 ORDER BY M.pub_date DESC");
       }

    public List<Cheep> GetCheepsFromAuthor(string author, int limit)
    {
        if ( limit == 0 )
        {
            limit = 1;
        }
        // filter by the provided author name
        var facade = new DBFacade<Cheep>(null);
        return facade.read($"SELECT * FROM (SELECT U.username, M.text, M.pub_date, RANK() OVER (ORDER BY M.pub_date) AS ranked FROM message M JOIN user U ON m.author_id = U.user_id WHERE U.username = \"{author}\" ORDER BY M.pub_date DESC)WHERE ranked BETWEEN (\"{limit}\" -1) *31 AND \"{limit}\" * 31"); 
    }
    
 
}
