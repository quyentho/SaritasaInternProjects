// Copyright (c) Saritasa, LLC

namespace TasksReader.Model
{
    using CsvHelper.Configuration;

    /// <summary>
    /// Mapping class for CsvHelper library.
    /// </summary>
    public sealed class TaskItemMap : ClassMap<TaskItem>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskItemMap"/> class.
        /// </summary>
        public TaskItemMap()
        {
            this.Map(m => m.Id).Name("taskId");
            this.Map(m => m.Title).Name("title");
        }
    }
}
