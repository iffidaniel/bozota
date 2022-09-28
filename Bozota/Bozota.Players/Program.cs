var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddScoped<SampleService>();

var app = builder.Build();

app.Logger.LogInformation("The app started");

var message = app.Configuration["HelloKey"] ?? "Hello";

app.MapGet("/", () => message);

/*using (var scope = app.Services.CreateScope())
{
    var sampleService = scope.ServiceProvider.GetRequiredService<SampleService>();
    sampleService.DoSomething();
}*/

app.Run();