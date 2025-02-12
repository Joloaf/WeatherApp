using System;
using System.Collections.Generic;
using System.IO;

namespace WeatherApp
{
    internal static class PrintWeatherData
    {
        public static void PrintDailyAverages(List<dynamic> dailyAverage)
        {
            foreach (var data in dailyAverage)
            {
                string locationText = data.Location.ToLower() == "inne" ? "Inomhus" : "Utomhus";
                Console.WriteLine($"Datum: {data.Year}-{data.Month}-{data.Day} | {locationText} - Medeltemp: {data.AverageTemp:F1}°C, Medelfuktighet: {data.AverageHumidity:F1}%");
            }
        }

        public static void PrintMonthlyAverages(List<dynamic> monthlyAverage)
        {
            foreach (var item in monthlyAverage)
            {
                string locationText = item.Location.ToLower() == "inne" ? "Inomhus" : "Utomhus";
                Console.WriteLine($"Datum: {item.Year}-{item.Month} | {locationText} - Medeltemp: {item.AverageTemp:F1}°C, Medelfuktighet: {item.AverageHumidity:F1}%");
            }
        }
    }
}