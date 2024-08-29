using System.Collections;
using System.Globalization;
using System.Text.RegularExpressions;
using System;
using System.Net.Sockets;


if (args[0] == "read" && args.Length == 1)
{
    read();
} else if (args[0] == "cheep")
{
    string[] messages = args;
    cheep(messages);
}



static void read()
{
    try
    {
        using StreamReader reader = new StreamReader("C://ITU//3. Semester//BDSA//Chirp//Chirp.CLI//chirp_cli_db.csv");
        string line;
        reader.ReadLine();
        while ((line = reader.ReadLine()) != null)
        {
            //Gets name 
            Regex nameSeperator = new Regex("(,\")|(\",)");
            string[] names = nameSeperator.Split(line);
        
            //Gets date
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(double.Parse(names[4])).ToLocalTime();
        
        
            Console.WriteLine(names[0] + " @ "+ dateTime + ": " + names[2]);
        }
 
    }
    catch (IOException e)
    {
        Console.WriteLine(e.Message);
        Console.WriteLine("did not ");
    }
}

static void cheep(string[] message)
{
    Console.WriteLine(message.Length);

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
    
    string userName = Environment.UserName;
    long dateTime = DateTimeOffset.Now.ToUnixTimeSeconds();

    string path = "C://ITU//3. Semester//BDSA//Chirp//Chirp.CLI//chirp_cli_db.csv";
    using (StreamWriter sw = File.AppendText(path))
    {
        sw.WriteLine(userName + ",\"" + newMessage + "\"," + dateTime);
    }
}


