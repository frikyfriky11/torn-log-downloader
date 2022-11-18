using Microsoft.EntityFrameworkCore;
using RestSharp;
using Serilog;
using Serilog.Events;
using TornLogDownloader;
using TornLogDownloader.Entities;

Log.Logger = new LoggerConfiguration()
  .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
  .Enrich.FromLogContext()
  .WriteTo.Console()
  .CreateBootstrapLogger();

IHost host = Host.CreateDefaultBuilder(args)
  .UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console())
  .ConfigureServices((context, services) =>
  {
    services.AddDbContext<ApplicationDbContext>(options =>
    {
      string? connectionString = context.Configuration.GetConnectionString("MainDb");
      options.UseSqlServer(connectionString);
    }, ServiceLifetime.Singleton);

    services.AddTransient<RestClient>();

    services.AddSingleton<TornApiClient>();

    services.AddHostedService<Worker>();
  })
  .Build();

await host.RunAsync();
