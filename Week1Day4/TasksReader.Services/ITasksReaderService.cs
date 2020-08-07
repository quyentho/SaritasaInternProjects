// Copyright (c) Saritasa, LLC

namespace TasksReader.Services
{
    using System.Collections.Generic;
    using TasksReader.Model;

    /// <summary>
    /// Declare functionality for business layer.
    /// </summary>
    public interface ITasksReaderService
    {
        /// <summary>
        /// Gets or sets list of task item.
        /// </summary>
        public List<TaskItem> Tasks { get; set; }

        /// <summary>
        /// Read data from file.
        /// </summary>
        void ReadFromFile();

       /// <summary>
       /// Find task by list id.
       /// </summary>
       /// <param name="input">User input.</param>
       /// <returns>SearchResult object.</returns>
        SearchResult FindByIds(string input);
    }
}
