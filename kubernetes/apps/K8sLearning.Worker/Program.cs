using K8sLearning.Worker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddSingleton(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var http = new HttpClient
    {
        BaseAddress = new Uri(config["WebApiUrl"])
    };
    return http;
});
var host = builder.Build();
host.Run();
