using Microsoft.Data.Sqlite;

namespace Chirp.Razor;

public class DBFacade<T>
{
    public IEnumerable<Cheep> read(string query)
    {
        string sqlDBFilePath = "./data/chirp.db";
        IEnumerable<Cheep> records = new List<Cheep>();
        using (var connection = new SqliteConnection($"Data Source={sqlDBFilePath}"))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = query;
            
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var cheep = new Cheep(
                    Convert.ToString(reader["author_id"]),
                    Convert.ToString(reader["text"]),
                    Convert.ToInt64(reader["pub_date"]) // decimal will be better for money
                );
                records.Append(cheep);
            }
            connection.Close();
        }
        return records;
    }
    
}