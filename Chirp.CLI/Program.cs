using DocoptNet;
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
        const string usage = @"Chirp CLI version.

Usage:
  chirp read --all 
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
            if (arguments["--all"].IsTrue)
            {
                UserInterface.PrintCheeps(database.Read(null));
            }
            else
            {
                var limit = int.Parse(arguments["<limit>"].ToString());
                UserInterface.PrintCheeps(database.Read(limit));
            }
        } else if (arguments["cheep"].IsTrue)
        {
            //string[] messages = args;
            var messages = arguments["<message>"].ToString();
            database.Store(new Cheep(Environment.UserName, messages, DateTimeOffset.Now.ToUnixTimeSeconds()));
        }
    }
    
}