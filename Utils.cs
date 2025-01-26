using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Projet
{
    public class Utils
    {
        public static void ImportData(IMongoCollection<NetflixTitle> collection)
        {
            string filePath = "./assets/datasets/netflix_titles.csv";

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ",",
                IgnoreBlankLines = true
            };

            collection.DeleteMany(FilterDefinition<NetflixTitle>.Empty);

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, config))
            {
                var records = csv.GetRecords<NetflixTitle>().ToList();

                foreach (var record in records)
                {
                    if (record.DateAdded.HasValue && DateTime.TryParseExact(record.DateAdded.Value.ToString(), "MMMM dd, yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                    {
                        record.DateAdded = date;
                    }
                }

                collection.InsertMany(records);
            }

            Console.WriteLine("Data has been imported successfully!");
        }

        public static void CreerCollectionGenres(IMongoDatabase database, IMongoCollection<NetflixTitle> collection)
        {
            var genresCollection = database.GetCollection<BsonDocument>("genres");

            genresCollection.DeleteMany(FilterDefinition<BsonDocument>.Empty);

            var genres = collection.Aggregate()
                                .Unwind(x => x.ListedIn)
                                .Group(x => x["ListedIn"], g => new { Genre = g.Key })
                                .ToList();

            var documents = genres.Select(g => new BsonDocument { { "name", g.Genre } }).ToList();
            genresCollection.InsertMany(documents);

            Console.WriteLine("La collection `Genres` a été créée et remplie avec succès.");
        }

        public static void AfficherTousLesTitres(IMongoCollection<NetflixTitle> collection)
        {
            Console.WriteLine("=== Tous les Titres (10 premiers) ===");
            var titres = collection.Find(FilterDefinition<NetflixTitle>.Empty).Limit(10).ToList();

            foreach (var titre in titres)
            {
                Console.WriteLine($"{titre.Title} ({titre.ReleaseYear}) - {titre.Type}");
            }

            Console.WriteLine("\nAppuyez sur une touche pour revenir au menu.");
            Console.ReadKey();
        }

        public static void RechercherParActeur(IMongoCollection<NetflixTitle> collection)
        {
            Console.Write("Entrez le nom d'un acteur : ");
            var acteur = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(acteur))
            {
                Console.WriteLine("Nom d'acteur invalide.");
                return;
            }

            var filter = Builders<NetflixTitle>.Filter.AnyIn(x => x.Cast, new[] { acteur });
            var titres = collection.Find(filter).Limit(10).ToList();

            Console.WriteLine($"\n=== Résultats pour l'acteur : {acteur} (10 premiers) ===");
            if (!titres.Any())
            {
                Console.WriteLine("Aucun résultat trouvé.");
            }
            else
            {
                foreach (var titre in titres)
                {
                    Console.WriteLine($"{titre.Title} ({titre.ReleaseYear}) - {titre.Type}");
                }
            }

            Console.WriteLine("\nAppuyez sur une touche pour revenir au menu.");
            Console.ReadKey();
        }

        public static void TrierParGenre(IMongoDatabase database, IMongoCollection<NetflixTitle> collection)
        {
            var genresCollection = database.GetCollection<BsonDocument>("genres");

            Console.WriteLine("=== Genres Disponibles ===");

            var genres = genresCollection.Find(FilterDefinition<BsonDocument>.Empty).ToList();
            foreach (var genre in genres)
            {
                Console.WriteLine($"- {genre["name"]}");
            }

            Console.Write("\nEntrez un genre pour filtrer : ");
            var genreChoisi = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(genreChoisi))
            {
                Console.WriteLine("Genre invalide.");
                return;
            }

            var filter = Builders<NetflixTitle>.Filter.AnyEq(x => x.ListedIn, genreChoisi);
            var titres = collection.Find(filter).Limit(10).ToList();

            Console.WriteLine($"\n=== Résultats pour le genre : {genreChoisi} (10 premiers) ===");
            if (!titres.Any())
            {
                Console.WriteLine("Aucun résultat trouvé.");
            }
            else
            {
                foreach (var titre in titres)
                {
                    Console.WriteLine($"{titre.Title} ({titre.ReleaseYear}) - {titre.Type}");
                }
            }

            Console.WriteLine("\nAppuyez sur une touche pour revenir au menu.");
            Console.ReadKey();
        }

        public static void TrierParAnnee(IMongoCollection<NetflixTitle> collection)
        {
            Console.Write("Entrez une année de sortie : ");
            if (int.TryParse(Console.ReadLine(), out int annee))
            {
                var filter = Builders<NetflixTitle>.Filter.Eq(t => t.ReleaseYear, annee);
                var titres = collection.Find(filter).Limit(10).ToList();

                Console.WriteLine($"\n=== Résultats pour l'année : {annee} (10 premiers) ===");
                if (!titres.Any())
                {
                    Console.WriteLine("Aucun résultat trouvé.");
                }
                else
                {
                    foreach (var titre in titres)
                    {
                        Console.WriteLine($"{titre.Title} ({titre.ReleaseYear}) - {titre.Type}");
                    }
                }
            }
            else
            {
                Console.WriteLine("Année invalide.");
            }

            Console.WriteLine("\nAppuyez sur une touche pour revenir au menu.");
            Console.ReadKey();
        }

    }
}
