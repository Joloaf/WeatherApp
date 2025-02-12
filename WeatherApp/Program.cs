namespace WeatherApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var (weatherAverages, monthlyAverage) = TextToList.ListList();

            PrintWeatherData.PrintDailyAverages(weatherAverages);
            PrintWeatherData.PrintMonthlyAverages(monthlyAverage);
            SaveToFile.SaveMonthlyAverages(monthlyAverage);
        }
    }
}