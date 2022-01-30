using Parquet_Experiment.DataStructure;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parquet_Experiment.Collection
{
  internal class EmployeeCollection
  {
    private int RecordSizeNeeded { get; set; } = 10000000;

    public ConcurrentBag<Employee> EmployeeCollections { get; set; } = new ConcurrentBag<Employee>();

    public EmployeeCollection(int recordSize = 0)
    {
      if (recordSize != 0)
      {
        RecordSizeNeeded = recordSize;
      }

      Parallel.For(0, RecordSizeNeeded,
                   index =>
                   {
                     int id = Random.Shared.Next(int.MaxValue);
                     Catagory ob = new();
                     var category = ob.GetRandom();
                     EmployeeCollections.Add(new Employee() { Id = id, Name = "Employee - " + id, Catagory = category });
                   });
    }

    internal IList<string> GetEmployeeCategorys()
    {
      var ids = from a in EmployeeCollections select a.Catagory;
      return ids.ToList();
    }

    internal IList<string> GetEmployeeNames()
    {
      var ids = from a in EmployeeCollections select a.Name;
      return ids.ToList();
    }

    internal IList<int> GetEmployeeIds()
    {
      var ids = from a in EmployeeCollections select a.Id;
      return ids.ToList();
    }
  }
}
