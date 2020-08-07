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
        /// Gets or sets cached items.
        /// </summary>
        public List<TaskItem> CachedItems { get; set; } = new List<TaskItem>();

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

        public List<TaskItem> GetCachedTasks()
        {
            if (this.CachedItems is null)
            {
                throw new TaskNotFoundException();
            }

            return this.CachedItems;
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
