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
        public static List<WeatherData> ListList()
        {
            string filePath = @"..\..\..\Files\tempData_medFel.txt";
            string pattern = @"(?<year>\d{4})-(?<month>\d{2})-(?<day>\d{2})\s(?<time>\d{2}:\d{2}:\d{2}),(?<place>\w+),(?<temp>-?\d+\.\d+),(?<humidity>\d+)";

            List<WeatherData> weatherList = new List<WeatherData>(); // Skapar listan

            

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
                }
            }

            
            return weatherList; // Returnerar listan så att andra klasser kan använda den
        }
    }
}


