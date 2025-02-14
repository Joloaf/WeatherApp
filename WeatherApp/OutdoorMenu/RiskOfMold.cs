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
    internal class RiskOfMold
    {
        public static void SortByMoldRisk()
        {
            Console.Clear();
            MainMenus.ShowHeader();

            var weatherData = TextToList.ListList();

            // Filtrera endast utomhusdata och beräkna mögelrisk
            var moldRiskAverage = weatherData
                .Where(w => w.Location.Equals("ute", StringComparison.OrdinalIgnoreCase))
                .GroupBy(w => new { w.Year, w.Month, w.Day })
                .Select(g => new
                {
                    Date = $"{g.Key.Year}-{g.Key.Month}-{g.Key.Day}",
                    AverageMoldRisk = g.Average(x => x.MoldRisk)
                })
                .OrderBy(x => x.AverageMoldRisk)
                .ToList();

            // Skapa tabell
            var table = new Table()
                .BorderColor(Color.DarkOrange3)
                .AddColumn(new TableColumn("[bold]Date[/]").Centered())
                .AddColumn(new TableColumn("[bold]Mold Risk Outdoors (%)[/]").Centered());

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
        public static double CalculateMoldRisk(double temperature, double humidity)
        {
            if (temperature < -5 || temperature > 40) return 0;
            if (humidity < 50) return 0;

            double tempRisk = (temperature + 5) / 45;
            tempRisk = Math.Max(0, Math.Min(1, tempRisk));

            double humidityRisk = (humidity - 50) / 50;
            humidityRisk = Math.Max(0, Math.Min(1, humidityRisk));

            double moldRisk = Math.Sqrt(tempRisk * humidityRisk) * 100;

            return Math.Round(moldRisk, 2);
        }
    }
}

