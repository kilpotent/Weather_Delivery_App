using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace Weather_Delivery_App

{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent(); // Initializes the components defined in MainPage.xaml
        }

        // Event handler for the button click
        private async void OnGetWeatherClicked(object sender, EventArgs e)
        {
            string city = "Lamia,GR"; // City to fetch weather for
            string apiKey = "3885e28cd48fb923fdaf2bab60582765"; // Your OpenWeatherMap API key
            string baseUrl = "https://api.openweathermap.org/data/2.5/weather";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = $"{baseUrl}?q={city}&appid={apiKey}&units=metric";
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        var weatherData = JsonConvert.DeserializeObject<WeatherResponse>(jsonResponse);

                        // Update the UI with weather data
                        CityLabel.Text = $"City: {weatherData.Name}";
                        TemperatureLabel.Text = $"Temperature: {Convert.ToInt32(weatherData.Main.Temp)}°C";
                        WeatherDescriptionLabel.Text = $"Weather: {weatherData.Weather[0].Description}";
                    }
                    else
                    {
                        CityLabel.Text = "Error: Unable to fetch data.";
                    }
                }
                catch (Exception ex)
                {
                    CityLabel.Text = $"Error: {ex.Message}";
                }
            }
        }

        // Weather response model classes
        public class WeatherResponse
        {
            public string Name { get; set; }
            public WeatherMain Main { get; set; }
            public Weather[] Weather { get; set; }
        }

        public class WeatherMain
        {
            public double Temp { get; set; }
        }

        public class Weather
        {
            public string Description { get; set; }
        }
    }
}
