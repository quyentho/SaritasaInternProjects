using System;
using System.Collections.Generic;
using System.Text;

namespace TasksReader.Model
{
    public class SearchResult
    {
        public List<int> NotFoundIds { get; set; }

        public List<TaskItem> FoundItems { get; set; }
    }
}
