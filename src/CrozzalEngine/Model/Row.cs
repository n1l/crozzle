using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CrozzleEngine.Model
{
  /// <summary>
  /// Row nodel object
  /// </summary>
  [XmlType(TypeName = "row")]
  public class Row : ICollection<Cell>
  {
    public Row()
    {
      Cells = new List<Cell>();
    }

    /// <summary>
    /// Row cells
    /// </summary>
    [XmlIgnore]
    public List<Cell> Cells { get; }

    #region [ Implementation of ICollection ]

    public IEnumerator<Cell> GetEnumerator()
    {
      return Cells.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public void Add(Cell item)
    {
      Cells.Add(item);
    }

    public void Clear()
    {
      Cells.Clear();
    }

    public bool Contains(Cell item)
    {
      return Cells.Contains(item);
    }

    public void CopyTo(Cell[] array, int arrayIndex)
    {
      Cells.CopyTo(array, arrayIndex);
    }

    public bool Remove(Cell item)
    {
      return Cells.Remove(item);
    }

    public int Count
    {
      get { return Cells.Count; }
    }

    public bool IsReadOnly
    {
      get { return false;}
    }

    #endregion
  }
}