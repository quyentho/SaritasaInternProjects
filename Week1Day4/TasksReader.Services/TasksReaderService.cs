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
        /// Gets or sets list of task item.
        /// </summary>
        public List<TaskItem> Tasks { get; set; }

        /// <summary>
        /// Read data from csv file.
        /// </summary>
        public void ReadFromFile()
        {
            using (var reader = new StreamReader(ConfigurationManager.AppSettings["Path"]))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.BadDataFound = null;
                csv.Configuration.Delimiter = ";";
                csv.Configuration.RegisterClassMap(new TaskItemMap());

                this.Tasks = new List<TaskItem>();
                this.Tasks = csv.GetRecords<TaskItem>().ToList<TaskItem>();
            }
        }

        /// <summary>
        /// Find task by list id.
        /// </summary>
        /// <param name="input">User input.</param>
        /// <returns>SearchResult object.</returns>
        public SearchResult FindByIds(string input)
        {
            List<int> listOfIds = this.GetIdsFromInput(input);

            SearchResult result = new SearchResult();

            result.FoundItems = this.Tasks.Where(t => listOfIds.Contains(t.Id)).ToList();

            IEnumerable<int> foundIds = result.FoundItems.Select(t => t.Id);
            result.NotFoundIds = listOfIds.Except(foundIds).ToList();

            return result;
        }

        private List<int> GetIdsFromInput(string input)
        {
            return input.Split(",", System.StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
        }
    }
}
