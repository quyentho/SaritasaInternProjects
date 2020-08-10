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
            var readTasksService = new ReadTasksService();
            var tasks = readTasksService.ReadFromFile();

            CachedFindTasksService cachedFindTasksService = new CachedFindTasksService(new FindTasksService());

<<<<<<< HEAD
            AdditionalService additionalService = new TaskReaderCachingService(readerService);
            additionalService.ReadFromFile();
=======
>>>>>>> 62dfcef360ad46131d7bf4328ad8c7c509b431a0
            Console.WriteLine("Press Ctrl + C to exit");
            Console.WriteLine("Enter D to see 10 last searched items");
            do
            {
                string input = Console.ReadLine();
                SearchResult searchResult = new SearchResult();
                if (IsShowCache(input))
                {
<<<<<<< HEAD
                    DisplayValidTasks((additionalService as TaskReaderCachingService).GetCachedTasks());
                    continue;
=======
                    searchResult.FoundItems = cachedFindTasksService.GetCachedTasks();
                    DisplaySearchResult(searchResult);
                }
                else
                {
                    try
                    {
                        searchResult = FindTasks(cachedFindTasksService, tasks, input);
                        DisplaySearchResult(searchResult);
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Input must be number.");
                    }
                    catch (TaskNotFoundException e)
                    {
                        Console.WriteLine(e.Message);
                    }
>>>>>>> 62dfcef360ad46131d7bf4328ad8c7c509b431a0
                }
            }
            while (true);
        }

        private static SearchResult FindTasks(CachedFindTasksService cachedFindTasksService, List<TaskItem> tasks, string input)
        {
            try
            {
                var ids = cachedFindTasksService.GetIdsFromInput(input);
                var searchResult = cachedFindTasksService.FindByIds(tasks, ids);
                return searchResult;
            }
            catch
            {
                throw;
            }
        }

        private static bool IsShowCache(string input)
        {
            return input.Equals("d", StringComparison.InvariantCultureIgnoreCase);
        }

        private static void DisplaySearchResult(SearchResult searchResult)
        {
            DisplayValidTasks(searchResult.FoundItems);
            if (!(searchResult.NotFoundIds is null))
            {
                DisplayInvalidTasks(searchResult.NotFoundIds);
            }
        }

        private static void DisplayInvalidTasks(List<int> notFoundsIds)
        {
            foreach (var id in notFoundsIds)
            {
                Console.WriteLine($"{id}: [!] Task with id {id} not found");
            }
        }

        private static void DisplayValidTasks(List<TaskItem> foundItems)
        {
            foreach (var item in foundItems)
            {
                Console.WriteLine($"{item.Id},{item.Title}");
            }
        }
    }
}
