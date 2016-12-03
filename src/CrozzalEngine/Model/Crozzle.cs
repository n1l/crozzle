using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;
using CrozzleEngine.Exceptions;
using CrozzleEngine.Interfaces;

namespace CrozzleEngine.Model
{
  [XmlRoot(ElementName = "crozzle")]
  public class Crozzle : ICrozzle
  {
    private const string HTML_TEMPLATE_NAME = "HtmlTemplate.xslt";


    private bool _loaded;
    private IConfiguration _config;
    private Difficult _level;

    /// <summary>
    /// Ctor
    /// </summary>
    public Crozzle()
    {
      DisplayedWords = new List<DisplayWord>();
      Rows = new List<Row>();
    }



    /// <summary>
    /// Score of crozzle
    /// </summary>
    [XmlElement(ElementName = "score")]
    public int Score { get; set; }

    /// <summary>
    /// Matrix
    /// </summary>
    [XmlArray(ElementName = "rows")]
    public List<Row> Rows { get; }

    /// <summary>
    /// Validation
    /// </summary>
    [XmlElement(ElementName = "validation")]
    public ValidationResult ValidationResult { get; set; }

    #region [ Implementation of ICrozzle ]

    /// <summary>
    /// Difficult level for serizlization
    /// </summary>
    [XmlElement(ElementName = "difficult")]
    public string Difficulty { get; set; }

    /// <summary>
    /// Difficulty level
    /// </summary>
    [XmlIgnore]
    public Difficult DifficultyLevel
    {
      get { return _level; }
      set
      {
        _level = value;
        Difficulty = _level.ToString().ToUpper();
      }
    }

    [XmlIgnore]
    public string Name { get; set; }

    /// <summary>
    /// Words count
    /// </summary>
    [XmlIgnore]
    public int WordsCount { get; set; }

    /// <summary>
    /// Max rows count
    /// </summary>
    [XmlIgnore]
    public int RowsCount { get; set; }

    /// <summary>
    /// Max columns count
    /// </summary>
    [XmlIgnore]
    public int ColumnsCount { get; set; }

    /// <summary>
    /// Horizontal words in crozzle
    /// </summary>
    [XmlIgnore]
    public int HorizontalWordsCount { get; set; }

    /// <summary>
    /// Vertical words in crozzle
    /// </summary>
    [XmlIgnore]
    public int VerticalWordsCount { get; set; }

    /// <summary>
    /// Words dictionary
    /// </summary>
    [XmlIgnore]
    public List<string> WordsDictionary { get; set; }

    /// <summary>
    /// Words to display
    /// </summary>
    [XmlIgnore]
    public List<DisplayWord> DisplayedWords { get; }

    /// <summary>
    /// Words intersecting count
    /// </summary>
    [XmlIgnore]
    public int WordIntersectingCount { get; set; }



    /// <summary>
    /// Complete loading crozzle
    /// </summary>
    public void SetLoadingComplete()
    {
      CrozzleLogger.InitLog(Name, true);

      CreateMatrix();
      FillMatrix();
      CalculateScore();
      _loaded = true;

      CrozzleLogger.EndLog(Name, true, ValidationResult.IsValid);
    }

    /// <summary>
    /// Is Crozzle loaded?
    /// </summary>
    /// <returns>true if yes, otherwise - false</returns>
    private bool Loaded()
    {
      return _loaded;
    }

    /// <summary>
    /// Create matrix for html serizlization
    /// </summary>
    private void CreateMatrix()
    {
      for (int i = 0; i < this.RowsCount; i++)
      {
        var row = new Row();
        for (int j = 0; j < this.ColumnsCount; j++)
        {
          var column = new Cell
          {
            RowIndex = i,
            ColumnIndex = j,
            Character = string.Empty
          };

          row.Add(column);
        }

        Rows.Add(row);
      }
    }

    /// <summary>
    /// Calculate score
    /// </summary>
    private void CalculateScore()
    {
      if (ValidationResult.ConfigError)
      {
        return;
      }

      Score = 0;
      var cells = Rows.SelectMany(row => row.Cells).ToList();
      var points = _config.PointPerLetters;
      var intersectingCells = cells.Where(cell => cell.IsIntersecting()).GroupBy(cell => cell.Letter).ToList();
      var noneintersectingCells = cells.Where(cell => !cell.IsIntersecting()).GroupBy(cell => cell.Letter).ToList();

      Score += _config.PointsPerWord * DisplayedWords.Count;

      foreach (var point in points)
      {
        if (point.PointType == LetterPointType.Intersecting)
        {
          var intGroup = intersectingCells.FirstOrDefault(group => group.Key == point.Letter);
          if (intGroup != null)
          {
            Score += intGroup.Count() * point.Point;
          }
        }
        if (point.PointType == LetterPointType.Nonintersecting)
        {
          var intGroup = noneintersectingCells.FirstOrDefault(group => group.Key == point.Letter);
          if (intGroup != null)
          {
            Score += intGroup.Count() * point.Point;
          }
        }
      }
    }

    /// <summary>
    /// Fill matrix with values
    /// </summary>
    private void FillMatrix()
    {
      var allCells = Rows.SelectMany(row => row.Cells).ToList();

      foreach (var wordModelObject in DisplayedWords)
      {
        List<Cell> cells = null;

        var orientation = wordModelObject.Orientation;
        var hPosition = wordModelObject.HorizontalPosition;
        var vPosition = wordModelObject.VerticalPosition;
        var orientationPostition = 0;
        var word = wordModelObject.Word;
        var startPosition = 0;
        var endPosition = 0;
        if (orientation == WordOrientation.Horizontal)
        {
          startPosition = vPosition;
          endPosition = vPosition + word.Length;
          orientationPostition = hPosition;
          cells = allCells.Where(cell => cell.ColumnIndex >= startPosition && cell.ColumnIndex < endPosition && cell.RowIndex == hPosition).ToList();
        }
        if (orientation == WordOrientation.Vertical)
        {
          startPosition = hPosition;
          endPosition = hPosition + word.Length;
          orientationPostition = vPosition;
          cells = allCells.Where(cell => cell.RowIndex >= startPosition && cell.RowIndex < endPosition && cell.ColumnIndex == vPosition).ToList();
        }

        for (int i = 0; i < word.Length; i++)
        {
          var cell = cells[i];
          var character = word[i];
          cell.Character = character.ToString();
          cell.EntryCount++;
          cell.Words.Add(wordModelObject);
        }

        var delimeted = allCells.DelimetedBySpaces(startPosition, endPosition, orientationPostition, orientation);
        var orientationIsOk = Constraints.WordOrientationIsCorrect(startPosition, endPosition);
        var intersectingIsOk = allCells.IntersectingConstarint(DifficultyLevel);


        if (!delimeted)
        {
          ValidationResult.AddError($"Word {wordModelObject} has not enough spaces");
          CrozzleLogger.Log($"Word {wordModelObject} has not enough spaces");
        }
        if (!orientationIsOk)
        {
          ValidationResult.AddError($"Word {wordModelObject} has problem with orientation");
          CrozzleLogger.Log($"Word {wordModelObject} has not enough spaces");
        }
        if (!intersectingIsOk)
        {
          ValidationResult.AddError($"Words intersection count is invalid difficulty level - {DifficultyLevel}");
          CrozzleLogger.Log($"Word {wordModelObject} has not enough spaces");
        }
      }
    }


    /// <summary>
    /// Set config for crozzle
    /// </summary>
    /// <param name="config">Configuration <see cref="IConfiguration"/></param>
    public void SetConfig(IConfiguration config)
    {
      _config = config;
    }
    
    /// <summary>
    /// Serialize to html
    /// </summary>
    /// <returns>html</returns>
    public string SerializeToHtml()
    {
      if (!Loaded())
      {
        throw new ModelLoadException($"Cant serialize to html, {nameof(Crozzle)} is not loaded");
      }

      var engine = Assembly.GetExecutingAssembly();
      var templateName = engine.GetManifestResourceNames().First(file => file.Contains(HTML_TEMPLATE_NAME));
      string xslt;
      string html;

      using (var ebeddedResource = new StreamReader(engine.GetManifestResourceStream(templateName)))
      {
        xslt = ebeddedResource.ReadToEnd();
      }

      using (var htmlStream = new MemoryStream())
      using (var xmlStream = new MemoryStream())
      using (var xslStream = new MemoryStream(Encoding.UTF8.GetBytes(xslt)))
      {
        var xmlSerizlizer = new XmlSerializer(typeof(Crozzle));
        xmlSerizlizer.Serialize(xmlStream, this);
        xmlStream.Position = 0;
        var xslReader = XmlReader.Create(xslStream);
        var xmlReader = XmlReader.Create(xmlStream);
        var t = new XslCompiledTransform(true);
        t.Load(xslReader);
        t.Transform(xmlReader, new XsltArgumentList(), htmlStream);
        htmlStream.Position = 0;
        var htmlReader = new StreamReader(htmlStream);
        html = htmlReader.ReadToEnd();
      }

      return html;
    }

    #endregion
  }
}
