using SixLabors.ImageSharp.PixelFormats;

namespace Fractal.Api.Logic
{
  public interface IColorGradient
  {
    Rgba32 Rainbow(int wave);
  }
}