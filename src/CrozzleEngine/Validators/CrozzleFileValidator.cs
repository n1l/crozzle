using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using CrozzleEngine.Interfaces;
using CrozzleEngine.Model;

namespace CrozzleEngine.Validators
{
    /// <summary>
    /// Crozzle file validator object, validate crozzle files before create model
    /// </summary>
    public class CrozzleFileValidator : FileLoader, IFileValidator
    {
        private const string NOT_CORRECT_DATA_MSG = "ERROR: Not correct data in {0}: expected {1}, but was {2}";
        private const string DIFFICULTY_IS_INCORRECT_MSG = "ERROR: Difficulty level is incorrect: expected {0} or {1} or {2}, but was {3}";
        private const string NUMBER_IS_INCORRECT_MSG = "ERROR: Number of {0} is incorrect: expected integer between {1}, but was {2}";
        private const string NUMBER_IS_INCORRECT_MSG2 = "ERROR: Number of {0} is incorrect: expected positive integer, but was {1}";
        private const string NOT_UNIQUE_WORDS_MSG = "ERROR: Not unique words {0}";
        private const string MULTIPLE_INSERTED_WORDS_MSG = "ERROR: Multiple inserted words in crozzle: {0}";
        private const string NOT_CORRECT_FORMAT_MSG = "ERROR: {0} not in correct format";

        private const string EASY_DIFFICULTY_PATTERN = "EASY";
        private const string MEDIUM_DIFFICULTY_PATTERN = "MEDIUM";
        private const string HARD_DIFFICULTY_PATTERN = "HARD";
        private const string HORIZONTAL_ORIENTATION_PATTERN = "HORIZONTAL";
        private const string VERTICAL_ORIENTATION_PATTERN = "VERTICAL";
        private const string WORD_REGEX_PATTERN = "^[A-Z]+$";


        private const int CROZZLE_HEADER_LENGTH = 6;
        private const int NUMBER_OF_WORDS_MIN = 10;
        private const int NUMBER_OF_WORDS_MAX = 1000;
        private const int NUMBER_OF_ROWS_MIN = 4;
        private const int NUMBER_OF_ROWS_MAX = 400;
        private const int NUMBER_OF_COLLUMNS_MIN = NUMBER_OF_ROWS_MIN * 2;
        private const int NUMBER_OF_COLLUMNS_MAX = NUMBER_OF_ROWS_MAX * 2;
        private const int ZERO = 0;
        private const int ISUNIQUE = 1;
        private const int ARGUMENT_NUMBER_COUNT = 4;



        private Dictionary<string, int> _words;
        private Dictionary<string, int> _inseredWords;

        private int _index;
        private bool _headerIsSet;
        private bool _wordsDictionaryIsSet;

        private void Reset()
        {
            _index = 0;
            _headerIsSet = _wordsDictionaryIsSet = false;
            _words = new Dictionary<string, int>();
            _inseredWords = new Dictionary<string, int>();
        }

        /// <summary>
        /// Validate file
        /// </summary>
        /// <param name="filePath">Path to file</param>
        /// <returns>Result of validation, has state and messages <see cref="ValidationResult"/></returns>
        public ValidationResult Validate(string filePath)
        {
            var fileName = filePath.Split('\\').Last();
            CrozzleLogger.InitLog(fileName, false);

            Reset();
            var result = new ValidationResult();

            try
            {
                LoadFile(filePath);
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
                return result;
            }

            var maxRows = 0;
            var maxColumns = 0;
            foreach (var @string in file)
            {
                if (!_headerIsSet)
                {
                    //6 values in string
                    var header = @string.Split(',');
                    var headerDataCountIsOk = header.Length == CROZZLE_HEADER_LENGTH;
                    if (!headerDataCountIsOk)
                    {
                        var message = string.Format(NOT_CORRECT_FORMAT_MSG, "Header(first row)");
                        CrozzleLogger.Log(message);
                        result.AddCriticalError(message);
                        _headerIsSet = true;
                        continue;
                    }

                    //EASY, MEDIUM or HARD
                    var difficulty = header[0];
                    var difficultyIsOk = difficulty.Equals(EASY_DIFFICULTY_PATTERN) || difficulty.Equals(MEDIUM_DIFFICULTY_PATTERN)
                                         || difficulty.Equals(HARD_DIFFICULTY_PATTERN);
                    if (!difficultyIsOk)
                    {
                        var message = string.Format(DIFFICULTY_IS_INCORRECT_MSG, EASY_DIFFICULTY_PATTERN, MEDIUM_DIFFICULTY_PATTERN,
                                                    HARD_DIFFICULTY_PATTERN, difficulty);
                        CrozzleLogger.Log(message);
                        result.AddCriticalError(message);
                    }

                    //for all numberics check
                    int tmp;


                    //numerics
                    //number of words
                    var numberOfWords = header[1];
                    var numberOfWordsIsOk = int.TryParse(numberOfWords, out tmp) && (tmp >= NUMBER_OF_WORDS_MIN || tmp <= NUMBER_OF_WORDS_MAX);
                    if (!numberOfWordsIsOk)
                    {
                        var message = string.Format(NUMBER_IS_INCORRECT_MSG, "words", $"[{NUMBER_OF_WORDS_MIN}..{NUMBER_OF_WORDS_MAX}]", numberOfWords);
                        CrozzleLogger.Log(message);
                        result.AddError(message);
                    }

                    //number of rows
                    var numberOfRows = header[2];
                    var numberOfRowsIsOk = int.TryParse(numberOfRows, out tmp) && (tmp >= NUMBER_OF_ROWS_MIN || tmp <= NUMBER_OF_ROWS_MAX);
                    maxRows = tmp;
                    if (!numberOfRowsIsOk)
                    {
                        var message = string.Format(NUMBER_IS_INCORRECT_MSG, "rows", $"[{NUMBER_OF_ROWS_MIN}..{NUMBER_OF_ROWS_MAX}]", numberOfRows);
                        CrozzleLogger.Log(message);
                        result.AddError(message);
                    }

                    //number of columns
                    var numberOfColumns = header[3];
                    var numberOfColumnsIsOk = int.TryParse(numberOfColumns, out tmp) && (tmp >= NUMBER_OF_COLLUMNS_MIN || tmp <= NUMBER_OF_COLLUMNS_MAX);
                    maxColumns = tmp;
                    if (!numberOfColumnsIsOk)
                    {
                        var message = string.Format(NUMBER_IS_INCORRECT_MSG, "columns", $"[{NUMBER_OF_COLLUMNS_MIN}..${NUMBER_OF_COLLUMNS_MAX}]", numberOfColumns);
                        CrozzleLogger.Log(message);
                        result.AddError(message);
                    }

                    //number of horizontal words
                    var numberOfHorizontalWords = header[4];
                    var numberOfHorizontalWordsIsOk = int.TryParse(numberOfHorizontalWords, out tmp) && tmp > ZERO;
                    if (!numberOfHorizontalWordsIsOk)
                    {
                        var message = string.Format(NUMBER_IS_INCORRECT_MSG2, "horizontal words", numberOfHorizontalWords);
                        CrozzleLogger.Log(message);
                        result.AddError(message);
                    }


                    //number of vertical words
                    var numberOfVerticalWords = header[5];
                    var numberOfVerticalWordsIsOk = int.TryParse(numberOfVerticalWords, out tmp) && tmp > ZERO;
                    if (!numberOfVerticalWordsIsOk)
                    {
                        var message = string.Format(NUMBER_IS_INCORRECT_MSG2, "vertical words", numberOfVerticalWords);
                        CrozzleLogger.Log(message);
                        result.AddError(message);
                    }

                    _headerIsSet = true;
                    continue;
                }


                if (!_wordsDictionaryIsSet)
                {
                    var wordsList = @string.Split(',');

                    foreach (var word in wordsList)
                    {
                        if (!_words.ContainsKey(word))
                        {
                            _words.Add(word, 0);
                        }
                        _words[word]++;
                    }

                    var allWordsAreUnique = !_words.Any(word => word.Value > 1);
                    if (!allWordsAreUnique)
                    {
                        var notUniqueWordsList = string.Join(",", _words.Where(word => word.Value > 1)
                                                                        .Select(word => word.Key));
                        var message = string.Format(NOT_UNIQUE_WORDS_MSG, notUniqueWordsList);
                        CrozzleLogger.Log(message);
                        result.AddError(message);
                    }

                    _wordsDictionaryIsSet = true;
                    continue;
                }

                //check words
                {
                    int tmp;
                    _index++;

                    var wordPreferences = @string.Split(',');
                    var wordPreferencesIsOk = wordPreferences.Length == ARGUMENT_NUMBER_COUNT;
                    if (!wordPreferencesIsOk)
                    {
                        var message = string.Format(NOT_CORRECT_DATA_MSG, $"one of words preference, row number - {_index}", ARGUMENT_NUMBER_COUNT, wordPreferences.Length);
                        CrozzleLogger.Log(message);
                        result.AddCriticalError(message);
                        continue;
                    }

                    var orientation = wordPreferences[0];
                    var orientationIsOk = orientation.Equals(HORIZONTAL_ORIENTATION_PATTERN) || orientation.Equals(VERTICAL_ORIENTATION_PATTERN);
                    if (!orientationIsOk)
                    {
                        var message = string.Format(NOT_CORRECT_DATA_MSG, $"orientation preferences, row number - {_index}",
                                                    $"{HORIZONTAL_ORIENTATION_PATTERN} or {VERTICAL_ORIENTATION_PATTERN}", orientation);
                        CrozzleLogger.Log(message);
                        result.AddCriticalError(message);
                    }

                    var rowIndex = wordPreferences[1];
                    var rowIndexIsOk = int.TryParse(rowIndex, out tmp);
                    var rowInt = tmp;
                    if (!rowIndexIsOk)
                    {
                        var message = string.Format(NOT_CORRECT_DATA_MSG, $"rows index preferences, row number - {_index}", "integer value", rowIndex);
                        CrozzleLogger.Log(message);
                        result.AddError(message);
                    }

                    if (rowInt > maxRows)
                    {
                        var message = string.Format($"Incorrect data in crozzle preferences, max rows count is {maxRows}, but word index was - {rowInt}");
                        CrozzleLogger.Log(message);
                        result.AddCriticalError(message);
                    }

                    var columnIndex = wordPreferences[2];
                    var columnIndexIsOk = int.TryParse(columnIndex, out tmp);
                    var columnInt = tmp;
                    if (!columnIndexIsOk)
                    {
                        var message = string.Format(NOT_CORRECT_DATA_MSG, $"column index preferences, row number - {_index}", "integer value",
                                                    columnIndex);
                        CrozzleLogger.Log(message);
                        result.AddError(message);
                    }

                    if (columnInt > maxColumns)
                    {
                        var message =
                          string.Format($"Incorrect data in crozzle preferences, max columns count is {maxColumns}, but word column index was - {columnInt}");
                        CrozzleLogger.Log(message);
                        result.AddCriticalError(message);
                    }

                    var actualWord = wordPreferences[3];
                    var regex = new Regex(WORD_REGEX_PATTERN);
                    var actualWordIsOk = regex.IsMatch(actualWord) && _words.ContainsKey(actualWord);
                    if (!actualWordIsOk)
                    {
                        var message = string.Format(NOT_CORRECT_DATA_MSG, $"words to view, word is not ok, row number - {_index}",
                                                    "word, that contains in dictionary", actualWord);
                        CrozzleLogger.Log(message);
                        result.AddError(message);
                    }

                    if (!_inseredWords.ContainsKey(actualWord))
                    {
                        _inseredWords.Add(actualWord, ZERO);
                    }
                    _inseredWords[actualWord]++;
                }


            }

            if (_inseredWords.Any(word => word.Value > 1))
            {
                var notUniqueWordsList = string.Join(",", _inseredWords.Where(word => word.Value > ISUNIQUE).Select(word => word.Key));
                var message = string.Format(MULTIPLE_INSERTED_WORDS_MSG, notUniqueWordsList);
                CrozzleLogger.Log(message);
                result.AddError(message);
            }

            CrozzleLogger.EndLog(fileName, false, result.IsValid);

            return result;
        }
    }
}
