// See https://aka.ms/new-console-template for more information
using Parquet;
using Parquet.Data;
using Parquet_Experiment;
using Parquet_Experiment.Collection;

Console.WriteLine("Hello, World!");
var location = AppDomain.CurrentDomain.BaseDirectory;

//ParquetHelper.WriteTest(location);
//ParquetHelper.ReadTest(location);

//ParquetHelper.SerializeAndSave(location);
var res = ParquetHelper.Derialize(location);