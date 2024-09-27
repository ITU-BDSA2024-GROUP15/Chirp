using DocoptNet;
using Microsoft.AspNetCore.Mvc.Razor;
using SimpleDB;

namespace Chirp.CLI;

//TODO Add documentation
/// <summary>
/// Class <c>Program</c> runs the program 
/// </summary>
class Program {
    
    /// <summary> 
    /// Method for running and executing the program
    /// </summary>
    /// <param name="args">The arguments that needs to be executed</param> 
    public static async Task Main(String[] args)
    {
        const string usage = @"Chirp CLI version. 

        Usage:
            chirp read --all 
            chirp read <limit>
            chirp readLatest <limit>
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
               await UserInterface.PrintCheeps(null);
            }
            else
            {
                var limit = int.Parse(arguments["<limit>"].ToString());
               await UserInterface.PrintCheeps(limit);
            }
        } else if (arguments["readLatest"].IsTrue){
            var limit = -1 * int.Parse(arguments["<limit>"].ToString());
            await UserInterface.PrintCheeps(limit);

        }else if (arguments["cheep"].IsTrue)
        {
            var messages = arguments["<message>"].ToString();
            
            //Send cheep via userinterface
            Cheep newCheep = new Cheep(Environment.UserName, messages, DateTimeOffset.Now.ToUnixTimeSeconds());
            await UserInterface.SendCheep(newCheep);
        }
    }

    
}