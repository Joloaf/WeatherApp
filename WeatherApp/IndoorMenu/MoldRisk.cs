using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.MainMenu;
using WeatherApp.OutdoorMenu;

namespace WeatherApp.IndoorMenu
{
    internal class MoldRisk
    {

        public static void SortByMoldRisk()
        {
            Console.Clear();

            MainMenus.ShowHeader();

            AnsiConsole.Markup("[bold green]Sortering: Minst till störst risk av mögel[/]\n");













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
