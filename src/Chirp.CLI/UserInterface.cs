using SimpleDB;

namespace Chirp.CLI;

//TODO Add documentation

public static class UserInterface
{
    public static void PrintCheeps(IEnumerable<Cheep> cheeps)
    {
        foreach (var cheep in cheeps)
        {
            Console.WriteLine(cheep.Author + " @ " + parseDataTime(cheep.Timestamp) + ": " + cheep.Message);
        }
    }
    
    
    public static DateTime parseDateTime(long timestamp)
    {
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(timestamp).ToLocalTime();
        return dateTime;
    }
}