using System.Collections;
using System.Globalization;
using System.Text.RegularExpressions;
using System;
using System.Net.Sockets;
using CsvHelper;
using CsvHelper.Configuration;
using SimpleDB;

class Program {
    
    static IDatabaseRepository<Cheep> database = new CSVDatabase<Cheep>();

    public static void Main(String[] args)
    {
        if (args[0] == "read")
        {
            if (args[1] != null)
            {
                var records = database.Read(int.Parse(args[1])).ToList();
                
                foreach (var record in records)
                {
                    Console.WriteLine(record.Author + " @ " + parseDataTime(record.Timestamp) + ": " + record.Message);
                }
            }
            else
            {
                Console.WriteLine("Specify how many cheeps you wanna read");
            }
           
        } else if (args[0] == "cheep")
        {
            //string[] messages = args;
            var messages = combineMessage(args);
            database.Store((new Cheep(Environment.UserName, messages, DateTimeOffset.Now.ToUnixTimeSeconds())));
            
            
            
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


