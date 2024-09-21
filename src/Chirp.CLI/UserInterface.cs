using SimpleDB;

namespace Chirp.CLI;

//TODO Add documentation
/// <summary>
/// Class <c>UserInterface</c> manages what needs to be shown to the user
/// </summary>
public static class UserInterface
{
    /// <summary>
    /// Method for printing cheeps in the form: Author - TimeStamp - Message
    /// </summary>
    /// <param name="cheeps">the collection of cheeps to be printed</param>
    public static void PrintCheeps(IEnumerable<Cheep> cheeps)
    {
        foreach (var cheep in cheeps)
        {
            Console.WriteLine(cheep.Author + " @ " + parseDateTime(cheep.Timestamp) + ": " + cheep.Message);
        }
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
        
        //Formatting timstamp/date
        string timeStampFormatted = timeStamp.ToString("dd-MM-yyyy HH:mm:ss");
        return timeStampFormatted;
        
        
    }
}