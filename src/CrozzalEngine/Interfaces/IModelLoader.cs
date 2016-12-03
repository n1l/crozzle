using CrozzleEngine.Model;

namespace CrozzleEngine.Interfaces
{
  /// <summary>
  /// Model loader interface
  /// </summary>
  /// <typeparam name="T">Model tye</typeparam>
  public interface IModelLoader<out T>
  {
    /// <summary>
    /// Create model from file
    /// </summary>
    /// <param name="filePath">Path to file</param>
    void CreateModel(string filePath);

    /// <summary>
    /// Get model after creation
    /// </summary>
    /// <returns><see cref="T"/></returns>
    T GetModel();
  }
}
