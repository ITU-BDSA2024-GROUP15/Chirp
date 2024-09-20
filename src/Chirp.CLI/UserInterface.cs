using System.Text;
using System.Text.Json;
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
    public static async Task PrintCheeps(int? limit = null)
    {
        HttpClient client = new HttpClient();
        var url = "http://localhost:5217/cheeps";
        var response = await client.GetAsync(url);
        
        var jsonResponse = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var cheeps = JsonSerializer.Deserialize<List<Cheep>>(jsonResponse, options);

        int count = 0;
        foreach (var cheep in cheeps)
        {
            if (count == limit)
            {
                break;
            }
            Console.WriteLine(cheep.Author + " @ " + parseDateTime(cheep.Timestamp) + ": " + cheep.Message);
            count++;
        }
    }


    public static async Task SendCheep(Cheep cheep)
    {
        HttpClient client = new HttpClient();
        
        //URL to server
        var url = "http://localhost:5217/cheep";
        
        //Make JSON object (Co-authored-by: Bing copilot)
        var cheepAsJSON = JsonSerializer.Serialize(cheep);
        var encodedContent = new StringContent(cheepAsJSON, Encoding.UTF8, "application/json");
        Console.WriteLine("Trying to send cheep");
        
        //Send cheep (JSON)
        var respone = await client.PostAsync(url, encodedContent);
        Console.WriteLine(respone + "aaa");
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