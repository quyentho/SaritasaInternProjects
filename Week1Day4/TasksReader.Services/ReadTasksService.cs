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
    /// Read tasks service.
    /// </summary>
    public class ReadTasksService : IReadTasksService
    {
        /// <summary>
        /// Read data from csv file.
        /// </summary>
        /// <returns>List of tasks.</returns>
        public List<TaskItem> ReadFromFile()
        {
            using (var reader = new StreamReader(ConfigurationManager.AppSettings["Path"]))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.BadDataFound = null;
                csv.Configuration.Delimiter = ";";
                csv.Configuration.RegisterClassMap(new TaskItemMap());

                var tasks = new List<TaskItem>();
                tasks = csv.GetRecords<TaskItem>().ToList<TaskItem>();
                return tasks;
            }
        }
    }
}
