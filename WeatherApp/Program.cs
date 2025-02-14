using WeatherApp.MainMenu;
using WeatherApp.OutdoorMenu;



namespace WeatherApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var weatherData = TextToList.ListList();
            TextToList.SaveMonthlyAveragesToFile(weatherData, "monthly_averages.txt");
            MainMenus.ShowMainMenu();
        }
    }
}
