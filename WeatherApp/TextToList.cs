using System;
using System.IO;
using System.Text.RegularExpressions;
using WeatherApp.Models;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace WeatherApp
{
    internal class TextToList
    {
        public static void ListList()
        {
            string pattern = @"(?<year>\d{4})-(?<month>(0[1-9]|1[0-2]))-(?<day>(0[1-9]|[12][0-9]|3[01]))\s(?<time>\d{2}:\d{2}:\d{2}),(?<plats>\w+),(?<temp>-?(4[0-9]|50|\d{1,2})\.\d+),(?<luftfuktighet>(0|[1-9][0-9]?|100))";

            List<WeatherData> allWeatherData = new List<WeatherData>();

            using (StreamReader reader = new StreamReader(@"..\..\..\Files\tempData_medFel.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Match match = Regex.Match(line, pattern);

                    if (match.Success)
                    {
                        string year = match.Groups["year"].Value;
                        string month = match.Groups["month"].Value;
                        string day = match.Groups["day"].Value;
                        string time = match.Groups["time"].Value;
                        string location = match.Groups["plats"].Value;
                        string temp = match.Groups["temp"].Value;
                        string humidity = match.Groups["luftfuktighet"].Value;

                        double tempParsed;
                        double humidityParsed;

                        bool tempParseSuccess = double.TryParse(temp, NumberStyles.Any, CultureInfo.InvariantCulture, out tempParsed);
                        bool humidityParseSuccess = double.TryParse(humidity, NumberStyles.Any, CultureInfo.InvariantCulture, out humidityParsed);

                        if (tempParseSuccess && humidityParseSuccess)
                        {
                            var weatherData = new WeatherData
                            {
                                Year = year,
                                Month = month,
                                Day = day,
                                Time = time,
                                Location = location,
                                Temp = tempParsed,
                                Humidity = humidityParsed
                            };
                            allWeatherData.Add(weatherData);
                        }
                        else
                        {
                            Console.WriteLine($"Fel vid parsning av temperatur eller fuktighet: Temp={temp}, Fuktighet={humidity}");
                        }
                    }
                }
            }

            List<WeatherData> indoorWeatherData = allWeatherData.Where(data => data.Location.ToLower() == "inne").ToList();
            List<WeatherData> outdoorWeatherData = allWeatherData.Where(data => data.Location.ToLower() == "ute").ToList();

            var indoorAverages = indoorWeatherData
                .GroupBy(data => new { data.Year, data.Month, data.Day })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    g.Key.Day,
                    AverageTemp = g.Average(x => x.Temp),
                    AverageHumidity = g.Average(x => x.Humidity)
                })
                .ToList();

            var outdoorAverages = outdoorWeatherData
                .GroupBy(data => new { data.Year, data.Month, data.Day })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    g.Key.Day,
                    AverageTemp = g.Average(x => x.Temp),
                    AverageHumidity = g.Average(x => x.Humidity)
                })
                .ToList();

            List<DailyWeatherAverage> indoorWeatherAverages = indoorAverages.Select(d => new DailyWeatherAverage
            {
                Year = d.Year,
                Month = d.Month,
                Day = d.Day,
                AverageTemp = d.AverageTemp,
                AverageHumidity = d.AverageHumidity
            }).ToList();

            List<DailyWeatherAverage> outdoorWeatherAverages = outdoorAverages.Select(d => new DailyWeatherAverage
            {
                Year = d.Year,
                Month = d.Month,
                Day = d.Day,
                AverageTemp = d.AverageTemp,
                AverageHumidity = d.AverageHumidity
            }).ToList();

            foreach (var indoorData in indoorWeatherAverages)
            {
                var outdoorData = outdoorWeatherAverages.FirstOrDefault(o => o.Year == indoorData.Year && o.Month == indoorData.Month && o.Day == indoorData.Day);

                if (outdoorData != null)
                {
                    Console.WriteLine($"Datum: {indoorData.Year}-{indoorData.Month}-{indoorData.Day} | Inomhus - Medeltemp: {indoorData.AverageTemp:F1}°C, Medelfuktighet: {indoorData.AverageHumidity:F1}%");
                    Console.WriteLine($"Datum: {indoorData.Year}-{indoorData.Month}-{indoorData.Day} | Utomhus - Medeltemp: {outdoorData.AverageTemp:F1}°C, Medelfuktighet: {outdoorData.AverageHumidity:F1}%");
                }
            }
        }
    }
}
