using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fractal.Api.Model
{

  public class FractalComputationMessage
  {
    public IList<FractalComputationJobList> Rows { get; set; }
  }

  public class FractalComputationJobList
  {
    public IList<FractalComputationJob> Jobs { get; set; }
  }

  public class FractalComputationJob
  {
    public string JobId { get; set; }
    public string Link { get; set; }

  }
}
