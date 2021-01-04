using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fractal.Api
{
  public class Fractal
  {
    public static int ITERATION_LIMIT = 255;
    private readonly double x1;
    private readonly double x2;
    private readonly double y1;
    private readonly double y2;
    private readonly double deltaX;
    private readonly double deltaY;

    private readonly Rgba32[] colorMapping;

    private Fractal()
    {
      colorMapping = new Rgba32[ITERATION_LIMIT + 1];
      for (int i = 0; i < ITERATION_LIMIT + 1; i++)
      {
        colorMapping[i] = ColorForIteration(i);

      }
      colorMapping[ITERATION_LIMIT] = new Rgba32(0, 0, 0);

    }

    public Fractal(double x, double y, double precision, int viewMaxX, int viewMaxY) : this()
    {
      this.x1 = x - precision;
      this.x2 = x + precision;

      this.deltaX = (precision + precision) / viewMaxX;

      this.y1 = y - precision;
      this.y2 = y + precision;
      this.deltaY = (precision + precision) / viewMaxY;
    }

    public Fractal(double x1, double x2, double y1, double y2, int maxX, int maxY) : this()
    {
      this.x1 = x1;
      this.x2 = x2;

      this.deltaX = (x2 - x1) / maxX;

      this.y1 = y1;
      this.y2 = y2;
      this.deltaY = (y2 - y1) / maxY;
    }

    public int GetIterationStepNumber(int pointX, int pointY)
    {
      double x = x1 + deltaX * pointX;
      double y = y1 + deltaY * pointY;

      return GetIterationStepNumber(x, y, Fractal.ITERATION_LIMIT);
    }

    public int GetIterationStepNumber(double pointX, double pointY, int limit)
    {
      double a = 0, b = 0;

      for (int i = 0; i < limit; i++)
      {
        double t = a * a - b * b;
        b = 2 * a * b;

        a = t;

        a += pointX;
        b += pointY;

        var m = a * a + b * b;

        if (m > 4)
        {
          return i;
        }
      }
      return limit;
    }

    public Rgba32 Rainbow(int wave)
    {
      return this.colorMapping[wave];
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
  }
}
