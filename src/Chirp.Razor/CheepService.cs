

using Chirp.Razor;

public interface ICheepService
{
    public List<Cheep> GetCheeps(int limit);
    public List<Cheep> GetCheepsFromAuthor(string author, int limit);

    public void ChangeDB(DBFacade<Cheep> facade);
}

public class CheepService : ICheepService
{
    private DBFacade<Cheep> facade;


    public CheepService()
    {
        this.facade = new DBFacade<Cheep>(null);
    }
    
    public List<Cheep> GetCheeps(int limit) //TODO ASK TA IF THIS IS ILLEGAL
       {
           if ( limit == 0 )
           {
               limit = 1;
           }
           
           
           return facade.read($"Select U.username, M.text, M.pub_date from message M join user U on M.author_id = U.user_id WHERE M.message_id BETWEEN (\"{limit}\" -1) * 31 AND \"{limit}\" * 31 ORDER BY M.pub_date DESC");
       }

    public List<Cheep> GetCheepsFromAuthor(string author, int limit)
    {
        if ( limit == 0 )
        {
            limit = 1;
        }
        // filter by the provided author nam
        return facade.read($"SELECT * FROM (SELECT U.username, M.text, M.pub_date, RANK() OVER (ORDER BY M.pub_date) AS ranked FROM message M JOIN user U ON m.author_id = U.user_id WHERE U.username = \"{author}\" ORDER BY M.pub_date DESC)WHERE ranked BETWEEN (\"{limit}\" -1) *31 AND \"{limit}\" * 31"); 
    }
    

    public void ChangeDB(DBFacade<Cheep> facade)
    {
        this.facade = facade;
    }
 
}
