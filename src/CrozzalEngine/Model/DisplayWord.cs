namespace CrozzleEngine.Model
{
  /// <summary>
  /// Display word model object
  /// </summary>
  public class DisplayWord
  {
    private const int SUPPERS_VALUE_ZERO_INDEX = 1;

    private int _hPostion;
    private int _vPostion;

    /// <summary>
    /// Word orientation
    /// </summary>
    public WordOrientation Orientation { get; set; }

    /// <summary>
    /// Row position
    /// </summary>
    public int HorizontalPosition
    {
      get { return _hPostion; }
      set { _hPostion = value - SUPPERS_VALUE_ZERO_INDEX; } // because of 0 indexing
    }

    /// <summary>
    /// Column position
    /// </summary>
    public int VerticalPosition
    {
      get { return _vPostion; }
      set { _vPostion = value - SUPPERS_VALUE_ZERO_INDEX; } // because of 0 indexing
    }

    /// <summary>
    /// Word to view
    /// </summary>
    public string Word { get; set; }

    /// <summary>
    /// Word in array presentation to slice for cells
    /// </summary>
    /// <returns></returns>
    public char[] GetWordChars()
    {
      return Word.ToCharArray();
    }

    /// <summary>
    /// To string representation
    /// </summary>
    /// <returns>get word for model</returns>
    public override string ToString()
    {
      return Word;
    }
  }
}
