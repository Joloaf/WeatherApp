using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.IndoorMenu;
using WeatherApp.MainMenu;

namespace WeatherApp.OutdoorMenu
{
    internal class WarmestToColdest
    {

        public static void SortByWarmestToColdest()
        {

            Console.Clear();

            MainMenus.ShowHeader();

            AnsiConsole.Markup("[bold green]Sortering av varmast till kallaste dagen enligt medeltemperatur per dag[/]\n");






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
