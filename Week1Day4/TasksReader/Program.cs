// Copyright (c) Saritasa, LLC

namespace TasksReader
{
    using System;
    using System.Collections.Generic;
    using TasksReader.Services;

    /// <summary>
    /// Application entry point.
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            var readerService = new TasksReaderService();
            List<TaskItem> tasks = readerService.ReadFromFile();
        }
    }
}
