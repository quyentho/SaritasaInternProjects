// Copyright (c) Saritasa, LLC

namespace TasksReader.Services
{
    using System.Collections.Generic;
    using TasksReader.Model;

    /// <summary>
    /// Declare functionality for business layer.
    /// </summary>
    public interface IReadFileService
    {
        /// <summary>
        /// Read data from file.
        /// </summary>
        /// <returns>List of tasks.</returns>
        List<TaskItem> ReadFromFile();
    }
}
