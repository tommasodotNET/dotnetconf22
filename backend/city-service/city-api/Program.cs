var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDaprClient();

var app = builder.Build();

var cities = new []{"Rome", "Milan", "Venice"};

app.MapGet("/cities", async (DaprClient daprClient) => 
{
    var result = new List<CityWeather>();

    foreach(var city in cities)
    {
        // SAME AS: var weather = await httpClient.GetAsync($"http://localhost:{Environment.GetEnvironmentVariable("DAPR_HTTP_PORT")}/v1.0/invoke/weather-api/method/weather/{city}");
        var weather = await daprClient.InvokeMethodAsync<List<WeatherForecast>>(HttpMethod.Get, "weather-api", $"weather/{city}");
        
        result.Add(new CityWeather
        {
            City = city,
            Weather = weather
        });
    }

    return Results.Ok(result);
});

app.MapGet("/cities/{city}", async (DaprClient daprClient, string city) => 
{
    if(!cities.Contains(city))
        return Results.NotFound();

    var result = new List<CityWeather>();

    // SAME AS: var weather = await httpClient.GetAsync($"http://localhost:{Environment.GetEnvironmentVariable("DAPR_HTTP_PORT")}/v1.0/invoke/weather-api/method/weather/{city}");
    var weather = await daprClient.InvokeMethodAsync<List<WeatherForecast>>(HttpMethod.Get, "weather-api", $"weather/{city}");
    
    return Results.Ok(new CityWeather
    {
        City = city,
        Weather = weather
    });
});

app.MapGet("/probe", () => "Ok");

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials()); // allow credentials

app.Run();

internal class CityWeather
{
    [JsonProperty("city")]
    public string? City { get; set; }

    [JsonProperty("weather")]
    public List<WeatherForecast>? Weather { get; set; }
}

internal class WeatherForecast
{
    [JsonProperty("date")]
    public DateTime Date { get; set; }

    [JsonProperty("temperatureC")]
    public int TemperatureC { get; set; }
    
    [JsonProperty("summary")]
    public string? Summary { get; set; }
}