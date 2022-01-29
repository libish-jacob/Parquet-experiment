using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parquet_Experiment.DataStructure
{
  public class Catagory
  {
    public const string A = "A";
    public const string B = "B";
    public const string C = "C";
    public const string D = "D";

    public string GetRandom()
    {
      var fields = this.GetType().GetFields();
      var selectedId = Random.Shared.Next(fields.Length - 1);

      return (string)fields[selectedId].GetValue(null);
    }
  }
  internal class Employee
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Catagory { get; set; }
  }
}
