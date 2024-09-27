﻿using Microsoft.Data.Sqlite;

namespace Chirp.Razor;

public class DBFacade<T>
{
    private string path;
    public DBFacade(string path)
    {
        this.path = path;
        if (path == null )
        {
            this.path = GetPathToDB();
        }
    }
    
    public List<Cheep> read(string query)
    {
        
        //string PATH = GetPathToDB();
        List<Cheep> records = new List<Cheep>();
        

        using ( var connection = new SqliteConnection($"Data Source={path}") )
        {
            try
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = query;

                using var reader = command.ExecuteReader();
                while ( reader.Read() )
                {
                    var cheep = new Cheep(
                        Convert.ToString(reader["username"]),
                        Convert.ToString(reader["text"]),
                        Convert.ToInt64(reader["pub_date"])
                    );
                    records.Add(cheep);
                }
            }
            catch ( SqliteException e )
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                connection.Close();
            }
            return records;

        }
        
    }
    
    private static string GetPathToDB()
    {
        string absolutePath = Environment.CurrentDirectory;
        string[] splitPath = absolutePath.Split(Path.DirectorySeparatorChar);
        string pathToDB = "";
        for (int i = 0; i < splitPath.Length - 1; i++)
        {
            pathToDB += splitPath[i] + Path.DirectorySeparatorChar;
            if (splitPath[i].ToLowerInvariant().Equals("chirp"))
            {  
                break;
            }
        }
        pathToDB += "/src/Chirp.Razor/data/chirp.db";
        return pathToDB;
        
    }
    
}