using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(opt => opt.AddDefaultPolicy(builder =>
{
    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // [NOTE] development settings
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.MapGet("/host-info", (IConfiguration configuration) =>
{
    var currentDir = Directory.GetCurrentDirectory();
    var path = Path.GetFullPath(configuration["HostInfoFile"] ?? throw new Exception("Not found"));
    return Results.File(
        path: path,
        contentType: "text/plain",
        fileDownloadName: "host.txt");
})
.WithName("GetHostInfo")
.WithOpenApi();

app.MapPost("/upload-file", async (IFormFile file, IConfiguration configuration) =>
{
    var path = Path.Combine(
        configuration["DataDir"] ?? throw new Exception("Not found"),
        Path.GetFileName(file.FileName));
    using var fileStream = File.OpenWrite(path);
    await file.CopyToAsync(fileStream);
    return Results.Ok();
})
.WithName("UploadFile")
.DisableAntiforgery();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
