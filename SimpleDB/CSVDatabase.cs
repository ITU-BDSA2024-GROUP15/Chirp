using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace SimpleDB;

public sealed class CSVDatabase<T> : IDatabaseRepository<T>
{
    public IEnumerable<T> Read(int? limit = null)
    {
        using (var reader = new StreamReader("../SimpleDB/chirp_cli_db.csv"))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            IEnumerable<T> records;
            if (limit != null)
            {
                records = csv.GetRecords<T>().ToList().Take((int) limit); //We use the way we setup the Cheep class to "map" to how we stored the information in the csv file (it has a header - Author,Message,Timestamp)

            }
            else
            {
                records = csv.GetRecords<T>().ToList();
            }
            return records;
        }
        
    }

    public void Store(T record)
    {
        
        //Store it in a record ready to write to csv file by using CsvHelper
        
        
        var records = new List<T>();
        records.Add(record);

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false,
        };
        
        using (var stream = File.Open("../SimpleDB/chirp_cli_db.csv", FileMode.Append))
        using (var writer = new StreamWriter(stream))
        using (var csv = new CsvWriter(writer, config))
        {
            csv.WriteRecords(records);
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

    static DateTime parseDataTime(long timestamp)
    {
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(timestamp).ToLocalTime();
        return dateTime;
    }
    
    
}