var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDaprClient();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
