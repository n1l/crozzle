using System.Collections.Generic;
using CrozzleEngine.Model;

namespace CrozzleEngine.Interfaces
{
  /// <summary>
  /// Common configuration interface
  /// </summary>
  public interface IConfiguration
  {
    /// <summary>
    /// Title
    /// </summary>
    string ConfigurationTitle { get; }
    
    /// <summary>
    /// Max groups count
    /// </summary>
    int GrupsLimit { get; }

    /// <summary>
    /// Point per word you recieve
    /// </summary>
    int PointsPerWord { get; }

    /// <summary>
    /// Point per each letter, you recieve <see cref="PointPerLetter"/>
    /// </summary>
    List<PointPerLetter> PointPerLetters { get; }

    /// <summary>
    /// Validation result after config is loaded <see cref="ValidationResult"/>
    /// </summary>
    ValidationResult ValidationResult { get; }
  }
}
