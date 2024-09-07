using SimpleDB;

namespace Chirp.CLI;

//TODO Add documentation
/// <summary>
/// Class <c>Program</c> runs the program
/// </summary>
class Program {
    
    static IDatabaseRepository<Cheep> database = new CSVDatabase<Cheep>();
    /// <summary>
    /// Method for running and executing the program
    /// </summary>
    /// <param name="args">The arguments that needs to be executed</param>
    public static void Main(String[] args)
    {
        if (args[0] == "read")
        {
            if (args.Length > 1)
            {
                UserInterface.PrintCheeps(database.Read(int.Parse(args[1])).ToList());
            }
            else
            {
                UserInterface.PrintCheeps(database.Read(null).ToList());
            }
            
           
        } else if (args[0] == "cheep")
        {
            //string[] messages = args;
            var messages = combineMessage(args);
            database.Store((new Cheep(Environment.UserName, messages, DateTimeOffset.Now.ToUnixTimeSeconds())));
            
            
            
        }
    }
    
    /// <summary>
    /// Method for creating a message for a record from an array of strings
    /// </summary>
    /// <param name="message">The text needed to be combined</param>
    /// <returns>The message as a single string</returns>
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

    
}