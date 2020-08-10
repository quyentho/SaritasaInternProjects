// Copyright (c) Saritasa, LLC

using System.Collections.Generic;
using TasksReader.Model;

namespace TasksReader.Services
{
    /// <summary>
    /// Interface for Find tasks service.
    /// </summary>
    public interface IFindTasksService
    {
       /// <summary>
       /// Finds tasks.
       /// </summary>
       /// <param name="tasks">List of tasks.</param>
       /// <param name="ids">List of ids from user input.</param>
       /// <returns>SearchResult object.</returns>
        SearchResult FindByIds(List<TaskItem> tasks, List<int> ids);

        /// <summary>
        /// Split input into list of ids.
        /// </summary>
        /// <param name="input">User input.</param>
        /// <returns>List of ids.</returns>
        List<int> GetIdsFromInput(string input);
    }
}
