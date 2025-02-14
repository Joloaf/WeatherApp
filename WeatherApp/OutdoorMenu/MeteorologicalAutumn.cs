using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using WeatherApp.IndoorMenu;
using WeatherApp.MainMenu;
using WeatherApp.Models;

namespace WeatherApp.OutdoorMenu
{
    internal class MeteorologicalAutumn
    {
        public static void CheckAutumn()
        {
            Console.Clear();
            MainMenus.ShowHeader();
            Console.WriteLine();

            // Hämta väderdata
            List<WeatherData> weatherData = TextToList.ListList();

            // Beräkna datumet för meteorologisk höst
            string autumnStartDate = TextToList.CalculateSeasonStartDate(weatherData, 10);

            // Skapa tabellen med Spectre.Console
            var table = new Table()
                .BorderColor(Color.DarkOrange3)
                .AddColumn(new TableColumn("[bold]Säsong[/]").Centered())
                .AddColumn(new TableColumn("[bold]Startdatum[/]").Centered());

            table.AddRow("Meteorologisk Höst", autumnStartDate ?? "Ingen höst registrerad");

            // Visa tabellen
            AnsiConsole.Write(new Padder(table, new Padding(62, 0, 0, 0)));

            // Menyval
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

