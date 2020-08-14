using System;
using System.Collections.Generic;
using System.Text;
using TasksReader.Model;

namespace TasksReader.Services
{
    /// <summary>
    /// Decorator for find task service.
    /// </summary>
    public abstract class FindTaskServiceDecorator : IFindTasksService
    {
        private readonly IFindTasksService findTaskService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FindTaskServiceDecorator"/> class.
        /// </summary>
        /// <param name="findTasksService">IFindTaskService for DI.</param>
        public FindTaskServiceDecorator(IFindTasksService findTasksService)
        {
            this.findTaskService = findTasksService;
        }

        /// <summary>
        /// Call to find task service to find by id.
        /// </summary>
        /// <param name="tasks">List of tasks.</param>
        /// <param name="ids">List of ids from user input.</param>
        /// <returns>SearchResult object.</returns>
        public SearchResult FindByIds(List<TaskItem> tasks, List<int> ids)
        {
            SearchResult result = this.findTaskService.FindByIds(tasks, ids);

            this.Decorate(result);
            return result;
        }

        /// <summary>
        /// Call to find task service to get list id.
        /// </summary>
        /// <param name="input">User input.</param>
        /// <returns>List of ids.</returns>
        public List<int> GetIdsFromInput(string input)
        {
            return this.findTaskService.GetIdsFromInput(input);
        }

        /// <summary>
        /// Add functionality after find tasks.
        /// </summary>
        /// <param name="result">Search result.</param>
        public abstract void Decorate(SearchResult result);

    }
}
