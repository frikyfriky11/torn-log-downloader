using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TornLogDownloader.Entities;
using ILogger = Serilog.ILogger;

namespace TornLogDownloader;

public class Worker : BackgroundService
{
  private readonly TornApiClient _tornApiClient;
  private readonly ApplicationDbContext _dbContext;
  private readonly IHostApplicationLifetime _hostApplicationLifetime;
  private readonly ILogger _logger;

  public Worker(ILogger logger, ApplicationDbContext dbContext, TornApiClient tornApiClient, IHostApplicationLifetime hostApplicationLifetime)
  {
    _dbContext = dbContext;
    _logger = logger.ForContext<Worker>();
    _tornApiClient = tornApiClient;
    _hostApplicationLifetime = hostApplicationLifetime;
  }

  protected override async Task ExecuteAsync(CancellationToken cancellationToken)
  {
    await _dbContext.Database.MigrateAsync(cancellationToken);

    EntityEntry<Run> run = await _dbContext.Runs.AddAsync(new Run
    {
      Started = DateTimeOffset.Now,
    }, cancellationToken);

    await _dbContext.SaveChangesAsync(cancellationToken);

    try
    {
      long? from = await _dbContext
        .RawLogs
        .OrderByDescending(x => x.Timestamp)
        .Select(x => (long?)(x.Timestamp + 1))
        .FirstOrDefaultAsync(cancellationToken);

      long? to = null;

      List<RawLog> apiRawLogs;

      do
      {
        EntityEntry<RunApiCall> runApiCall = await _dbContext.RunApiCalls.AddAsync(new RunApiCall
        {
          Run = run.Entity,
          Started = DateTimeOffset.Now,
        }, cancellationToken);

        apiRawLogs = await _tornApiClient.GetLogsAsync(from, to, cancellationToken);

        runApiCall.Entity.Ended = DateTimeOffset.Now;
        runApiCall.Entity.RawLogs = apiRawLogs;

        await _dbContext.SaveChangesAsync(cancellationToken);

        Thread.Sleep(700);

        to = apiRawLogs
          .OrderBy(x => x.Timestamp)
          .Select(x => (long?)x.Timestamp)
          .FirstOrDefault();
      } while (apiRawLogs.Count != 0);

      run.Entity.Ended = DateTimeOffset.Now;
      run.Entity.IsSuccessful = true;
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "Something wrong happened while downloading the data");
      run.Entity.Ended = DateTimeOffset.Now;
    }

    await _dbContext.SaveChangesAsync(cancellationToken);

    _hostApplicationLifetime.StopApplication();
  }
}







