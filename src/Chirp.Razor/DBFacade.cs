using Microsoft.Data.Sqlite;

namespace Chirp.Razor;

public class DBFacade<T>
{
    private string path;
    public DBFacade(string path)
    {
        this.path = path;
        if (path == null )
        {
            this.path = GetPathToDB();
        }
    }
    
    public List<Cheep> read(string query)
    {
        
        List<Cheep> records = new List<Cheep>();
        

        using ( var connection = new SqliteConnection($"Data Source={path}") )
        {
            try
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = query;

                using var reader = command.ExecuteReader();
                while ( reader.Read() )
                {
                    var cheep = new Cheep(
                        Convert.ToString(reader["username"]),
                        Convert.ToString(reader["text"]),
                        parseDateTime(Convert.ToInt64(reader["pub_date"]))
                    );
                    records.Add(cheep);
                }
            }
            catch ( SqliteException e )
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                connection.Close();
            }
            return records;

        }
        
    }
    
    private static string GetPathToDB()
    {
        string absolutePath = Environment.CurrentDirectory;
        string[] splitPath = absolutePath.Split(Path.DirectorySeparatorChar);
        string pathToDB = "";
        for (int i = 0; i < splitPath.Length - 1; i++)
        {
            pathToDB += splitPath[i] + Path.DirectorySeparatorChar;
            if (splitPath[i].ToLowerInvariant().Equals("chirp"))
            {  
                break;
            }
        }
        pathToDB += "/src/Chirp.Razor/data/chirp.db";
        return pathToDB;
        
    }
    
    /// <summary>
    /// This method converts unix time into the AM/PM format
    /// </summary>
    /// <param name="timestamp">the unix timestamp</param>
    /// <returns>The converted time from the unix timestamp</returns>
    public static string parseDateTime(long timestamp)
    {
        //Convert unix timestamp to datetime (Datetimeoffset)
        DateTimeOffset dateTime = DateTimeOffset.FromUnixTimeSeconds(timestamp);
        
        //We get the danish timezone
        TimeZoneInfo danishTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
        
        //Converts the timestamp to danish timezone
        DateTime timeStamp = TimeZoneInfo.ConvertTime(dateTime, danishTimeZone).DateTime;
        
        //Formatting timestamp/date
        string timeStampFormatted = timeStamp.ToString("HH:mm:ss dd-MM-yyyy");
        return timeStampFormatted;
        
        
    }
    
}