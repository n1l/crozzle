using CrozzleEngine.Model;

namespace CrozzleEngine.Interfaces
{
    /// <summary>
    /// Common interface for files validation
    /// </summary>
    public interface IFileValidator
    {
        /// <summary>
        /// Validate file
        /// </summary>
        /// <param name="filePath">Path to file</param>
        /// <returns>Result of the validation process <see cref="ValidationResult"/></returns>
        ValidationResult Validate(string filePath);
    }
}
