using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Windows.Input;
//using ThreadNetwork;
using WeatherApp.Models;

namespace WeatherApp
{
    public partial class MainPage : ContentPage
    {
        private int _temp;

        public int Temp
        {
            get { return _temp; }
            set
            {
                _temp = value;
                OnPropertyChanged();
            }
        }

        private double _wind;
        public double Wind
        {
            get { return _wind; }
            set
            {
                _wind = value;
                OnPropertyChanged();
            }
        }

        private int _humidity;
        public int Humidity
        {
            get { return _humidity; }
            set
            {
                _humidity = value;
                OnPropertyChanged();
            }
        }

        private int _pressure;

        public int Pressure
        {
            get { return _pressure; }
            set
            {
                _pressure = value;
                OnPropertyChanged();
            }
        }

        private string _country;
        public string Country
        {
            get { return _country; }
            set
            {
                _country = value;
                OnPropertyChanged();
            }
        }

        private int _clouds;
        public int Clouds
        {
            get { return _clouds; }
            set
            {
                _clouds = value;
                OnPropertyChanged();
            }
        }

        private string _weatherdescription;
        public string WeatherDescription
        {
            get { return _weatherdescription; }
            set
            {
                _weatherdescription = value;
                OnPropertyChanged();
            }
        }

        private int _dateAndTime;
        public int DateAndTime 
        { get { return _dateAndTime; }
          set 
            { 
                _dateAndTime = value;
                OnPropertyChanged();
            }
        }

        private int _sunrise;

        public int Sunrise
        {
            get { return _sunrise; }
            set 
            { 
                _sunrise = value;
                OnPropertyChanged();
            }
        }

        private int _sunset;

        public int Sunset
        {
            get { return _sunset; }
            set
            {
                _sunset = value;
                OnPropertyChanged();
            }
        }

        private long dateModified;
        public long DateModified
        {
            get => dateModified;
            set
            {
                if (dateModified.Equals(value)) return;
                dateModified = value;
                OnPropertyChanged();
            }
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

        public async void GetLatestWeather(object parameters)
        {
            DateAndTimeClass currenttime = new DateAndTimeClass();

            Location location = await _gpsmodule.GetCurrentLocation();
            double lat = location.Latitude;
            double lng = location.Longitude;

            string appid = "c40b6d8a0180a6a744694f3b0f0a0771";
            string response = await _client.GetStringAsync(new Uri($"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lng}&appid={appid}&units=metric"));

            WeatherForecast currentweather = JsonConvert.DeserializeObject<WeatherForecast>(response);



            if (currentweather != null)
            {
                Temp = (int)Math.Round(currentweather.main.temp);
                Wind = currentweather.wind.speed;
                Sunrise = currentweather.sys.sunrise;
                Sunset = currentweather.sys.sunset;
                DateModified = currentweather.dt;
                Humidity = currentweather.main.humidity;
                Pressure = currentweather.main.pressure;
                Country = currentweather.sys.country;
                Clouds = currentweather.clouds.all;

                if (currentweather.weather.Count > 0)
                {
                    WeatherDescription = currentweather.weather[0].description;
                }

            }

        }

    }

}
