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
            Console.WriteLine();
            Console.WriteLine();




            // Hämta väderdata
            List<WeatherData> weatherData = TextToList.ListList();






            // Sortera dagarna baserat på medelluftfuktighet 
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






            
            var table = new Table()
                .BorderColor(Color.DarkOrange3) 
                .AddColumn(new TableColumn("[bold]Date[/]").Centered())
                .AddColumn(new TableColumn("[bold]Average Humidity Outdoors (%)[/]").Centered());

            foreach (var day in sortedDays)
            {
                table.AddRow(day.Date, day.AverageHumidity.ToString("F1"));
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
    }
}


