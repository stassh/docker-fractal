using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fractal.Api.Logic
{
  public class ColorGradient : IColorGradient
  {
    private static readonly int ITERATION_LIMIT = 255;
    private readonly Rgba32[] colorMapping;

    public ColorGradient()
    {

      colorMapping = new Rgba32[ITERATION_LIMIT + 1];
      for (int i = 0; i < ITERATION_LIMIT + 1; i++)
      {
        colorMapping[i] = ColorForIteration(i);
      }
      colorMapping[ITERATION_LIMIT] = new Rgba32(0, 0, 0);
    }

    public Rgba32 ColorForIteration(int iteration)
    {
      return new Rgba32(ColorWave(iteration), ColorWave(iteration + 85), ColorWave(iteration + 170), 255);
    }

    public byte ColorWave(int iteration)
    {
      var wave = 125 * Math.Sin(Convert.ToDouble(iteration) / 15) + 126;
      return (byte)(Convert.ToByte(wave));
    }
    public Rgba32 Rainbow(int wave)
    {
      return this.colorMapping[wave];
    }
  }
}
