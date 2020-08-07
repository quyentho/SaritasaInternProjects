// Copyright (c) Saritasa, LLC

namespace TasksReader.Services
{
    using System.Collections.Generic;
    using TasksReader.Model;

    /// <summary>
    /// Decorator class.
    /// </summary>
    public abstract class AdditionalService : ITasksReaderService
    {
        private readonly ITasksReaderService tasksReaderService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdditionalService"/> class.
        /// </summary>
        /// <param name="tasksReaderService">A task reader service.</param>
        public AdditionalService(ITasksReaderService tasksReaderService)
        {
            this.tasksReaderService = tasksReaderService;
        }

        /// <summary>
        /// Gets or sets list task item.
        /// </summary>
        public List<TaskItem> Tasks { get; set; }

        /// <summary>
        /// Find task by list id.
        /// </summary>
        /// <param name="input">User input.</param>
        /// <returns>SearchResult object.</returns>
        public SearchResult FindByIds(string input)
        {
            return this.tasksReaderService.FindByIds(input);
        }

        /// <summary>
        /// Read data from file.
        /// </summary>
        public void ReadFromFile()
        {
            this.tasksReaderService.ReadFromFile();
        }
    }
}
