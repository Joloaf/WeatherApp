﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using WeatherApp.Models;
using WeatherApp.OutdoorMenu;

namespace WeatherApp
{
    internal class TextToList
    {
        public static List<WeatherData> ListList()
        {
            string filePath = @"..\..\..\Files\tempData_medFel.txt";
            string pattern = @"(?<year>2016)-(?<month>0[6-9]|1[0-2])-(?<day>0[1-9]|[12][0-9]|3[01])\s(?<time>\d{2}:\d{2}:\d{2}),(?<place>Inne|Ute),(?<temp>-?(20|1[0-9]|[0-9]|[1-3][0-9]|40)\.\d+),(?<humidity>100|[1-9][0-9]|0?[0-9])";
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
                        double moldRisk = RiskOfMold.CalculateMoldRisk(temp, humidity);

                        weatherList.Add(new WeatherData
                        {
                            Year = match.Groups["year"].Value,
                            Month = match.Groups["month"].Value,
                            Day = match.Groups["day"].Value,
                            Time = match.Groups["time"].Value,
                            Location = match.Groups["place"].Value,
                            Temp = temp,
                            Humidity = humidity,
                            MoldRisk = moldRisk
                        });
                    }
                    else
                    {
                        Console.WriteLine($"Fel i raden: {line}");
                    }
                }
            }

            // Spara månatliga medelvärden till fil
            SaveMonthlyAveragesToFile(weatherList, @"..\..\..\Files\monthlyAverages.txt");

            // Beräkna och visa datum för höstens och vinterns start
            string autumnStartDate = CalculateSeasonStartDate(weatherList, 10);
            string winterStartDate = CalculateSeasonStartDate(weatherList, 0);

            using (StreamWriter writer = new StreamWriter(@"..\..\..\Files\monthlyAverages.txt", true))
            {
                writer.WriteLine();
                writer.WriteLine("Säsongsstart:");
                SaveAndDisplaySeasonStartDate(writer, "Hösten", autumnStartDate);
                SaveAndDisplaySeasonStartDate(writer, "Vintern", winterStartDate);

                // Add mold risk calculation formula
                writer.WriteLine();
                writer.WriteLine(@"Om temperaturen är utanför intervallet -5°C till 40°C eller luftfuktigheten är under 50%, returnera 0; 
annars beräkna MoldRisk = √(((Temperatur + 5) / 45) * ((Luftfuktighet - 50) / 50)) * 100, 
och returnera MoldRisk avrundad till två decimaler.");
            }

            // Skriv ut resultaten - används för att testa att allt fungerar
            //SaveAndDisplaySeasonStartDate(Console.Out, "Hösten", autumnStartDate);
            //SaveAndDisplaySeasonStartDate(Console.Out, "Vintern", winterStartDate);

            return weatherList;
        }

        // Funktion för att spara månatliga medelvärden till fil
        public static void SaveMonthlyAveragesToFile(List<WeatherData> data, string filePath)
        {
            var monthlyAverages = data.GroupBy(w => new { w.Year, w.Month, w.Location })
                .Select(group => new
                {
                    Year = group.Key.Year,
                    Month = group.Key.Month,
                    Location = group.Key.Location,
                    AvgTemp = group.Average(w => w.Temp),
                    AvgHumidity = group.Average(w => w.Humidity),
                    AvgMoldRisk = group.Average(w => w.MoldRisk)
                })
                .OrderBy(item => item.Year)
                .ThenBy(item => item.Month)
                .ThenBy(item => item.Location == "Ute" ? 0 : 1);




            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine("Utomhus:");
                    foreach (var item in monthlyAverages.Where(item => item.Location == "Ute"))
                    {
                        writer.WriteLine($"{item.Year} - {GetMonthName(item.Month),-10} Medeltemperatur: {item.AvgTemp,-8:F1} Luftfuktighet: {item.AvgHumidity,-8:F1} Mögelrisk: {item.AvgMoldRisk:F1}%");
                    }

                    writer.WriteLine();

                    writer.WriteLine("Inomhus:");
                    foreach (var item in monthlyAverages.Where(item => item.Location == "Inne"))
                    {
                        writer.WriteLine($"{item.Year} - {GetMonthName(item.Month),-10} Medeltemperatur: {item.AvgTemp,-8:F1} Luftfuktighet: {item.AvgHumidity,-8:F1} Mögelrisk: {item.AvgMoldRisk:F1}%");
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Fel vid skrivning till filen {filePath}: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ett oväntat fel inträffade vid filskrivning: {ex.Message}");
            }




        }

        // Funktion för att spara och visa datum för en säsongs start
        private static void SaveAndDisplaySeasonStartDate(TextWriter writer, string seasonName, string startDate)
        {
            if (!string.IsNullOrEmpty(startDate))
            {
                writer.WriteLine($"{seasonName} börjar den: {startDate}");
            }
            else
            {
                writer.WriteLine($"{seasonName} började inte under den angivna perioden.");
            }
        }

        // Funktion för att beräkna datum för säsongens start
        public static string CalculateSeasonStartDate(List<WeatherData> data, double threshold)
        {
            var dailyAverages = CalculateDailyAverages(data);

            // Debugging: Print daily average temperatures
            //foreach (var day in dailyAverages)
            //{
            //    Console.WriteLine($"Datum: {day.Date} - Medeltemperatur: {day.AvgTemp:F1}°C");
            //}

            for (int i = 0; i <= dailyAverages.Count - 5; i++)
            {
                if (dailyAverages.Skip(i).Take(5).All(d => d.AvgTemp < threshold))
                {
                    return dailyAverages[i].Date;
                }
            }

            return null;
        }

        // Funktion för att beräkna dagliga medelvärden
        private static List<(string Date, double AvgTemp)> CalculateDailyAverages(List<WeatherData> data)
        {
            return data.Where(w => w.Location == "Ute")
                .GroupBy(w => new { w.Year, w.Month, w.Day })
                .Select(group => new
                {
                    Date = $"{group.Key.Year}-{group.Key.Month}-{group.Key.Day}",
                    AvgTemp = group.Average(w => w.Temp)
                })
                .OrderBy(item => item.Date)
                .Select(item => (item.Date, item.AvgTemp))
                .ToList();
        }

        private static string GetMonthName(string month)
        {
            return month switch
            {
                "01" => "Januari",
                "02" => "Februari",
                "03" => "Mars",
                "04" => "April",
                "05" => "Maj",
                "06" => "Juni",
                "07" => "Juli",
                "08" => "Augusti",
                "09" => "September",
                "10" => "Oktober",
                "11" => "November",
                "12" => "December",
                _ => month
            };
        }
    }
}


