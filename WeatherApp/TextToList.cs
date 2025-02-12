using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using WeatherApp.Models;

namespace WeatherApp
{
    internal class TextToList
    {
        public static void ListList()
        {
            string filePath = @"..\..\..\Files\tempData_medFel.txt";
            string pattern = @"(?<year>\d{4})-(?<month>\d{2})-(?<day>\d{2})\s(?<time>\d{2}:\d{2}:\d{2}),(?<place>\w+),(?<temp>-?\d+\.\d+),(?<humidity>\d+)";
            List<WeatherData> weatherList = new List<WeatherData>(); // Lista för att spara väderdata

            // Läs filen rad för rad med File.ReadLines()
            foreach (string line in File.ReadLines(filePath))
            {
                Match match = Regex.Match(line, pattern);
                if (match.Success)
                {
                    bool tempSuccess = double.TryParse(match.Groups["temp"].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out double temp);
                    bool humiditySuccess = double.TryParse(match.Groups["humidity"].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out double humidity);

                    if (tempSuccess && humiditySuccess)
                    {
                        weatherList.Add(new WeatherData
                        {
                            Year = match.Groups["year"].Value,
                            Month = match.Groups["month"].Value,
                            Day = match.Groups["day"].Value,
                            Time = match.Groups["time"].Value,
                            Location = match.Groups["place"].Value,
                            Temp = temp,
                            Humidity = humidity
                        });
                    }
                    else
                    {
                        Console.WriteLine($"Fel i raden: {line}");
                    }
                }
            }

            // Beräkna och visa medelvärden för dag och månad
            ShowAverages(weatherList, "dag", w => new { w.Year, w.Month, w.Day, w.Location });
            ShowAverages(weatherList, "månad", w => new { w.Year, w.Month, w.Location });

            // Spara månatliga medelvärden till fil
            SaveMonthlyAveragesToFile(weatherList, @"..\..\..\Files\monthlyAverages.txt");
        }

        // Funktion för att räkna ut och visa medeltemperatur och luftfuktighet
        private static void ShowAverages(List<WeatherData> data, string period, Func<WeatherData, object> groupBy)
        {
            var averages = data.GroupBy(groupBy)
                .Select(group => new
                {
                    Key = group.Key,
                    AvgTemp = group.Average(w => w.Temp),
                    AvgHumidity = group.Average(w => w.Humidity)
                });

            // Skriv ut resultaten
            foreach (var item in averages)
            {
                dynamic key = item.Key;
                string locationText = key.Location.ToLower() == "inne" ? "Inomhus" : "Utomhus";
                string dateText = period == "dag" ? $"{key.Year}-{key.Month}-{key.Day}" : $"{key.Year}-{key.Month}";

                Console.WriteLine($"Datum: {dateText} | {locationText} - Medeltemp: {item.AvgTemp:F1}°C, Medelfuktighet: {item.AvgHumidity:F1}%");
            }
        }

        // Funktion för att spara månatliga medelvärden till fil
        private static void SaveMonthlyAveragesToFile(List<WeatherData> data, string filePath)
        {
            var monthlyAverages = data.GroupBy(w => new { w.Year, w.Month, w.Location })
                .Select(group => new
                {
                    Year = group.Key.Year,
                    Month = group.Key.Month,
                    Location = group.Key.Location,
                    AvgTemp = group.Average(w => w.Temp),
                    AvgHumidity = group.Average(w => w.Humidity)
                });

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Year,Month,Location,AvgTemp,AvgHumidity");
                foreach (var item in monthlyAverages)
                {
                    writer.WriteLine($"{item.Year},{item.Month},{item.Location},{item.AvgTemp:F1},{item.AvgHumidity:F1}");
                }
            }
        }
    }
}

