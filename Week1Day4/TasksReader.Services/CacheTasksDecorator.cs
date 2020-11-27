// Copyright (c) Saritasa, LLC

using System.Collections.Generic;
using TasksReader.Model;

namespace TasksReader.Services
{
    /// <summary>
    /// Decorator to caching service.
    /// </summary>
    public class CacheTasksDecorator : FindTaskServiceDecorator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheTasksDecorator"/> class.
        /// </summary>
        /// <param name="findTasksService">Service inject from outside.</param>
        public CacheTasksDecorator(IFindTasksService findTasksService)
            : base(findTasksService)
        {
        }

        /// <summary>
        /// Gets or sets cached items.
        /// </summary>
        public List<TaskItem> CachedItems { get; set; } = new List<TaskItem>();

        /// <summary>
        /// Caches 10 recent found tasks.
        /// </summary>
        /// <param name="result">Search result to cache.</param>
        public override void Decorate(SearchResult result)
        {
            this.CacheItems(result);
        }

        /// <summary>
        /// Gets tasks cached.
        /// </summary>
        /// <returns>List of tasks cached.</returns>
        public List<TaskItem> GetCachedTasks()
        {
            if (this.CachedItems is null)
            {
                throw new TaskNotFoundException("No cached task.");
            }

            return this.CachedItems;
        }

        private void CacheItems(SearchResult result)
        {
            int numberOfItemAfterAdded = this.CachedItems.Count + result.FoundItems.Count;
            if (numberOfItemAfterAdded > 10)
            {
                this.CachedItems.RemoveRange(0, numberOfItemAfterAdded - 10);
            }

            this.CachedItems.AddRange(result.FoundItems);
        }
    }
}
