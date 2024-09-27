using Microsoft.Data.Sqlite;

namespace Chirp.Razor;

public class DBFacade<T>
{
    public IEnumerable<Cheep> read(string query)
    {
        string sqlDBFilePath = "./data/chirp.db";
        IEnumerable<Cheep> records = new List<Cheep>();
        

        using ( var connection = new SqliteConnection($"Data Source={sqlDBFilePath}") )
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
                        Convert.ToString(reader["author_id"]),
                        Convert.ToString(reader["text"]),
                        Convert.ToInt64(reader["pub_date"])
                    );
                    records.Append(cheep);
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
    
}