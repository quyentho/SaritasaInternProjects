// Copyright (c) Saritasa, LLC

using System.Collections.Generic;
using System.Threading.Tasks;
using TasksReader.Model;

namespace TasksReader.Services
{
    /// <summary>
    /// Decorator to caching service.
    /// </summary>
    public class TaskReaderCachingService : AdditionalService
    {
        private readonly ITasksReaderService tasksReaderService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskReaderCachingService"/> class.
        /// </summary>
        /// <param name="tasksReaderService">Service inject from outside.</param>
        public TaskReaderCachingService(ITasksReaderService tasksReaderService)
            : base(tasksReaderService)
        {
        }

        /// <summary>
        /// Gets or sets list of task item.
        /// </summary>
        public List<TaskItem> Tasks { get; set; }

        public List<TaskItem> CachedItems { get; set; }

        /// <summary>
        /// Find task by list id.
        /// </summary>
        /// <param name="input">User input.</param>
        /// <returns>SearchResult object.</returns>
        public SearchResult FindByIds(string input)
        {
            var result = base.FindByIds(input);

            int numberOfItemAfterAdded = this.CachedItems.Count + result.FoundItems.Count;
            if (numberOfItemAfterAdded <= 10)
            {
                this.AddNewCachingItem(result.FoundItems);
            }
            else
            {
                this.CachedItems.RemoveRange(0, numberOfItemAfterAdded - 10);
                this.AddNewCachingItem(result.FoundItems);
            }

            return result;
        }

        private void AddNewCachingItem(List<TaskItem> items)
        {
            this.CachedItems.AddRange(items);
        }

        /// <summary>
        /// Read data from file.
        /// </summary>
        public void ReadFromFile()
        {
            base.ReadFromFile();
        }
    }
}
