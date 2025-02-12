using Spectre.Console;
using System;
using WeatherApp.MainMenu; 

namespace WeatherApp.OutdoorMenu
{
    internal class OutdoorMenus
    {
        public static void ShowOutdoorMenu()
        {
            while (true)
            {
                Console.Clear();

                // Visa rubriken
                MainMenus.ShowHeader();

                



                // Skapa tabellen
                var table = new Table();
                table.Border = TableBorder.Rounded;
                table.AddColumn("[yellow]Nr[/]");
                table.AddColumn("[blue]Options for outdoor temperatures[/]");

                table.AddRow("1", "Average temperature and humidity per day for selected date");
                table.AddRow("2", "Sort: Warmest to coldest day by average temperature per day");
                table.AddRow("3", "Sort: Driest to most humid day by average humidity per day");
                table.AddRow("4", "Sort: Lowest to highest risk of mold");
                table.AddRow("5", "Date for meteorological autumn");
                table.AddRow("6", "Date for meteorological winter");


                AnsiConsole.Write(new Padder(table, new Padding(45, 2, 0, 0))); 

                





                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.D1:
                        AverageTemperatureHumidity.ShowAverageTempHumidity();
                        break;
                    case ConsoleKey.D2:
                        WarmestToColdest.SortByWarmestToColdest();
                        break;
                    case ConsoleKey.D3:
                        DriestToMostHumid.SortByDriestToMostHumid();
                        break;
                    case ConsoleKey.D4:
                        RiskOfMold.SortByMoldRisk();
                        break;
                    case ConsoleKey.D5:
                        MeteorologicalAutumn.CheckAutumn();
                        break;
                    case ConsoleKey.D6:
                        MeteorologicalWinter.CheckWinter();
                        break;
                    case ConsoleKey.I: 
                        IndoorMenu.IndoorMenus.ShowIndoorMenus();
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

