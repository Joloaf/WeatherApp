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
        // Delegate to aggregate a group of WeatherData
        public delegate double Aggregator(IEnumerable<WeatherData> groupData);

        public static void SortByDriestToMostHumid()
        {
            Console.Clear();
            MainMenus.ShowHeader();
            Console.WriteLine();
            Console.WriteLine();

            List<WeatherData> weatherData = TextToList.ListList();

            // Define an aggregator for average humidity
            Aggregator humidityAggregator = group => group.Average(x => x.Humidity);

            // Group and sort using the aggregator delegate
            var sortedDays = GroupAndSort(weatherData, humidityAggregator);

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

        // Generic method to group data by date and sort based on the aggregated value.
        public static List<WeatherData> GroupAndSort(List<WeatherData> weatherData, Aggregator aggregator)
        {
            return weatherData
                .Where(w => w.Location.Equals("ute", StringComparison.OrdinalIgnoreCase))
                .GroupBy(w => new { w.Year, w.Month, w.Day })
                .Select(g => new WeatherData
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Day = g.Key.Day,
                    Humidity = aggregator(g)
                })
                .OrderBy(x => x.Humidity)
                .ToList();
        }
    }
}
