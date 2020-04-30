using System.Collections.Generic;
using CrozzleEngine.Model;

namespace CrozzleEngine.Interfaces
{
    /// <summary>
    /// Main interface of the program
    /// The crozzle common interface
    /// </summary>
    public interface ICrozzle
    {
        /// <summary>
        /// Difficulty level
        /// </summary>
        Difficult DifficultyLevel { get; }

        /// <summary>
        /// Words count in the crozzle
        /// </summary>
        int WordsCount { get; }

        /// <summary>
        /// Rows count in result matrix
        /// </summary>
        int RowsCount { get; }

        /// <summary>
        /// Columns count in result matrix
        /// </summary>
        int ColumnsCount { get; }

        /// <summary>
        /// Horizontal words in the crozzle count
        /// </summary>
        int HorizontalWordsCount { get; }

        /// <summary>
        /// Vertical words in the crozzle count
        /// </summary>
        int VerticalWordsCount { get; }

        /// <summary>
        /// Score
        /// </summary>
        int Score { get; }

        /// <summary>
        /// Words in the crozzle dictionary
        /// </summary>
        List<string> WordsDictionary { get; }

        /// <summary>
        /// Words displayed in the crozzle <see cref="DisplayWord"/>
        /// </summary>
        List<DisplayWord> DisplayedWords { get; }

        /// <summary>
        /// Validation result of crozzle model loading
        /// </summary>
        ValidationResult ValidationResult { get; }

        /// <summary>
        /// Serialize to html for viewing
        /// </summary>
        /// <returns></returns>
        string SerializeToHtml();

        /// <summary>
        /// Set crozzle config
        /// </summary>
        /// <param name="config"></param>
        void SetConfig(IConfiguration config);
    }
}
