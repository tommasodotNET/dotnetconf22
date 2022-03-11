var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/weather/{city}", (string city) => 
{
    var Summaries = new []{ "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"};
    var rng = new Random();

    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
    {
        Date = DateTime.Now.AddDays(index),
        TemperatureC = rng.Next(-20, 55),
        Summary = Summaries[rng.Next(Summaries.Length)]
    })
    .ToArray();
});

app.Run();

internal class WeatherForecast
{
    [JsonProperty("date")]
    public DateTime Date { get; set; }

    [JsonProperty("temperatureC")]
    public int TemperatureC { get; set; }
    
    [JsonProperty("summary")]
    public string Summary { get; set; }
}