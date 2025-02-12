using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using WeatherApp.MainMenu;
using WeatherApp.Models;
using WeatherApp;
using WeatherApp.OutdoorMenu;

namespace WeatherApp.IndoorMenu
{
    internal class MoldRisk
    {
        public static void SortByMoldRisk()
        {
            Console.Clear();
            MainMenus.ShowHeader();

            var weatherData = TextToList.ListList(); // Hämta väderdata




            // Filtrera endast inomhusdata 
            var moldRiskAverage = weatherData
                .Where(w => w.Location.Equals("inne", StringComparison.OrdinalIgnoreCase)) 
                .GroupBy(w => new { w.Year, w.Month, w.Day })
                .Select(g => new
                {
                    Date = $"{g.Key.Year}-{g.Key.Month}-{g.Key.Day}",
                    AverageMoldRisk = g.Average(x => CalculateMoldRisk(x.Temp, x.Humidity)) 
                })
                .OrderBy(x => x.AverageMoldRisk) 
                .ToList();





            
            var table = new Table()
                .BorderColor(Color.DarkOrange3) // 🔹 Samma färg som DriestHumid
                .AddColumn(new TableColumn("[bold]Date[/]").Centered())
                .AddColumn(new TableColumn("[bold]Mold Risk Indoors (%)[/]").Centered());

            foreach (var entry in moldRiskAverage)
            {
                table.AddRow(entry.Date, $"{entry.AverageMoldRisk:F1}%");
            }




            
            AnsiConsole.Write(new Padder(table, new Padding(58, 0, 0, 0)));

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




        // Metod för att beräkna mögelrisk
        private static double CalculateMoldRisk(double temperature, double humidity)
        {
            if (temperature < 0 || temperature > 35) return 0;
            if (humidity < 60) return 0;

            double tempRisk = (temperature - 0) / 30;
            tempRisk = Math.Max(0, Math.Min(1, tempRisk));

            double humidityRisk = (humidity - 60) / 40;
            humidityRisk = Math.Max(0, Math.Min(1, humidityRisk));

            double moldRisk = (tempRisk + humidityRisk) / 2;

            return Math.Round(moldRisk * 100, 2);
        }
    }
}

