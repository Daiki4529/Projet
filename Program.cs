using MongoDB.Driver;
using MongoDB.Bson;
using CsvHelper.Configuration;
using System.Globalization;
using CsvHelper;

namespace Projet
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("NetflixData");
            var collection = database.GetCollection<NetflixTitle>("titles");

            if (args.Length > 0 && args[0] == "import")
            {   
                Utils.ImportData(collection);
                Utils.CreerCollectionGenres(database, collection);
                return;
            }

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Menu Netflix ===");
                Console.WriteLine("1. Afficher tous les titres");
                Console.WriteLine("2. Rechercher un titre par acteur");
                Console.WriteLine("3. Trier par genre");
                Console.WriteLine("4. Trier par année de sortie");
                Console.WriteLine("5. Quitter");
                Console.WriteLine("Veuillez choisir une option :");

                var cki = Console.ReadKey();
                Console.Clear();

                if (cki.Key == ConsoleKey.D1)
                {
                    Utils.AfficherTousLesTitres(collection);
                }
                else if (cki.Key == ConsoleKey.D2)
                {
                    Utils.RechercherParActeur(collection);
                }
                else if (cki.Key == ConsoleKey.D3)
                {
                    Utils.TrierParGenre(database, collection);
                }
                else if (cki.Key == ConsoleKey.D4)
                {
                    Utils.TrierParAnnee(collection);
                }
                else if (cki.Key == ConsoleKey.D5)
                {
                    Console.WriteLine("Au revoir !");
                    break;
                }
                else
                {
                    Console.WriteLine("Option invalide. Appuyez sur une touche pour revenir au menu.");
                    Console.ReadKey();
                }
            }
        }
    }
}