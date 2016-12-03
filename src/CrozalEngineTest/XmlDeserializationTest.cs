using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using CrozzleEngine.Model;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace CrozzleEngineTest
{
  [TestFixture]
  public class XmlDeserializationTest
  {
    [Test]
    public void GenerateCrozzleXmlTest()
    {

      var d = new XmlDocument();
      var setted = true;
      var crozzle = new Crozzle();
      for (int i = 0; i < 10; i++)
      {
        var row = new Row();
        for (int j = 0; j < 10; j++)
        {
          var column = new Cell();
          column.Character = setted ? "A" : "<![CDATA[&nbsp]]> ";
          row.Cells.Add(column);
          setted = !setted;
        }
        crozzle.Rows.Add(row);
      }


      var xmlSerizlizer = new XmlSerializer(typeof(Crozzle));
      xmlSerizlizer.Serialize(Console.Out, crozzle);
    }

    [Test]
    public void GenerateValidationXmlTest()
    {
      var result = new ValidationResult();
      for (int i = 0; i < 10; i++)
      {
        result.AddError($"ERROR: {i} row of {10} rows, please check your files");
      }


      var xmlSerizlizer = new XmlSerializer(typeof(ValidationResult));
      xmlSerizlizer.Serialize(Console.Out, result);
    }
  }
}
