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
        // Definiera delegaten för att sortera väderdata
        public delegate List<WeatherData> SortWeatherData(List<WeatherData> weatherData);

        public static void SortByDriestToMostHumid()
        {
            Console.Clear();
            MainMenus.ShowHeader();
            Console.WriteLine();
            Console.WriteLine();

            List<WeatherData> weatherData = TextToList.ListList();

            SortWeatherData sortFunction = SortByHumidity; // Deligaten används här 

            var sortedDays = sortFunction(weatherData); // Deligaten anroppas 

            var table = new Table()
                .BorderColor(Color.DarkOrange3)
                .AddColumn(new TableColumn("[bold]Date[/]").Centered())
                .AddColumn(new TableColumn("[bold]Average Humidity Outdoors (%)[/]").Centered());

            foreach (var day in sortedDays)
            {
                string date = $"{day.Year}-{day.Month}-{day.Day}";
                table.AddRow(date, day.Humidity.ToString("F1"));
            }

            AnsiConsole.Write(new Padder(table, new Padding(55, 0, 0, 0)));

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

        public static List<WeatherData> SortByHumidity(List<WeatherData> weatherData)
        {
            return weatherData
                .Where(w => w.Location.Equals("ute", StringComparison.OrdinalIgnoreCase))
                .GroupBy(w => $"{w.Year}-{w.Month}-{w.Day}")
                .Select(g => new WeatherData
                {
                    Year = g.Key.Split('-')[0],
                    Month = g.Key.Split('-')[1],
                    Day = g.Key.Split('-')[2],
                    Humidity = g.Average(x => x.Humidity)
                })
                .OrderBy(x => x.Humidity)
                .ToList();
        }
    }
}
