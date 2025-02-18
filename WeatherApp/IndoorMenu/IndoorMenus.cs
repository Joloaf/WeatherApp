using Spectre.Console;
using System;
using WeatherApp.MainMenu;

namespace WeatherApp.IndoorMenu
{
    internal class IndoorMenus
    {
        public static void ShowIndoorMenus()
        {
            while (true)
            {
                Console.Clear();

                MainMenus.ShowHeader();


                var table = new Table();
                table.Border = TableBorder.Rounded;
                table.AddColumn("[yellow]Nr[/]");
                table.AddColumn("[green]Options for indoor temperature[/]");

                table.AddRow("1", "Average temperature for selected date");
                table.AddRow("2", "Sorting: Warmest to coldest day by average temperature per day");
                table.AddRow("3", "Sorting: Driest to most humid day by average humidity per day");
                table.AddRow("4", "Sorting: Lowest to highest risk of mold");


                AnsiConsole.Write(new Padder(table, new Padding(45, 2, 0, 0))); 

                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.D1:
                        AverageTemperature.SearchTemperatureByDate();
                        break;
                    case ConsoleKey.D2:
                        WarmestColdest.SortByWarmestToColdest();
                        break;
                    case ConsoleKey.D3:
                        DriestHumid.SortByDriestToMostHumid();
                        break;
                    case ConsoleKey.D4:
                        MoldRisk.SortByMoldRisk();
                        break;
                    case ConsoleKey.O: 
                        OutdoorMenu.OutdoorMenus.ShowOutdoorMenu();
                        break;
                    case ConsoleKey.Q:
                        MainMenus.ShowMainMenu();
                        break;
                    case ConsoleKey.D0:
                        return;
                    default:
                        AnsiConsole.Markup("[bold red]\nFelaktigt val, försök igen![/]\n");
                        break;
                }
            }
        }
    }
}


