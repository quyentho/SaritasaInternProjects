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
            do
            {
                var readerService = new TasksReaderService();

                AdditionalService additionalService = new TaskReaderCachingService(readerService);
                additionalService.ReadFromFile();

                string input = Console.ReadLine();

                var searchResult = additionalService.FindByIds(input);

                Display(searchResult);
            } while (true);
        }

        private static void Display(SearchResult searchResult)
        {
            foreach (var item in searchResult.FoundItems)
            {
                Console.WriteLine($"{item.Id},{item.Title}");
            }

            foreach (var id in searchResult.NotFoundIds)
            {
                Console.WriteLine($"{id}: [!] Task with id {id} not found");
            }
        }
    }
}
