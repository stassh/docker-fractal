using Fractal.Api.Model;
using Fractal.Api.Service;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Fractal.Api
{
  public class FractalJobWorker : BackgroundService
  {
    private readonly ILogger<FractalJobWorker> logger;
    private readonly IJobSchedulerService jobSchedulerService;
    private readonly IFractalTileSchedulerService fractalTileSchedulerService;

    public FractalJobWorker(ILogger<FractalJobWorker> logger, IJobSchedulerService jobSchedulerService, IFractalTileSchedulerService fractalTileSchedulerService)
    {
      this.logger = logger;
      this.jobSchedulerService = jobSchedulerService;
      this.fractalTileSchedulerService = fractalTileSchedulerService;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      while (!stoppingToken.IsCancellationRequested)
      {
        if (jobSchedulerService != null)
        {
          var message = await jobSchedulerService.GetMessageAsync(stoppingToken);
          logger.LogInformation("Received message {0}", message.AsString);

          await fractalTileSchedulerService.PushMessageAsync(JsonSerializer.Deserialize<FractalComputationMessage>(message.AsString));

          await jobSchedulerService.DeleteMessageAsync(message, stoppingToken);
          logger.LogInformation("Message delete");
        }

        logger.LogInformation("ExecuteAsync loop end");
        await Task.Delay(1000, stoppingToken);
      }
      logger.LogInformation("ExecuteAsync done");
    }
  }
}
