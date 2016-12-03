using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrozzleEngine.Exceptions;
using CrozzleEngine.Interfaces;
using CrozzleEngine.Model;

namespace CrozzleEngine.Loaders
{
  /// <summary>
  /// Crozzle model loader, loads crozzle from file, and create Crozzle object model
  /// </summary>
  public class CrozzleLoader : FileLoader, ICrozzleLoader<IConfiguration>
  {
    private readonly IFileValidator _crozzleFileValidator;

    private Crozzle _crozzle;
    private bool _headerIsSet;
    private bool _wordsDictionaryIsSet;
    private IConfiguration _crozzleConfig;

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="crozzleFileValidator">Dependency</param>
    public CrozzleLoader(IFileValidator crozzleFileValidator)
    {
      _crozzleFileValidator = crozzleFileValidator;
    }

    private void Reset()
    {
      _headerIsSet = _wordsDictionaryIsSet = false;
      _crozzle = new Crozzle();
    }

    /// <summary>
    /// Create model object
    /// </summary>
    /// <param name="filePath">Path to file</param>
    public void CreateModel(string filePath)
    {
      Reset();
      var validationResult = _crozzleFileValidator.Validate(filePath);
      if (_crozzleConfig == null)
      {
        throw new ModelLoadException("Configuration is not loaded!\n Please load configuration at first");
      }

      _crozzle.ValidationResult = validationResult + _crozzleConfig.ValidationResult;

      if (_crozzle.ValidationResult.Critical)
      {
        throw new ModelLoadException($"Critical errors in files: {_crozzle.ValidationResult}");
      }

      var fileName = filePath.Split('\\').Last();

      _crozzle.Name = fileName;
      _crozzle.SetConfig(_crozzleConfig);

      LoadFile(filePath);

      foreach (var @string in file)
      {
        if (!_headerIsSet)
        {
          var header = @string.Split(',');
          _crozzle.DifficultyLevel = (Difficult)Enum.Parse(typeof(Difficult), header[0], true);
          _crozzle.WordsCount = int.Parse(header[1]);
          _crozzle.RowsCount = int.Parse(header[2]);
          _crozzle.ColumnsCount = int.Parse(header[3]);
          _crozzle.HorizontalWordsCount = int.Parse(header[4]);
          _crozzle.VerticalWordsCount = int.Parse(header[5]);

          _headerIsSet = true;
          continue;
        }

        if (!_wordsDictionaryIsSet)
        {
          var wordsList = @string.Split(',')
                                     .ToList();
          _crozzle.WordsDictionary = wordsList;

          _wordsDictionaryIsSet = true;
          continue;
        }

        var word = new DisplayWord();
        var wordPreferences = @string.Split(',');
        word.Orientation = (WordOrientation)Enum.Parse(typeof(WordOrientation), wordPreferences[0], true);
        word.HorizontalPosition = int.Parse(wordPreferences[1]);
        word.VerticalPosition = int.Parse(wordPreferences[2]);
        word.Word = wordPreferences[3];
        _crozzle.DisplayedWords.Add(word);
      }

      _crozzle.SetLoadingComplete();
    }

    public ICrozzle GetModel()
    {
      return _crozzle;
    }

    public ICrozzleLoader<IConfiguration> SetConfig(IConfiguration config)
    {
      _crozzleConfig = config;
      return this;
    }
  }
}
