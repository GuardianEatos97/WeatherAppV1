using Newtonsoft.Json;
using System.Windows.Input;
//using ThreadNetwork;
using WeatherApp.Models;

namespace WeatherApp
{
    public partial class MainPage : ContentPage
    {
        private double _temp;

        public double Temp
        {
            get { return _temp; }
            set { _temp = value; OnPropertyChanged(); }
        }


        private GPSModule _gpsmodule;
        
       
        private string _currentWeather;

       /* public string CurrentWeather
        {
            get { return _currentWeather; }
            set
            {
                _currentWeather = value;
                OnPropertyChanged();
            }
        }*/

        

        public MainPage()
        {
            InitializeComponent();
            ShowWeatherCommand = new Command(GetLatestWeather);
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            _gpsmodule = new GPSModule();
            BindingContext = this;

        }

        public ICommand ShowWeatherCommand { get; set; }

        private HttpClient _client;

        private void OnCounterClicked(object sender, EventArgs e)
        {
            
        }
        public async void GetLatestWeather(object parameters)
        {

            Location location = await _gpsmodule.GetCurrentLocation();
            double lat = location.Latitude;
            double lng = location.Longitude;

            string appid = "c40b6d8a0180a6a744694f3b0f0a0771";
            string response = await _client.GetStringAsync(new Uri($"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lng}&appid={appid}&units=metric"));

            WeatherForecast currentweather = JsonConvert.DeserializeObject<WeatherForecast>(response);

            if (currentweather != null)
            {
               Temp = currentweather.main.temp;
            }

        }

    }

}
