using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CrozzleEngine.Exceptions;
using CrozzleEngine.Interfaces;
using CrozzleEngine.Model;

namespace CrozzleEngine.Loaders
{
  public class ConfigurationLoader : FileLoader, IModelLoader<IConfiguration>
  {
    private readonly IFileValidator _configFileValidator;
    private readonly List<char> _letters;


    private CrozzleConfiguration _config;
    private bool _loaded;
    private bool _groupsLimitSet;
    private bool _pointPerWordSet;
    private bool _intersectingSet;
    private bool _nonintersectingSet;
    private int _letterIndex;

    public ConfigurationLoader(IFileValidator configFileValidator)
    {
      _letters = new List<char>(Enumerable.Range(65, 26).Select(code => (char)code));
      _configFileValidator = configFileValidator;
    }

    private void Reset()
    {
      _config = new CrozzleConfiguration();
      _letterIndex = 0;
      _loaded = _groupsLimitSet = _pointPerWordSet = _intersectingSet = _nonintersectingSet = false;
    }

    public IConfiguration GetModel()
    {
      return _loaded ? _config : null;
    }

    public void CreateModel(string filePath)
    {

      if (string.IsNullOrEmpty(filePath))
      {
        throw new ModelLoadException("Please select config file");
      }

      Reset();

      var validationResult = _configFileValidator.Validate(filePath);

      if (validationResult.Critical)
      {
        throw new ModelLoadException($"Critical errors in files: {validationResult}");
      }


      var fileName = filePath.Split('\\').Last();

      _config.ConfigurationTitle = fileName;
      _config.ValidationResult = validationResult;

      if (validationResult.ConfigError)
      {
        _loaded = true;
        return;
      }

      LoadFile(filePath);

      foreach (var @string in file)
      {
        if (!_groupsLimitSet)
        {
          var regEx = new Regex(Patterns.NUMERIC_PATTERN);
          var resultMatch = regEx.Match(@string);
          _config.GrupsLimit = int.Parse(resultMatch.Groups[0].Value);
          _groupsLimitSet = true;
          continue;
        }

        if (!_pointPerWordSet)
        {
          var regEx = new Regex(Patterns.NUMERIC_PATTERN);
          var resultMatch = regEx.Match(@string);
          _config.PointsPerWord = int.Parse(resultMatch.Groups[0].Value);
          _pointPerWordSet = true;
          continue;
        }

        if (!_intersectingSet)
        {
          var regExLetter = new Regex(Patterns.LETTER_PATTERN);
          var letterResultMatch = regExLetter.Match(@string);

          var regExNumeric = new Regex(Patterns.NUMERIC_PATTERN);
          var numericResultMatch = regExNumeric.Match(@string);

          var point = new PointPerLetter
          {
            Letter = letterResultMatch.Groups[0].Value[1],
            PointType = LetterPointType.Intersecting,
            Point = int.Parse(numericResultMatch.Groups[0].Value)
          };
          _config.PointPerLetters.Add(point);
          _letterIndex++;

          if (_letterIndex == _letters.Count)
          {
            _intersectingSet = true;
            _letterIndex = 0;
          }

          continue;
        }

        if (!_nonintersectingSet && _intersectingSet)
        {

          var regExLetter = new Regex(Patterns.LETTER_PATTERN);
          var letterResultMatch = regExLetter.Match(@string);

          var regExNumeric = new Regex(Patterns.NUMERIC_PATTERN);
          var numericResultMatch = regExNumeric.Match(@string);

          var point = new PointPerLetter
          {
            Letter = letterResultMatch.Groups[0].Value[1],
            PointType = LetterPointType.Nonintersecting,
            Point = int.Parse(numericResultMatch.Groups[0].Value)
          };
          _config.PointPerLetters.Add(point);
          _letterIndex++;
          

          if (_letterIndex == _letters.Count)
          {
            _nonintersectingSet = true;
            _letterIndex = 0;
          }

        }
      }
      _loaded = true;
    }
  }
}
