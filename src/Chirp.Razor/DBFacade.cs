using System.Reflection;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.FileProviders;

namespace Chirp.Razor;

public class DBFacade
{
    private string path;
    public DBFacade(string path)
    {
        this.path = path;
        if (path == null )
        {
            this.path = Environment.GetEnvironmentVariable("CHIRPDBPATH") ?? Path.GetTempPath() + "Chirp.db";

            if ( !File.Exists(path) )
            {
                createDB();
            }
        }

        if ( path == "test" )
        {
            this.path = GetPathToTestDB();
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


    private void createDB()
    {
        var embeddedProviderSchema = new EmbeddedFileProvider(Assembly.GetExecutingAssembly());
        using var readerSchema = embeddedProviderSchema.GetFileInfo("/data/schema.sql").CreateReadStream();
        using var srSchema = new StreamReader(readerSchema);

        var querySchema = srSchema.ReadToEnd();
        
        var embeddedProviderDump = new EmbeddedFileProvider(Assembly.GetExecutingAssembly());
        using var readerDump = embeddedProviderDump.GetFileInfo("/data/dump.sql").CreateReadStream();
        using var srDump = new StreamReader(readerDump);
        
        var queryDump = srDump.ReadToEnd();

        
        using var connection = new SqliteConnection($"Data Source={path}");
        
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = querySchema; //We execute schema
        command.ExecuteNonQuery();

        command.CommandText = queryDump; //We execute dump
        command.ExecuteNonQuery();
        
        connection.Close();
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
    
    
    private static string GetPathToTestDB()
    {
        var pathToDB = GetPathToChirp();
        pathToDB += "./test/data/test.db";
        return pathToDB;
        
    }
    
    private static string GetPathToChirp()
    {
        string absolutePath = Environment.CurrentDirectory;
        string[] splitPath = absolutePath.Split(Path.DirectorySeparatorChar);
        string pathToChirp = "";
        for (int i = 0; i < splitPath.Length - 1; i++)
        {
            pathToChirp += splitPath[i] + Path.DirectorySeparatorChar;
            if (splitPath[i].ToLowerInvariant().Equals("chirp"))
            {  
                break;
            }
        }
        return pathToChirp;
        
    }
    
}