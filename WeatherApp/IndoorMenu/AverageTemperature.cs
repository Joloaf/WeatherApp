using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WeatherApp.MainMenu;
using WeatherApp.Models;
using WeatherApp;
using WeatherApp.OutdoorMenu;

namespace WeatherApp.IndoorMenu
{
    internal class AverageTemperature
    {
        public static void SearchTemperatureByDate()
        {
            Console.Clear();
            MainMenus.ShowHeader();

            var weatherData = TextToList.ListList(); // Hämta väderdata
            string inputDate = "";




            while (true)
            {
                ShowInputPanel(inputDate);
                inputDate = ReadUserInput();

                // Hantera menyval
                switch (inputDate.ToUpper())
                {
                    case "I":
                        IndoorMenus.ShowIndoorMenus();
                        return;
                    case "U":
                        OutdoorMenus.ShowOutdoorMenu();
                        return;
                    case "Q":
                        MainMenus.ShowMainMenu();
                        return;
                }




                // Validera inmatningen
                if (!DateTime.TryParseExact(inputDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                {
                    ShowErrorMessage("Invalid format! Use YYYY-MM-DD.");
                    continue;
                }







                // Hitta medeltemperatur för det valda datumet 
                var selectedDateData = weatherData
                    .Where(w => $"{w.Year}-{w.Month}-{w.Day}" == inputDate && w.Location.Equals("inne", StringComparison.OrdinalIgnoreCase))
                    .GroupBy(w => new { w.Year, w.Month, w.Day })
                    .Select(g => new
                    {
                        Date = $"{g.Key.Year}-{g.Key.Month}-{g.Key.Day}",
                        AverageTemperature = g.Average(x => x.Temp)
                    })
                    .FirstOrDefault();





                
                if (selectedDateData == null)
                {
                    ShowErrorMessage("No indoor temperature data found for this date!");
                }
                else
                {
                    ShowResult(selectedDateData);
                    WaitForMenuInput();
                }
            }
        }





        // Metod för att visa input-rutan
        private static void ShowInputPanel(string inputDate)
        {
            Console.Clear();
            MainMenus.ShowHeader();

            var inputPanel = new Panel($"[bold]Enter a date (YYYY-MM-DD) for indoor temperature:[/] [cyan]{inputDate}[/]")
                .BorderColor(Color.Blue)
                .Expand();

            AnsiConsole.Write(new Padder(inputPanel, new Padding(0, 0, 0, 0)));
        }






        // Metod för att läsa inmatning
        private static string ReadUserInput()
        {
            string input = "";
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Enter) return input;
                if (key.Key == ConsoleKey.Backspace && input.Length > 0)
                    input = input.Substring(0, input.Length - 1);
                else if (char.IsDigit(key.KeyChar) || key.KeyChar == '-')
                    input += key.KeyChar;

                ShowInputPanel(input);
            }
        }






        // Metod för att visa resultatet
        private static void ShowResult(dynamic selectedDateData)
        {
            var resultPanel = new Panel($"[bold]Date:[/] {selectedDateData.Date}\n" +
                                        $"[bold]Avg Indoor Temp:[/] {selectedDateData.AverageTemperature:F1}°C")
                .BorderColor(Color.Green)
                .Expand();

            AnsiConsole.Write(new Padder(resultPanel, new Padding(0, 0, 0, 0)));
        }







        // Metod för att visa ett felmeddelande
        private static void ShowErrorMessage(string message)
        {
            var errorPanel = new Panel($"[red]{message}[/]")
                .BorderColor(Color.Red)
                .Expand();

            AnsiConsole.Write(new Padder(errorPanel, new Padding(0, 0, 0, 0)));
            WaitForMenuInput();
        }






        
        private static void WaitForMenuInput()
        {
            while (true)
            {
                var key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.I:
                        IndoorMenus.ShowIndoorMenus();
                        return;
                    case ConsoleKey.U:
                        OutdoorMenus.ShowOutdoorMenu();
                        return;
                    case ConsoleKey.Q:
                        MainMenus.ShowMainMenu();
                        return;
                }
            }
        }
    }
}

