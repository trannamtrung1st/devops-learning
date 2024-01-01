using DockerLearning.MyWebApi.Entities;
using DockerLearning.MyWebApi.Models;
using DockerLearning.MyWebApi.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(opt => opt.AddDefaultPolicy(builder =>
{
    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
}));
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString(nameof(AppDbContext))));

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

app.MapGet("/resources", async ([FromServices] AppDbContext dbContext) =>
{
    var models = await dbContext.Resources.Select(e => new ResourceModel
    {
        Id = e.Id,
        Name = e.Name
    }).ToArrayAsync();

    return Results.Ok(models);
})
.WithName("GetAllResources")
.WithOpenApi();

app.MapPost("/resources", async ([FromBody] ResourceModel model, [FromServices] AppDbContext dbContext) =>
{
    var entity = new ResourceEntity
    {
        Name = model.Name
    };
    await dbContext.AddAsync(entity);
    await dbContext.SaveChangesAsync();

    return Results.Ok();
})
.WithName("CreateResource")
.WithOpenApi();

await MigrateDatabase(app);

app.Run();

static async Task MigrateDatabase(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();
}

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}