// Copyright (c) Saritasa, LLC

namespace TasksReader.Services
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using CsvHelper;
    using TasksReader.Model;

    /// <summary>
    /// Service for reading task.
    /// </summary>
    public class TasksReaderService
    {
        /// <summary>
        /// Read data from csv file.
        /// </summary>
        /// <returns>List of task item.</returns>
        public List<TaskItem> ReadFromFile()
        {
            using (var reader = new StreamReader(ConfigurationManager.AppSettings["Path"]))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.BadDataFound = null;
                csv.Configuration.Delimiter = ";";
                csv.Configuration.RegisterClassMap(new TaskItemMap());

                List<TaskItem> records = new List<TaskItem>();
                records = csv.GetRecords<TaskItem>().ToList<TaskItem>();

                return records;
            }
        }
    }
}
