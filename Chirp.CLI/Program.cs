using DocoptNet;
using SimpleDB;

namespace Chirp.CLI;

//TODO Add documentation

class Program {
    
    static IDatabaseRepository<Cheep> database = new CSVDatabase<Cheep>();

    public static void Main(String[] args)
    {
        const string usage = @"Chirp CLI version.

Usage:
  chirp read <limit>
  chirp cheep <message>
  chirp (-h | --help)
  chirp --version

Options:
  -h --help     Show this screen.
  --version     Show version.
";
        
        var arguments = new Docopt().Apply(usage, args, version: "1.0", exit: true)!;
        
        if (arguments["read"].IsTrue) 
        {
            try // try catch replaces the previous if else statement to support being able to see all cheeps if limit is empty. (will also print everything if the user gives a limit that is not a number)
            {
                var limit = int.Parse(arguments["<limit>"].ToString());
                UserInterface.PrintCheeps(database.Read(limit));
            }
            catch (Exception e)
            {
                UserInterface.PrintCheeps(database.Read(null));
            }
        } else if (arguments["cheep"].IsTrue)
        {
            //string[] messages = args;
            var messages = arguments["<message>"].ToString();
            database.Store(new Cheep(Environment.UserName, messages, DateTimeOffset.Now.ToUnixTimeSeconds()));
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