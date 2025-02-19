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
        // Delegat för att beräkna ett aggregatvärde från en grupp av WeatherData
        public delegate double Aggregator(IEnumerable<WeatherData> groupData);

        public static void SortByDriestToMostHumid()
        {
            Console.Clear();
            MainMenus.ShowHeader();
            Console.WriteLine();
            Console.WriteLine();

            List<WeatherData> weatherData = TextToList.ListList();

            // Definiera en aggregator för att beräkna medelluftfuktighet
            Aggregator humidityAggregator = group => group.Average(x => x.Humidity);

            // Gruppera och sortera med hjälp av delegaten för aggregation
            var sortedDays = GroupAndSort(weatherData, humidityAggregator);

            var table = new Table()
                .BorderColor(Color.DarkOrange3)
                .AddColumn(new TableColumn("[bold]Datum[/]").Centered())
                .AddColumn(new TableColumn("[bold]Genomsnittlig luftfuktighet utomhus (%)[/]").Centered());

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

        // Generisk metod för att gruppera data per datum och sortera baserat på det aggregerade värdet.
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
