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
            try
            {
                var readTasksService = new ReadTasksService();
                var tasks = readTasksService.ReadFromFile();

                IFindTasksService findTasksService = new FindTasksService();
                findTasksService = new CacheTasksDecorator(findTasksService);

                Console.WriteLine("Press Ctrl + C to exit");
                Console.WriteLine("Enter D to see 10 last searched items");
                do
                {
                    string input = Console.ReadLine();
                    SearchResult searchResult = new SearchResult();
                    if (IsShowCache(input))
                    {
                        searchResult.FoundItems = (findTasksService as CacheTasksDecorator).GetCachedTasks();
                        DisplaySearchResult(searchResult);
                    }
                    else
                    {
                        try
                        {
                            searchResult = FindTasks(findTasksService, tasks, input);
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
                    }
                }
                while (true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static SearchResult FindTasks(IFindTasksService cachedFindTasksService, List<TaskItem> tasks, string input)
        {
            try
            {
                List<int> ids = cachedFindTasksService.GetIdsFromInput(input);
                SearchResult searchResult = cachedFindTasksService.FindByIds(tasks, ids);
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
