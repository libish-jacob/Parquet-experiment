using Parquet;
using Parquet.Data;
using Parquet_Experiment.Collection;
using Parquet_Experiment.DataStructure;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parquet_Experiment
{
  internal static class ParquetHelper
  {
    private static string fileName = "test.parquet";
    private static string fileName2 = "serialised-test.parquet";

    public static void WriteTest(string location)
    {
      EmployeeCollection col = new EmployeeCollection();
      IList<int> ids = col.GetEmployeeIds();

      var IdKey = new Parquet.Data.DataColumn(
                  new DataField<Int32>("Id"),
                  ids.ToArray()
              );

      IList<string> Names = col.GetEmployeeNames();
      var NameKey = new Parquet.Data.DataColumn(
                  new DataField<string>("Name"),
                  Names.ToArray()
              );

      IList<string> Categorys = col.GetEmployeeCategorys();
      var CategoryKey = new Parquet.Data.DataColumn(
                  new DataField<string>("Category"),
                  Categorys.ToArray()
              );

      var schema = new Schema(IdKey.Field, NameKey.Field, CategoryKey.Field);

      // if the file does not exists then it throws an error.
      using (var fileStream = new FileStream(Path.Combine(location, fileName), FileMode.OpenOrCreate))
      {
        ParquetOptions parquetOptions = new ParquetOptions();// { TreatByteArrayAsString = true };

        // Append every time.
        using (var parquetWriter = new ParquetWriter(schema, fileStream))
        {
          // snappy is the default one. We are setting it to show that this is how it is done.
          parquetWriter.CompressionMethod = CompressionMethod.Snappy;
          parquetWriter.CompressionLevel = 8;

          //parquetWriter.CompressionLevel = 2;
          using (ParquetRowGroupWriter groupWriter = parquetWriter.CreateRowGroup())
          {
            groupWriter.WriteColumn(IdKey);
            groupWriter.WriteColumn(NameKey);
            groupWriter.WriteColumn(CategoryKey);
          }
        }
      }
    }

    public static void ReadTest(string location)
    {
      using (Stream fileStream = System.IO.File.OpenRead(Path.Combine(location, fileName)))
      {
        using (var parquetReader = new ParquetReader(fileStream))
        {
          DataField[] dataFields = parquetReader.Schema.GetDataFields();

          for (int i = 0; i < parquetReader.RowGroupCount; i++)
          {
            using (ParquetRowGroupReader groupReader = parquetReader.OpenRowGroupReader(i))
            {
              DataColumn[] columns = dataFields.Select(groupReader.ReadColumn).ToArray();
              DataColumn firstColumn = columns[0];
              Array data = firstColumn.Data;
              int[] ids = (int[])data;

              DataColumn firstColumn2 = columns[1];
              data = firstColumn2.Data;
              string[] namess = (string[])data;

              DataColumn firstColumn3 = columns[2];
              data = firstColumn3.Data;
              string[] categoriess = (string[])data;
            }
          }
        }
      }
    }

    public static void SerializeAndSaveTest(string location)
    {
      EmployeeCollection col = new EmployeeCollection();

      // if the file does not exists then it throws an error.
      using (var fileStream = new FileStream(Path.Combine(location, fileName2), FileMode.OpenOrCreate))
      {
        ParquetConvert.Serialize(col.EmployeeCollections, fileStream, append: true);
      }
    }

    public static Employee[] DerializeTest(string location)
    {
      using (var fileStream = new FileStream(Path.Combine(location, fileName2), FileMode.Open))
      {
        // though it is a collection that we have serialised, just mention the type to deserialize.
        var data = ParquetConvert.Deserialize<Employee>(fileStream);
        return data;
      }
    }

    //public static void WriteTableTest(string location)
    //{
    //  EmployeeCollection col = new EmployeeCollection();

    //  IList<int> ids = col.GetEmployeeIds();
    //  IList<string> Names = col.GetEmployeeNames();
    //  IList<string> Categorys = col.GetEmployeeCategorys();

    //  var schema = new Schema(IdKey.Field, NameKey.Field, CategoryKey.Field);

    //  Parquet.Data.Rows.Table tb = new Parquet.Data.Rows.Table();
    //  for (int i = 0; i < ids.Count; i++)
    //  {
    //    tb.Add(ids[i], Names[i], Categorys[i]);
    //  }

    //  // if the file does not exists then it throws an error.
    //  using (var fileStream = new FileStream(Path.Combine(location, fileName), FileMode.OpenOrCreate))
    //  {
    //    ParquetOptions op = new ParquetOptions() { };

    //    // Append every time.
    //    using (var parquetWriter = new ParquetWriter(schema, fileStream))
    //    {
    //      // snappy is the default one. We are setting it to show that this is how it is done.
    //      parquetWriter.CompressionMethod = CompressionMethod.Snappy;

    //      //parquetWriter.CompressionLevel = 2;
    //      using (ParquetRowGroupWriter groupWriter = parquetWriter.CreateRowGroup())
    //      {
    //        groupWriter.Write(tb);
    //      }
    //    }
    //  }
    //}

    public static void LinqReadTest(string location)
    {
      using (Stream fileStream = System.IO.File.OpenRead(Path.Combine(location, fileName)))
      {
        ParquetOptions parquetOptions = new ParquetOptions() { TreatByteArrayAsString = true };
        using (var parquetReader = new ParquetReader(fileStream))
        {
          DataField[] dataFields = parquetReader.Schema.GetDataFields();

          for (int i = 0; i < parquetReader.RowGroupCount; i++)
          {
            using (ParquetRowGroupReader groupReader = parquetReader.OpenRowGroupReader(i))
            {
              DataColumn[] columns = dataFields.Select(groupReader.ReadColumn).ToArray();
              DataColumn firstColumn = columns[0];
              Array data = firstColumn.Data;
              int[] ids = (int[])data;

              DataColumn firstColumn2 = columns[1];
              data = firstColumn2.Data;
              string[] namess = (string[])data;

              DataColumn firstColumn3 = columns[2];
              data = firstColumn3.Data;
              string[] categoriess = (string[])data;


            }
          }
        }
      }
    }

    //public void Query()
    //{
    //  //Parquet.Thrift.Encoding.
    //}
  }
}
