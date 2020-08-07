// Copyright (c) Saritasa, LLC

namespace TasksReader.Services
{

    using TasksReader.Model;
    /// <summary>
    /// 
    /// </summary>
    public interface ITasksReaderService
    {
        /// <summary>
        /// Read data from file.
        /// </summary>
        void ReadFromFile();

       /// <summary>
       /// Find task by list id.
       /// </summary>
       /// <param name="input">User input.</param>
       /// <returns>SearchResult object.</returns>
        SearchResult FindByIds(string input);
    }
}
