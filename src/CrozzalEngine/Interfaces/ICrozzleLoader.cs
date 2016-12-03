using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrozzleEngine.Interfaces
{
  /// <summary>
  /// Crozzle loader interface
  /// </summary>
  /// <typeparam name="TDependency">Dependency object, configuration for crozzle <see cref="IConfiguration"/></typeparam>
  public interface ICrozzleLoader<in TDependency> : IModelLoader<ICrozzle> where TDependency: IConfiguration
  {
    /// <summary>
    /// Set config for model
    /// </summary>
    /// <param name="config">Configuration</param>
    /// <returns>Fluent, return self</returns>
    ICrozzleLoader<TDependency> SetConfig(TDependency config);
  }
}
