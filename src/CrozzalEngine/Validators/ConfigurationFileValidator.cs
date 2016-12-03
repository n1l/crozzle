using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CrozzleEngine.Interfaces;
using CrozzleEngine.Model;

namespace CrozzleEngine.Validators
{
  /// <summary>
  /// Configuration file validator, need for validation before create object model
  /// </summary>
  public class ConfigurationFileValidator : FileLoader, IFileValidator
  {
    private const string GROUPS_LIMIT_ERROR_MSG = "ERROR on configuration file validation: GROUPSPERCROZZLELIMIT is not match to pattern GROUPSPERCROZZLELIMIT=x where x is positive number";
    private const string POINT_PER_WORD_ERROR_MSG = "ERROR on configuration file validation: POINTSPERWORD is not match to pattern POINTSPERWORD=x where x is positive number";


    //readonly fields
    private readonly List<char> _letters;

    //state filed
    private bool _groupsLimitSet;
    private bool _pointPerWordSet;
    private bool _intersectingSet;
    private bool _nonintersectingSet;
    private int _letterIndex;

    private void Reset()
    {
      _groupsLimitSet = _pointPerWordSet = _intersectingSet = _nonintersectingSet = false;
      _letterIndex = 0;
    }


    /// <summary>
    /// Ctor
    /// </summary>
    public ConfigurationFileValidator()
    {
      //create list of letters
      _letters = new List<char>(Enumerable.Range(65, 26).Select(code => (char)code));
    }

    /// <summary>
    /// Validate config
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
        CrozzleLogger.Log(ex.Message);
        result.AddCriticalError(ex.Message);
        return result;
      }

      foreach (var @string in file)
      {
        if (!_groupsLimitSet)
        {
          var regEx = new Regex(Patterns.GROUPS_LIMIT_PATTERN);
          var resultMatch = regEx.Match(@string);
          if (!resultMatch.Success)
          {
            CrozzleLogger.Log(GROUPS_LIMIT_ERROR_MSG);
            result.AddConfigError(GROUPS_LIMIT_ERROR_MSG);
          }
          _groupsLimitSet = true;
          continue;
        }

        if (!_pointPerWordSet)
        {
          var regEx = new Regex(Patterns.POINTS_PER_WORD_PATTERN);
          var resultMatch = regEx.Match(@string);
          if (!resultMatch.Success)
          {
            CrozzleLogger.Log(POINT_PER_WORD_ERROR_MSG);
            result.AddConfigError(POINT_PER_WORD_ERROR_MSG);
          }
          _pointPerWordSet = true;
          continue;
        }

        if (!_intersectingSet)
        {
          var regEx = new Regex(Patterns.INTERSECTING_POINT_PATTERN);
          var resultMatch = regEx.Match(@string);
          if (!resultMatch.Success)
          {
            _letterIndex++;
            var message =
              $"ERROR on configuration file validation: INTERSECTING is not match to pattern INTERSECTING:[A-Z]=x where x is positive number, row index is {_letterIndex}";
            CrozzleLogger.Log(message);
            result.AddConfigError(message);
          }
          else
          {
            _letterIndex++;
          }
          if (_letterIndex == _letters.Count)
          {
            _intersectingSet = true;
            _letterIndex = 0;
            continue;
          }
        }
        
        if (!_nonintersectingSet && _intersectingSet)
        {
          var regEx = new Regex(Patterns.NONINTERSECTING_POINT_PATTERN);
          var resultMatch = regEx.Match(@string);
          if (!resultMatch.Success)
          {
            _letterIndex++;
            var message =
              $"ERROR on configuration file validation: NONINTERSECTING is not match to pattern NONINTERSECTING:[A-Z]=x where x is positive number, row index is {_letterIndex}";
            CrozzleLogger.Log(message);
            result.AddConfigError(message);
          }
          else
          {
            _letterIndex++;
          }
          if (_letterIndex == _letters.Count)
          {
            _nonintersectingSet = true;
            _letterIndex = 0;
          }
        }
      }
      CrozzleLogger.EndLog(fileName, false, result.IsValid);
      return result;
    }
  }
}
