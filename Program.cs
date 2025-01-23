using MongoDB.Driver;
using MongoDB.Bson;
using CsvHelper.Configuration;
using System.Globalization;
using CsvHelper;

class Program
{
    static void Main(string[] args)
    {
        var client = new MongoClient("mongodb://localhost:27017");
        var database = client.GetDatabase("NetflixData");
        var collection = database.GetCollection<NetflixTitle>("titles");

        string filePath = "./assets/datasets/netflix_titles.csv";

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            Delimiter = ",",
            IgnoreBlankLines = true
        };

        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, config))
        {
            var records = csv.GetRecords<NetflixTitle>().ToList();
            
            foreach (var record in records)
            {
                if (record.date_added.HasValue && DateTime.TryParseExact(record.date_added.Value.ToString(), "MMMM dd, yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                {
                    record.date_added = date;
                }
            }

            collection.InsertMany(records);
        }

        Console.WriteLine("Data has been imported successfully!");
    }
}