using KlmGomsEstimator.Presentation.CLI.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var services = new ServiceCollection();
services
    .AddSingleton<IConfiguration>(configuration)
    .ConfigureOptions()
    .ConfigureServices();

var serviceProvider = services.BuildServiceProvider();

var app = serviceProvider.GetRequiredService<ConsoleApplication>();
app.Run();
