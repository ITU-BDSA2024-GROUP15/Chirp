using SimpleDB;

namespace Chirp.CLI;

//TODO Add documentation

class Program {
    
    static IDatabaseRepository<Cheep> database = new CSVDatabase<Cheep>();

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