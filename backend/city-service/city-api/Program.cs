var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/cities", async () => 
{
    var cities = new []{"Rome", "Milan", "Venice"};
    var result = new List<CityWeatherModel>();
    var daprClient = new DaprClientBuilder().Build();

    foreach(var city in cities)
    {
        // SAME AS: var weather = await httpClient.GetAsync($"http://localhost:{Environment.GetEnvironmentVariable("DAPR_HTTP_PORT")}/v1.0/invoke/"weather-service/method/weather/{city}");
        var weather = await daprClient.InvokeMethodAsync<List<WeatherForecast>>(HttpMethod.Get, "weather-service", $"weather/{city}");
        
        result.Add(new CityWeatherModel
        {
            City = city,
            Weather = weather
        });
    }

    return result;
});


app.Run();

internal class CityWeatherModel
{
    [JsonProperty("city")]
    public string City { get; set; }

    [JsonProperty("weather")]
    public List<WeatherForecast> Weather { get; set; }
}

internal class WeatherForecast
{
    [JsonProperty("date")]
    public DateTime Date { get; set; }

    [JsonProperty("temperatureC")]
    public int TemperatureC { get; set; }
    
    [JsonProperty("summary")]
    public string Summary { get; set; }
}