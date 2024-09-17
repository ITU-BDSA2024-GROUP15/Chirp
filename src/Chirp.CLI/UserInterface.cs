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
    public static DateTime parseDateTime(long timestamp)
    {
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(timestamp).ToLocalTime();
        return dateTime;
    }
}