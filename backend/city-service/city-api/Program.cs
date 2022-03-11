var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDaprClient();
var app = builder.Build();

app.MapGet("/cities", async (DaprClient daprClient) => 
{
    var cities = new []{"Rome", "Milan", "Venice"};
    var result = new List<CityWeatherModel>();

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

app.MapGet("/cities/{city}", async (DaprClient daprClient, string city) => 
{
    var result = new List<CityWeatherModel>();

    // SAME AS: var weather = await httpClient.GetAsync($"http://localhost:{Environment.GetEnvironmentVariable("DAPR_HTTP_PORT")}/v1.0/invoke/"weather-service/method/weather/{city}");
    var weather = await daprClient.InvokeMethodAsync<List<WeatherForecast>>(HttpMethod.Get, "weather-service", $"weather/{city}");
    
    result.Add(new CityWeatherModel
    {
        City = city,
        Weather = weather
    });

    return result;
});

app.MapGet("/probe", () => "Ok");


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