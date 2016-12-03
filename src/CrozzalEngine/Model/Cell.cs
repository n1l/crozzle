using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CrozzleEngine.Model
{
  /// <summary>
  /// Cell model object for result matrix
  /// </summary>
  [XmlType(TypeName = "cell")]
  public class Cell
  {
    private const char NOT_A_LETTER = '0';

    /// <summary>
    /// Ctor
    /// </summary>
    public Cell()
    {
      EnoughSpace = true;
      Words = new List<DisplayWord>();
    }

    /// <summary>
    /// Row index in matrix
    /// </summary>
    [XmlIgnore]
    public int RowIndex { get; set; }

    /// <summary>
    /// Column index in matrix
    /// </summary>
    [XmlIgnore]
    public int ColumnIndex { get; set; }

    /// <summary>
    /// Score for this letter
    /// </summary>
    [XmlIgnore]
    public int Score { get; set; }

    /// <summary>
    /// Intersecting words
    /// </summary>
    [XmlIgnore]
    public List<DisplayWord> Words { get; }

    /// <summary>
    /// Letter of cell
    /// </summary>
    [XmlIgnore]
    public char Letter
    {
      get { return CharacterIsEmpty() ? NOT_A_LETTER : Character[0]; }
    }

    /// <summary>
    /// Count of entry(rewriting)
    /// </summary>
    [XmlIgnore]
    public int EntryCount { get; set; }

    /// <summary>
    /// Is enogh spaces for this cell
    /// </summary>
    [XmlIgnore]
    public bool EnoughSpace { get; set; }

    /// <summary>
    /// Letter of cell for xml serialization
    /// </summary>
    [XmlText]
    public string Character { get; set; }

    /// <summary>
    /// Is cell intersecting
    /// </summary>
    /// <returns>true if yes, otherwise false</returns>
    public bool IsIntersecting()
    {
      return EntryCount > 1;
    }

    /// <summary>
    /// Is character empty
    /// </summary>
    /// <returns>true if yes, otherwise false</returns>
    public bool CharacterIsEmpty()
    {
      return string.IsNullOrEmpty(Character);
    }

    /// <summary>
    /// Get neighbor column index
    /// </summary>
    /// <param name="orientation">Orientation of search</param>
    /// <returns>column index in matrix</returns>
    public int GetNeighborColumn(NeighborOrientation orientation)
    {
      var columnIndex = ColumnIndex;
      switch (orientation)
      {
        case NeighborOrientation.Diagonal:
        case NeighborOrientation.Horizontal:
          columnIndex++;
          break;
        case NeighborOrientation.Vertical:
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null);
      }
      return columnIndex;
    }

    /// <summary>
    /// Get neibot row index
    /// </summary>
    /// <param name="orientation">Orientation of search</param>
    /// <returns>Row index in matrix</returns>
    public int GetNeighborRow(NeighborOrientation orientation)
    {
      var rowIndex = RowIndex;
      switch (orientation)
      {
        case NeighborOrientation.Diagonal:
        case NeighborOrientation.Vertical:
          rowIndex++;
          break;
        case NeighborOrientation.Horizontal:
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null);
      }
      return rowIndex;
    }
  }
}