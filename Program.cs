// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Net.Http;
// using System.Text.Json;
// using System.Threading.Tasks;

// namespace AirQualityApp
// {
//     class Program
//     {
//         static async Task Main(string[] args)
//         {
//             List<string> largestCitiesInFrance = new List<string>
//             {
//                 "Paris",
//                 "Marseille",
//                 "Lyon",
//                 "Toulouse",
//                 "Nice",
//                 "Nantes",
//                 "Strasbourg",
//                 "Montpellier",
//                 "Bordeaux",
//                 "Lille",
//                 "Rennes",
//                 "Reims",
//                 "Le Havre",
//                 "Saint-Étienne",
//                 "Toulon"
//             };

//             var cityAirQualityList = new List<CityAirQuality>();

//             foreach (var city in largestCitiesInFrance)
//             {
//                 double airQuality = await GetAirQuality(city);
//                 if (!double.IsNaN(airQuality))
//                 {
//                     cityAirQualityList.Add(new CityAirQuality { City = city, AQI = airQuality });
//                 }
//             }

//             var sortedCityAirQualityList = cityAirQualityList.OrderBy(c => c.AQI).ToList();

//             Console.WriteLine("Les villes de France classées par qualité de l'air (AQI) :");
//             for (int i = 0; i < sortedCityAirQualityList.Count; i++)
//             {
//                 Console.WriteLine($"{i + 1}. {sortedCityAirQualityList[i].City} - AQI : {sortedCityAirQualityList[i].AQI}");
//             }
//         }

//         static async Task<double> GetAirQuality(string city)
//         {
//             using (var httpClient = new HttpClient())
//             {
//                 string token = "33cd76f9e9069ea58c067a34af3551143167ffdd"; // Remplacez par votre clé API AQICN
//                 string apiUrl = $"https://api.waqi.info/feed/{city}/?token={token}";
//                 var response = await httpClient.GetAsync(apiUrl);
//                 if (response.IsSuccessStatusCode)
//                 {
//                     var json = await response.Content.ReadAsStringAsync();
//                     var result = JsonSerializer.Deserialize<AQICNResult>(json);
//                     return result?.data?.aqi ?? double.NaN;
//                 }
//                 else
//                 {
//                     Console.WriteLine($"Erreur HTTP : {response.StatusCode}");
//                     return double.NaN;
//                 }
//             }
//         }
//     }

//     public class CityAirQuality
//     {
//         public string City { get; set; } = string.Empty;
//         public double AQI { get; set; }
//     }

//     public class AQICNResult
//     {
//         public AQICNData? data { get; set; }
//     }

//     public class AQICNData
//     {
//         public double aqi { get; set; }
//     }
// }
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace recupVille
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        private const string apiKey = "Hxx3krT5tohuvdC3sIJEtoQxPaOwwetzfzgwjfnX";
        private const string cityApiUrl = "https://api.api-ninjas.com/v1/city";

        static async Task Main(string[] args)
        {
            Console.WriteLine("Veuillez entrer le code du pays (par exemple, FR pour la France) :");
            string country = Console.ReadLine();

            Console.WriteLine($"Obtention des villes pour le pays : {country}");

            var cities = await GetTopCitiesByCountryAsync(country);

            if (cities == null || cities.Count == 0)
            {
                Console.WriteLine("Impossible d'obtenir la liste des villes.");
                return;
            }

            Console.WriteLine("Liste des villes obtenue avec succès:");

            foreach (var city in cities)
            {
                Console.WriteLine(city.Name);
            }
        }

        static async Task<List<City>> GetTopCitiesByCountryAsync(string country)
        {
            try
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("X-Api-Key", apiKey);
                Console.WriteLine("Envoi de la requête à l'API pour les villes...");
                var response = await client.GetAsync($"{cityApiUrl}?country={country}&limit=15");

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Erreur de l'API : {response.StatusCode}");
                    return new List<City>();
                }

                return await response.Content.ReadFromJsonAsync<List<City>>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des villes : {ex.Message}");
                return new List<City>();
            }
        }
    }

    class City
    {
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}

