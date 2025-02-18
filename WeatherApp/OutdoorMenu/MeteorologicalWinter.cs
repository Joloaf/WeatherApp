using Spectre.Console;
using System;
using System.Collections.Generic;
using WeatherApp.IndoorMenu;
using WeatherApp.MainMenu;
using WeatherApp.Models;

namespace WeatherApp.OutdoorMenu
{
    internal class MeteorologicalWinter
    {
        public static void CheckWinter()
        {
            Console.Clear();
            MainMenus.ShowHeader();
            Console.WriteLine();

            List<WeatherData> weatherData = TextToList.ListList();

            // Beräkna datumet för meteorologisk vinter
            string winterStartDate = TextToList.CalculateSeasonStartDate(weatherData, 0);

            
            var table = new Table()
                .BorderColor(Color.DarkBlue)
                .AddColumn(new TableColumn("[bold]Säsong[/]").Centered())
                .AddColumn(new TableColumn("[bold]Startdatum[/]").Centered());

            table.AddRow("Meteorologisk Vinter", winterStartDate ?? "Ingen vinter registrerad");

            
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
