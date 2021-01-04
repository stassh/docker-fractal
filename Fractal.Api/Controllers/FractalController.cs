﻿using Fractal.Api.Logic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Fractal.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class FractalController : ControllerBase
  {
    private readonly ILogger<FractalController> _logger;
    private readonly IFractalCalculator fractal;
    private readonly IColorGradient colorGradient;

    public FractalController(ILogger<FractalController> logger, IFractalCalculator fractal, IColorGradient colorGradient)
    {
      _logger = logger;
      this.fractal = fractal;
      this.colorGradient = colorGradient;
    }

    [HttpGet]
    [Route("full")]
    public async Task GetFull()
    {
      var response = HttpContext.Response;
      response.ContentType = "image/png";

      this._logger.LogInformation("Get");
      var imageBytes = await this.GetImageArray(HttpContext.Response);

      await response.Body.WriteAsync(imageBytes, 0, imageBytes.Length);
    }

    private async Task<byte[]> GetImageArray(HttpResponse response, double x = 0, double y = 0, double precision = 2.5, int VIEWMAXX = 800, int VIEWMAXY = 800)
    {
      var ms = new MemoryStream();
      response.RegisterForDispose(ms);
      var image = new Image<Rgba32>(VIEWMAXX, VIEWMAXY);
      response.RegisterForDispose(image);

      return await Task.Run(() =>
      {
        fractal.Init(x, y, precision, VIEWMAXX, VIEWMAXY);
        // todo: parallelize it
        for (int x = 0; x < image.Width; x++)
        {
          for (int y = 0; y < image.Height; y++)
          {
            // image[x, y] = x == y ? Color.Black : Color.CadetBlue;
            image[x, y] = colorGradient.Rainbow(fractal.GetIterationStepNumber(x, y));
          }
        }

        image.SaveAsPng(ms);
        var imageBytes = ms.ToArray();
        return imageBytes;
      });
    }
  }
}
