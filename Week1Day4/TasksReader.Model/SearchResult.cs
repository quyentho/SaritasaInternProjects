// Copyright (c) Saritasa, LLC

namespace TasksReader.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// Result after perform searching.
    /// </summary>
    public class SearchResult
    {
        /// <summary>
        /// Gets or sets list ids not found.
        /// </summary>
        public List<int> NotFoundIds { get; set; }

        /// <summary>
        /// Gets or sets task items found.
        /// </summary>
        public List<TaskItem> FoundItems { get; set; }
    }
}
