namespace K8sLearning.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly HttpClient _httpClient;

    public Worker(ILogger<Worker> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                var response = await _httpClient.GetStringAsync("/");
                _logger.LogInformation("{time} - API response: {response}", DateTimeOffset.Now, response);
            }

            await Task.Delay(5000, stoppingToken);
        }
    }
}
