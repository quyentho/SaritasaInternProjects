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
    public class FindTasksService : IFindTasksService
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
        /// Find tasks by ids. All input not found task, throw TaskNotFoundException.
        /// </summary>
        /// <param name="tasks">List of tasks.</param>
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

            result.NotFoundIds = this.GetNotFoundIds(ids, result);

            return result;
        }

        private List<int> GetNotFoundIds(List<int> ids, SearchResult result)
        {
            IEnumerable<int> foundIds = result.FoundItems.Select(t => t.Id);
            return ids.Except(foundIds).ToList();
        }
    }
}
