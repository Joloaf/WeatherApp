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

            var weatherData = TextToList.ListList(); // Hämta väderdata

            // Filtrera endast utomhusdata och beräkna mögelrisk
            var moldRiskAverage = weatherData
                .Where(w => w.Location.Equals("ute", StringComparison.OrdinalIgnoreCase)) // Endast utomhusdata
                .GroupBy(w => new { w.Year, w.Month, w.Day })
                .Select(g => new
                {
                    Date = $"{g.Key.Year}-{g.Key.Month}-{g.Key.Day}",
                    AverageMoldRisk = g.Average(x => CalculateMoldRisk(x.Temp, x.Humidity)) // Beräkna mögelrisk
                })
                .OrderBy(x => x.AverageMoldRisk) // Sortera från minst till störst mögelrisk
                .ToList();

            // Skapa tabell med Spectre.Console
            var table = new Table()
                .BorderColor(Color.DarkOrange3) // 🔹 Samma färg som DriestHumid
                .AddColumn(new TableColumn("[bold]Date[/]").Centered())
                .AddColumn(new TableColumn("[bold]Mold Risk Outdoors (%)[/]").Centered());

            foreach (var entry in moldRiskAverage)
            {
                table.AddRow(entry.Date, $"{entry.AverageMoldRisk:F1}%");
            }

            // 🔹 Flytta tabellen till samma position som i DriestHumid
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

        // Metod för att beräkna mögelrisk (förbättrad version)
        private static double CalculateMoldRisk(double temperature, double humidity)
        {
            if (temperature < -5 || temperature > 40) return 0; // Utökat temperaturintervall
            if (humidity < 50) return 0; // Lägre luftfuktighetsgräns

            // Normalisering av temperatur (0-1 skala)
            double tempRisk = (temperature + 5) / 45; // (-5°C → 0, 40°C → 1)
            tempRisk = Math.Max(0, Math.Min(1, tempRisk));

            // Normalisering av luftfuktighet (0-1 skala)
            double humidityRisk = (humidity - 50) / 50; // (50% → 0, 100% → 1)
            humidityRisk = Math.Max(0, Math.Min(1, humidityRisk));

            // Kombinera temperatur- och fuktighetsrisk
            double moldRisk = Math.Sqrt(tempRisk * humidityRisk) * 100; // Mer realistisk beräkning

            return Math.Round(moldRisk, 2); // Returnera procentvärde
        }
    }
}

