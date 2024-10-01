

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
           
           
           return facade.read($"SELECT * FROM(Select U.username, M.text, M.pub_date, ROW_NUMBER() OVER (ORDER BY M.pub_date DESC) AS RowNum from message M join user U on M.author_id = U.user_id) WHERE RowNum BETWEEN 1 +((\"{limit}\" - 1) * 32) AND \"{limit}\" * 32");
       }

    public List<Cheep> GetCheepsFromAuthor(string author, int limit)
    {
        if ( limit == 0 )
        {
            limit = 1;
        }
        // filter by the provided author name
        return facade.read($"SELECT * FROM (SELECT U.username, M.text, M.pub_date, ROW_NUMBER() OVER (ORDER BY M.pub_date DESC) AS RowNum FROM message M JOIN user U ON m.author_id = U.user_id WHERE U.username = \"{author}\")WHERE RowNum BETWEEN 1 + ((\"{limit}\" - 1) * 32) AND \"{limit}\" * 32"); 
    }
    

    public void ChangeDB(DBFacade<Cheep> facade)
    {
        this.facade = facade;
    }
 
}
