using SixLabors.ImageSharp.PixelFormats;

namespace Fractal.Api.Logic
{
  public interface IFractalCalculator
  {
    void Init(double x, double y, double precision, int viewMaxX, int viewMaxY);
    int GetIterationStepNumber(int pointX, int pointY);
  }
}