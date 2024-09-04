using System.Collections;
using System.Globalization;
using System.Text.RegularExpressions;
using System;
using System.Net.Sockets;
using Chirp.CLI;
using CsvHelper;


if (args[0] == "read" && args.Length == 1)
{
    read();
} else if (args[0] == "cheep")
{
    string[] messages = args;
    cheep(messages);
    //e
}


static void read()
{
    
    using (var reader = new StreamReader("chirp_cli_db.csv"))
    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
    {
        var records = csv.GetRecords<Cheep>(); //We use the way we setup the Cheep class to "map" to how we stored the information in the csv file (it has a header - Author,Message,Timestamp)
        foreach (var record in records)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(record.Timestamp).ToLocalTime();
            Console.WriteLine(record.Author + " @ " + dateTime + ": " + record.Message);
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
    var records = new List<Cheep>();
    records.Add(newCheep);
    
    using (var writer = new StreamWriter("chirp_cli_db.csv"))
    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
    {
        //TODO it deletes the current data in the csv file
        
        //csv.NextRecord(); 
        csv.WriteRecords(records);
    }
    
    
    
    /*
    Console.WriteLine(message.Length);

    
    
    string userName = Environment.UserName;
    long dateTime = DateTimeOffset.Now.ToUnixTimeSeconds();

    string path = "chirp_cli_db.csv";
    using (StreamWriter sw = File.AppendText(path))
    {
        sw.WriteLine(userName + ",\"" + newMessage + "\"," + dateTime);
    }
    */
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

