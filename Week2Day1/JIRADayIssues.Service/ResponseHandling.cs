﻿using System;
using System.Linq;
using JiraDayIssues.Model;
using Newtonsoft.Json;
using RestSharp;

namespace JiraDayIssues.Service
{
    /// <summary>
    /// Converts response from API requests to appropriate value.
    /// </summary>
    public class ResponseHandling
    {
        /// <summary>
        /// Deserialize response to get required value.
        /// </summary>
        /// <param name="response">Response from API request.</param>
        /// <returns>Response object.</returns>
        public ResponseObject DeserializeResponse(IRestResponse response)
        {
            return JsonConvert.DeserializeObject<ResponseObject>(response.Content, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        /// <summary>
        /// Format and display result from API response.
        /// </summary>
        /// <param name="responseObject">Object to display.</param>
        public void DisplayResponse(ResponseObject responseObject)
        {
            TimeSpan totalTime = TimeSpan.FromSeconds(responseObject.Issues.Sum(s => s.Field.TimeSpent));

            foreach (var issue in responseObject.Issues)
            {
                Console.WriteLine(issue.Field.Project.Name);
                var timeSpent = TimeSpan.FromSeconds(issue.Field.TimeSpent);
                var estimateTime = TimeSpan.FromSeconds(issue.Field.EstimateTime);
                Console.WriteLine(string.Format("{0,40} {1:0h}/{2:0h}", issue.Field.Summary, timeSpent.TotalHours, estimateTime.TotalHours));
                Console.WriteLine("\n");
            }

            if (responseObject.Issues.Count > 0)
            {
                Console.WriteLine(string.Format("Total {0:0h} {1:0m}", totalTime.Hours, totalTime.Minutes));
            }
        }

        public void DisplayWorklog(ResponseObject responseObject)
        {
            foreach (var worklog in responseObject.Worklogs)
            {
                Console.WriteLine($"Comment: {worklog.Comment}");
                Console.WriteLine($"Time spent: {worklog.TimeSpent}");
                Console.WriteLine();
            }
        }
    }
}
