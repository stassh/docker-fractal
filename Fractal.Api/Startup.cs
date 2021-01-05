using Fractal.Api.Logic;
using Fractal.Api.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fractal.Api
{
  public class Startup
  {
    // private ILogger logger { get; }

    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }
    

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddSingleton<IColorGradient, ColorGradient>();
      services.AddScoped<IFractalComputator, FractalComputator>();
      
      var JobInfoConnectionString = Configuration.GetConnectionString("FractalJobsQueue");
      services.AddTransient<IJobSchedulerService>(s => new JobSchedulerService(s.GetService<ILogger<JobSchedulerService>>(), JobInfoConnectionString));
      services.AddTransient<IFractalTileSchedulerService>(s => new FractalTileSchedulerService(s.GetService<ILogger<FractalTileSchedulerService>>(), JobInfoConnectionString));
      // var JobInfoConnectionString = Configuration.GetConnectionString("FractalJobsQueue");

      //CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
      //CloudQueue queue = queueClient.GetQueueReference("<queue>");

      // logger.LogInformation("ConnectionString:FractalJobsQueue = {0}", JobInfoConnectionString);
      services.AddHostedService<FractalJobWorker>();
      services.AddControllers();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
