using System.Text;
using System.Text.Json;
using SimpleDB;

namespace Chirp.CLI;

//TODO Add documentation
/// <summary>
/// Class <c>UserInterface</c> manages what needs to be shown to the user
/// </summary>
public class UserInterface 
{
    /// <summary>
    /// Method for printing cheeps in the form: Author - TimeStamp - Message
    /// </summary>
    /// <param name="cheeps">the collection of cheeps to be printed</param>
    public async Task<int> PrintCheeps(int? limit = null)
    {
        
        var cheepService = new CheepService(); //SINGLETON????
        var cheeps = cheepService.GetCheeps();
        
        foreach (var cheep in cheeps)
        {
            Console.WriteLine(cheep.Author + " @ " + parseDateTime(cheep.Timestamp) + ": " + cheep.Message);
            
        }

        return 0;



        /*
        HttpClient client = new HttpClient();
        var url = "https://bdsagroup015chirpremotedb-fxevcsaweqfxdxgz.northeurope-01.azurewebsites.net/cheeps";
        if ( limit.HasValue )
        {
            url += $"?limit={limit.Value}";
        }

        var response = await client.GetAsync(url);

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var cheeps = JsonSerializer.Deserialize<List<Cheep>>(jsonResponse, options);

        foreach (var cheep in cheeps)
        {
            Console.WriteLine(cheep.Author + " @ " + parseDateTime(cheep.Timestamp) + ": " + cheep.Message);

        }

        return cheeps.Count;
        */
    }


    public async Task<int> PrintCheeps(string author)
    {

        var cheepService = new CheepService(); //SINGLETON????
        var cheeps = cheepService.GetCheepsFromAuthor(author);

        foreach ( var cheep in cheeps )
        {
            Console.WriteLine(cheep.Author + " @ " + parseDateTime(cheep.Timestamp) + ": " +
                              cheep.Message);

        }

        return 0;

    }


    /// <summary>
    /// Method for sending a cheep via httpclient
    /// </summary>
    /// <param name="cheep">the cheep to be send</param>
    public static async Task SendCheep(Cheep cheep)
    {
        HttpClient client = new HttpClient();
        
        //URL to server
        var url = "https://bdsagroup015chirpremotedb-fxevcsaweqfxdxgz.northeurope-01.azurewebsites.net/cheep";
        
        //Make JSON object (Co-authored-by: Bing copilot)
        var cheepAsJSON = JsonSerializer.Serialize(cheep);
        var encodedContent = new StringContent(cheepAsJSON, Encoding.UTF8, "application/json");
        Console.WriteLine("Trying to send cheep");
        
        //Send cheep and wait for response
        var respone = await client.PostAsync(url, encodedContent);
        if (respone.IsSuccessStatusCode)
        {
            Console.WriteLine("Cheep has been sent");
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