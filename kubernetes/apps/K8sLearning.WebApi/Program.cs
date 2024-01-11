using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddHostedService<StartupTask>()
    .AddSingleton<ReadyHealthCheck>();
builder.Services.AddHealthChecks()
    .AddCheck<ReadyHealthCheck>(nameof(ReadyHealthCheck), tags: new[] { "Ready" })
    .AddCheck("Liveness", check: (token) =>
    {
        bool hasError = builder.Configuration.GetValue<bool>("HasError");
        return hasError ? HealthCheckResult.Unhealthy() : HealthCheckResult.Healthy();
    }, tags: new[] { "Live" });
builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(b => b.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.MapHealthChecks("/healthz/ready", new HealthCheckOptions
{
    Predicate = hc => hc.Tags.Contains("Ready")
});
app.MapHealthChecks("/healthz/live", new HealthCheckOptions
{
    // Predicate = _ => false,
    Predicate = hc => hc.Tags.Contains("Live"),
});

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/", (IConfiguration configuration) =>
{
    return configuration["Version"];
})
.WithName("GetVersion")
.WithOpenApi();

app.MapGet("/weatherforecast", () =>
{
    string hostName = System.Net.Dns.GetHostName();
    var addr = System.Net.Dns.GetHostAddresses(hostName);
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();

    return new
    {
        addr = addr.Select(a => a.ToString()),
        host = hostName,
        data = forecast
    };
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public class ReadyHealthCheck : IHealthCheck
{
    private volatile bool _isReady;

    public bool Ready
    {
        get => _isReady;
        set => _isReady = value;
    }

    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        if (Ready)
            return Task.FromResult(HealthCheckResult.Healthy("The startup task has completed."));
        return Task.FromResult(HealthCheckResult.Unhealthy("That startup task is still running."));
    }
}

public class StartupTask : BackgroundService
{
    private readonly ReadyHealthCheck _healthCheck;

    public StartupTask(ReadyHealthCheck healthCheck)
        => _healthCheck = healthCheck;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Simulate the effect of a long-running task.
        await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
        _healthCheck.Ready = true;
    }
}