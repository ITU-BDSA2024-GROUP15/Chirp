using System.Collections;
using System.Text.RegularExpressions;

try
{
    using StreamReader reader = new StreamReader("chirp_cli_db.csv");
    string line;
    reader.ReadLine();
    while ((line = reader.ReadLine()) != null)
    {
        Console.WriteLine(line);
        
        
        //string time = line.Substring(line.)
        //Console.WriteLine(time);
        
       
        Regex nameSeperator = new Regex(",\"");
        string[] names = nameSeperator.Split(line);
        
        Console.WriteLine(names[0]);
        
        
        
    }
    
    
    
    
    
}
catch (IOException e)
{
    Console.WriteLine(e.Message);
    Console.WriteLine("did not ");
}