using System.Collections;
using System.Globalization;
using System.Text.RegularExpressions;
using System;
using System.Net.Sockets;
using Chirp.CLI;
using CsvHelper;
using CsvHelper.Configuration;

class Program {

    public static void Main(String[] args)
    {
        if (args[0] == "read" && args.Length == 1)
        {
            read();
        } else if (args[0] == "cheep")
        {
            string[] messages = args;
            cheep(messages);
            //e
        }
    }

    static void read()
    {
        
        using (var reader = new StreamReader("chirp_cli_db.csv"))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var cheeps = csv.GetRecords<Cheep>(); //We use the way we setup the Cheep class to "map" to how we stored the information in the csv file (it has a header - Author,Message,Timestamp)
            foreach (var cheep in cheeps)
            {
                Console.WriteLine(cheep.Author + " @ " + parseDataTime(cheep.Timestamp) + ": " + cheep.Message);
            }
            
        }

    }

    static void cheep(string[] message)
    {
        
        string text = combineMessage(message);
        string userName = Environment.UserName;
        long dateTime = DateTimeOffset.Now.ToUnixTimeSeconds();

        //Create the new cheep
        Cheep newCheep = new Cheep(userName, text, dateTime);
        
        //Store it in a record ready to write to csv file by using CsvHelper
        var cheeps = new List<Cheep>();
        cheeps.Add(newCheep);

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false,
        };
        
        using (var stream = File.Open("chirp_cli_db.csv", FileMode.Append))
        using (var writer = new StreamWriter(stream))
        using (var csv = new CsvWriter(writer, config))
        {
            csv.WriteRecords(cheeps);
        }
        
    }


    static string combineMessage(string[] message)
    {
        string newMessage = "";
        for (int i = 1; i < message.Length; i++)
        {
            if (i == message.Length - 1)
            {
                newMessage += message[i];
            }
            else
            {
                newMessage += message[i] + " ";
            }
        }
        return newMessage;
    }

    static DateTime parseDataTime(long timestamp)
    {
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(timestamp).ToLocalTime();
        return dateTime;
    }
}


