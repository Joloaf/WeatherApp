using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using WeatherApp.IndoorMenu;
using WeatherApp.MainMenu;
using WeatherApp.Models;
using WeatherApp;

namespace WeatherApp.OutdoorMenu
{
    internal class DriestToMostHumid
    {
        public static void SortByDriestToMostHumid()
        {
            Console.Clear();
            MainMenus.ShowHeader();

            // 🔹 Hämta väderdata
            List<WeatherData> weatherData = TextToList.ListList();

            // 🔹 Sortera dagarna baserat på medelluftfuktighet (endast utomhus)
            var sortedDays = weatherData
                .Where(w => w.Location.Equals("ute", StringComparison.OrdinalIgnoreCase))
                .GroupBy(w => $"{w.Year}-{w.Month}-{w.Day}")
                .Select(g => new
                {
                    Date = g.Key,
                    AverageHumidity = g.Average(x => x.Humidity)
                })
                .OrderBy(x => x.AverageHumidity)
                .ToList();

            // 🔹 Skapa tabellen utan box
            var table = new Table()
                .AddColumn("Datum")
                .AddColumn("Medelfuktighet (%)");

            foreach (var day in sortedDays)
                table.AddRow(day.Date.PadLeft(15), day.AverageHumidity.ToString("F1").PadLeft(15)); // 🔹 Flyttar tabelltexten åt höger

            // 🔹 Flytta tabellen åt höger genom att ändra Console.Write
            Console.WriteLine("\n".PadLeft(10)); // 🔹 Extra mellanrum innan tabellen
            AnsiConsole.Write(table);

            // 🔹 Menyval för att navigera tillbaka
            var key = Console.ReadKey(true);
            switch (key.Key)
            {
                case ConsoleKey.I:
                    IndoorMenus.ShowIndoorMenus();
                    break;

                case ConsoleKey.U:
                    OutdoorMenus.ShowOutdoorMenu();
                    break;

                case ConsoleKey.Q:
                    MainMenus.ShowMainMenu();
                    return;

                default:
                    AnsiConsole.Markup("[bold red]\nFelaktigt val, försök igen![/]\n");
                    break;
            }
        }
    }
}

