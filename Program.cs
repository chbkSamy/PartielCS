using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace AirQualityApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            List<string> largestCitiesInFrance = new List<string>
            {
                "Paris",
                "Marseille",
                "Lyon",
                "Toulouse",
                "Nice",
                "Nantes",
                "Strasbourg",
                "Montpellier",
                "Bordeaux",
                "Lille",
                "Rennes",
                "Reims",
                "Le Havre",
                "Saint-Étienne",
                "Toulon"
            };

            var cityAirQualityList = new List<CityAirQuality>();

            foreach (var city in largestCitiesInFrance)
            {
                double airQuality = await GetAirQuality(city);
                if (!double.IsNaN(airQuality))
                {
                    cityAirQualityList.Add(new CityAirQuality { City = city, AQI = airQuality });
                }
            }

            var sortedCityAirQualityList = cityAirQualityList.OrderBy(c => c.AQI).ToList();

            Console.WriteLine("Les villes de France classées par qualité de l'air (AQI) :");
            for (int i = 0; i < sortedCityAirQualityList.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {sortedCityAirQualityList[i].City} - AQI : {sortedCityAirQualityList[i].AQI}");
            }
        }

        static async Task<double> GetAirQuality(string city)
        {
            using (var httpClient = new HttpClient())
            {
                string token = "33cd76f9e9069ea58c067a34af3551143167ffdd"; // Remplacez par votre clé API AQICN
                string apiUrl = $"https://api.waqi.info/feed/{city}/?token={token}";
                var response = await httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<AQICNResult>(json);
                    return result?.data?.aqi ?? double.NaN;
                }
                else
                {
                    Console.WriteLine($"Erreur HTTP : {response.StatusCode}");
                    return double.NaN;
                }
            }
        }
    }

    public class CityAirQuality
    {
        public string City { get; set; } = string.Empty; // Initialisation avec une valeur par défaut
        public double AQI { get; set; }
    }

    public class AQICNResult
    {
        public AQICNData? data { get; set; } // Propriété nullable
    }

    public class AQICNData
    {
        public double aqi { get; set; }
    }
}
