﻿// Copyright (c) Saritasa, LLC

using System.Collections.Generic;
using TasksReader.Model;

namespace TasksReader.Services
{
    /// <summary>
    /// Decorator to caching service.
    /// </summary>
    public class CachedFindTasksService : IFindTasksService
    {
        private readonly IFindTasksService tasksReaderService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedFindTasksService"/> class.
        /// </summary>
        /// <param name="tasksReaderService">Service inject from outside.</param>
        public CachedFindTasksService(IFindTasksService tasksReaderService)
        {
            this.tasksReaderService = tasksReaderService;
        }

        /// <summary>
        /// Gets or sets cached items.
        /// </summary>
        public List<TaskItem> CachedItems { get; set; } = new List<TaskItem>();

        /// <summary>
        /// Cached tasks 10 recent found tasks.
        /// </summary>
        /// <param name="tasks">List of tasks.</param>
        /// <param name="ids">List of ids from user input.</param>
        /// <returns>SearchResult object.</returns>
        public SearchResult FindByIds(List<TaskItem> tasks, List<int> ids)
        {
            var result = this.tasksReaderService.FindByIds(tasks, ids);

            this.CacheItems(result);

            return result;
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

        /// <summary>
        /// Split input into list of ids.
        /// </summary>
        /// <param name="input">User input.</param>
        /// <returns>List of ids.</returns>
        public List<int> GetIdsFromInput(string input)
        {
            return this.tasksReaderService.GetIdsFromInput(input);
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