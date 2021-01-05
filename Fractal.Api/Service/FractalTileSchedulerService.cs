using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Fractal.Api.Model;
using System.Threading;

namespace Fractal.Api.Service
{
  public interface IFractalTileSchedulerService
  {
    Task PushMessageAsync(FractalComputationMessage messagePayload);
    Task<CloudQueueMessage> GetMessageAsync(CancellationToken cancelationToken);
    Task DeleteMessageAsync(CloudQueueMessage message, CancellationToken cancelationToken);
  }

  public class FractalTileSchedulerService : IFractalTileSchedulerService
  {
    private readonly ILogger<FractalTileSchedulerService> logger;
    private readonly string connectionString;
    private CloudQueue queue;

    public FractalTileSchedulerService(ILogger<FractalTileSchedulerService> logger, string connectionString)
    {
      this.logger = logger;
      this.connectionString = connectionString;
      logger.LogInformation("Constructor done");
    }

    private async Task InitAsync()
    {
      var storageAccount = CloudStorageAccount.Parse(connectionString);
      var cloudQueueClient = storageAccount.CreateCloudQueueClient();
      queue = cloudQueueClient.GetQueueReference("fractal-tile-jobs");
      await queue.CreateIfNotExistsAsync();
      logger.LogInformation("Init done");
    }

    public async Task<CloudQueueMessage> GetMessageAsync(CancellationToken cancelationToken)
    {
      if (queue == null)
      {
        await InitAsync();
      }
      var message = await queue.GetMessageAsync(TimeSpan.FromSeconds(1), null, null, cancelationToken);
      return message;
    }

    public async Task DeleteMessageAsync(CloudQueueMessage message, CancellationToken cancelationToken)
    {
      if (queue == null)
      {
        await InitAsync();
      }
      await queue.DeleteMessageAsync(message.Id, message.PopReceipt, null, null, cancelationToken);
    }

    public async Task PushMessageAsync(FractalComputationMessage messagePayload)
    {
      if (queue == null)
      {
        await InitAsync();
      }
      var payload = JsonSerializer.Serialize(messagePayload);
      var cloudMessage = new CloudQueueMessage(payload);

      await queue.AddMessageAsync(cloudMessage);
      logger.LogDebug("PushMessageAsync with payload: {0}", payload);
    }

  }
}
