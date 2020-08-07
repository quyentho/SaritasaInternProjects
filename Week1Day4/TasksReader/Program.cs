// Copyright (c) Saritasa, LLC

namespace TasksReader
{
    using System;
    using System.Collections.Generic;
    using TasksReader.Model;
    using TasksReader.Services;

    /// <summary>
    /// Application entry point.
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            var readerService = new TasksReaderService();

            TaskReaderCachingService additionalService = new TaskReaderCachingService(readerService);
            additionalService.ReadFromFile();
            Console.WriteLine("Press Ctrl + C to exit");
            Console.WriteLine("Enter D to see 10 last searched items");
            do
            {
                string input = Console.ReadLine();
                if (IsShowCache(input))
                {
                    DisplayValidTasks(additionalService.GetCachedTasks());
                    continue;
                }

                var searchResult = additionalService.FindByIds(input);

                DisplaySearchResult(searchResult);
            }
            while (true);
        }

        private static bool IsShowCache(string input)
        {
            return input.Equals("d", StringComparison.InvariantCultureIgnoreCase);
        }

        private static bool IsPressControlD(ConsoleKeyInfo consoleKeyInfo)
        {
            return consoleKeyInfo.Modifiers == ConsoleModifiers.Control && consoleKeyInfo.Key == ConsoleKey.D;
        }

        private static void DisplaySearchResult(SearchResult searchResult)
        {
            DisplayValidTasks(searchResult.FoundItems);
            DisplayInvalidTasks(searchResult.NotFoundIds);
        }

        private static void DisplayInvalidTasks(List<int> searchResult)
        {
            foreach (var id in searchResult)
            {
                Console.WriteLine($"{id}: [!] Task with id {id} not found");
            }
        }

        private static void DisplayValidTasks(List<TaskItem> searchResult)
        {
            foreach (var item in searchResult)
            {
                Console.WriteLine($"{item.Id},{item.Title}");
            }
        }
    }
}
