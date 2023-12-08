using GAMETEQ.Currency.WebApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureServices();
var app = builder.Build();
app.ConfigureApplication();
app.Run();