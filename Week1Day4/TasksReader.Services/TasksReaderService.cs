// Copyright (c) Saritasa, LLC

namespace TasksReader.Services
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using CsvHelper;
    using TasksReader;
    using TasksReader.Model;

    /// <summary>
    /// Service for reading task.
    /// </summary>
    public class TasksReaderService : ITasksReaderService
    {
        /// <summary>
        /// Split input into list of ids.
        /// </summary>
        /// <param name="input">User input.</param>
        /// <returns>List of ids.</returns>
        public List<int> GetIdsFromInput(string input)
        {
            return input.Split(",", System.StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
        }

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

        /// <summary>
        /// Find tasks by ids. All input not found task, throw TaskNotFoundException.
        /// </summary>
        /// <param name="tasks">List of tasks</param>
        /// <param name="ids">lists of ids.</param>
        /// <returns>SearchResult object.</returns>
        public SearchResult FindByIds(List<TaskItem> tasks, List<int> ids)
        {
            SearchResult result = new SearchResult();

            result.FoundItems = tasks.Where(t => ids.Contains(t.Id)).ToList();
            if (result.FoundItems.Count <= 0)
            {
                throw new TaskNotFoundException("Task Not Found!");
            }

            IEnumerable<int> foundIds = result.FoundItems.Select(t => t.Id);
            result.NotFoundIds = ids.Except(foundIds).ToList();

            return result;
        }
    }
}
