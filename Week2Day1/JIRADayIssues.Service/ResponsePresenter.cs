// <copyright file="ResponseHandling.cs" company="Saritasa, LLC">
// copyright Saritasa, LLC
// </copyright>

using System;
using System.Linq;
using System.Text;
using JiraDayIssues.Model;
using Newtonsoft.Json;
using RestSharp;

namespace JiraDayIssues.Service
{
    /// <summary>
    /// Converts response from API requests to appropriate value and display.
    /// </summary>
    public class ResponsePresenter
    {
        /// <summary>
        /// Format and display result from API response.
        /// </summary>
        /// <param name="jiraResponse">Object to display.</param>
        public void DisplayResponse(JiraIssueResponse jiraResponse)
        {
            if (jiraResponse.Issues == null)
            {
                Console.WriteLine("No issue found.");
                return;
            }

            TimeSpan totalTime = TimeSpan.FromSeconds(jiraResponse.Issues.Sum(s => s.Field.TimeSpent));

            StringBuilder stringBuilder = new StringBuilder();

            foreach (var issue in jiraResponse.Issues)
            {
                stringBuilder.AppendLine(issue.Field.Project.Name);
                var timeSpent = TimeSpan.FromSeconds(issue.Field.TimeSpent);
                var estimateTime = TimeSpan.FromSeconds(issue.Field.EstimateTime);

                stringBuilder.AppendFormat("{0,40} {1:0h}/{2:0h}", issue.Field.Summary, timeSpent.TotalHours, estimateTime.TotalHours);
                stringBuilder.AppendLine("\n");
            }

            Console.WriteLine(stringBuilder.ToString());
            Console.WriteLine(string.Format("Total {0:0h} {1:0m}", totalTime.Hours, totalTime.Minutes));
        }
    }
}
