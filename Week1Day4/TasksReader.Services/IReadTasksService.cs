// Copyright (c) Saritasa, LLC

using System.Collections.Generic;

namespace TasksReader.Services
{
    /// <summary>
    /// Read tasks service interface.
    /// </summary>
    public interface IReadTasksService
    {
        /// <summary>
        /// Read data from csv file.
        /// </summary>
        /// <returns>List of tasks.</returns>
        List<TaskItem> ReadFromFile();
    }
}
