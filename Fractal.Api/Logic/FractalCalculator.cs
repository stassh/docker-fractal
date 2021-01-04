using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fractal.Api.Logic
{
  public class FractalCalculator : IFractalCalculator
  {
    public static int ITERATION_LIMIT = 255;
    private double x1;
    private double y1;

    private double deltaX;
    private double deltaY;

    public void Init(double x, double y, double precision, int viewMaxX, int viewMaxY)
    {
      this.x1 = x - precision;


      this.deltaX = (precision + precision) / viewMaxX;

      this.y1 = y - precision;

      this.deltaY = (precision + precision) / viewMaxY;
    }

    public int GetIterationStepNumber(int pointX, int pointY)
    {
      double x = x1 + deltaX * pointX;
      double y = y1 + deltaY * pointY;

      return GetIterationStepNumber(x, y, FractalCalculator.ITERATION_LIMIT);
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
  }
}
