using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrozzleEngine.Interfaces;

namespace CrozzleEngine.Model
{
  /// <summary>
  /// Configuration of crozzle
  /// </summary>
  public class CrozzleConfiguration : IConfiguration
  {
    /// <summary>
    /// Ctor
    /// </summary>
    public CrozzleConfiguration()
    {
      PointPerLetters = new List<PointPerLetter>();
    }

    /// <summary>
    /// Difficult level
    /// </summary>
    public Difficult DifficultLevel { get; set; }

    /// <summary>
    /// Config title
    /// </summary>
    public string ConfigurationTitle { get; set; }

    /// <summary>
    /// Groups allowed limit
    /// </summary>
    public int GrupsLimit { get; set; }

    /// <summary>
    /// Points per word
    /// </summary>
    public int PointsPerWord { get; set; }

    /// <summary>
    /// Point per letter <see cref="PointPerLetter"/>
    /// </summary>
    public List<PointPerLetter> PointPerLetters { get; }

    /// <summary>
    /// Result of the validation process <see cref="ValidationResult"/>
    /// </summary>
    public ValidationResult ValidationResult { get; set; }
  }
}
