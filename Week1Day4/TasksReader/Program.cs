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

            readerService.ReadFromFile();

            string input = Console.ReadLine();

            var searchResult = readerService.FindByIds(input);

            Display(searchResult);
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
