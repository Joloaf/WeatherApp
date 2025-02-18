using Spectre.Console;
using System;
using WeatherApp.IndoorMenu;
using WeatherApp.OutdoorMenu;

namespace WeatherApp.MainMenu
{
    internal class MainMenus
    {
        public static void ShowMainMenu()
        {
            while (true)
            {
                Console.Clear();

                
                ShowHeader();

                Console.WriteLine();
                Console.WriteLine();

                // diagram 
                AnsiConsole.Write(new Padder(
                new BarChart()
                   .Width(65)
                   .CenterLabel()
                   .AddItem("Outdoor", 23, Color.Green)
                   .AddItem("Indoor", 21, Color.Blue)
                   .AddItem("Humidity", 40, Color.Yellow),
                new Padding(47, 2, 20, 2)
                ));

                var key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.O:
                        OutdoorMenus.ShowOutdoorMenu();
                        break;
                    case ConsoleKey.I:
                        IndoorMenus.ShowIndoorMenus();
                        break;
                    case ConsoleKey.Q:
                        AnsiConsole.Markup("[bold red]Exiting program...[/]");
                        return;
                    default:
                        AnsiConsole.Markup("[bold red]Invalid selection, try again![/]\n");
                        break;
                }
            }
        }

        
        public static void ShowHeader()
        {
            
            AnsiConsole.Write(
                new FigletText("Weather Data")
                    .Centered()
                    .Color(Color.DarkOrange));

            
            AnsiConsole.Write(new Rule("[White]==============================[/]"));

           
            var grid = new Grid();
            grid.AddColumn();
            grid.AddColumn();
            grid.AddColumn();

            // Skapa tre boxar 
            var outdoorPanel = new Panel("[blue]O - Outdoor[/]").Border(BoxBorder.Rounded).Padding(4, 1, 4, 1).Expand();
            var indoorPanel = new Panel("[green]I - Indoor[/]").Border(BoxBorder.Rounded).Padding(4, 1, 4, 1).Expand();
            var quitPanel = new Panel("[red]Q - Quit[/]").Border(BoxBorder.Rounded).Padding(4, 1, 4, 1).Expand();

            grid.AddRow(outdoorPanel, indoorPanel, quitPanel);
            AnsiConsole.Write(new Padder(grid, new Padding(48, 4, 0, 0)));
        }
    }
}
