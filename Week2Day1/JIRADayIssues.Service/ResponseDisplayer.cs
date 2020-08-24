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
    /// Display Response get from API request.
    /// </summary>
    public class ResponseDisplayer
    {
        /// <summary>
        /// Display Response in correct format, announce back to console if response is null.
        /// </summary>
        /// <param name="jiraIssuesResponse">Object to display.</param>
        public void DisplayResponse(JiraIssueResponse jiraIssuesResponse)
        {
            if (jiraIssuesResponse.Issues == null)
            {
                Console.WriteLine("No issue found.");
                return;
            }

            string content = this.GetContent(jiraIssuesResponse);

            Console.WriteLine(content);
        }

        private string GetContent(JiraIssueResponse jiraIssuesResponse)
        {
            var stringBuilder = new StringBuilder();

            foreach (var issue in jiraIssuesResponse.Issues)
            {
                stringBuilder.AppendLine(issue.Field.Project.Name);

                var timeSpent = TimeSpan.FromSeconds(issue.Field.TimeSpent);

                var estimateTime = TimeSpan.FromSeconds(issue.Field.EstimateTime);

                stringBuilder.AppendFormat("{0,40} {1:0h}/{2:0h}", issue.Field.Summary, timeSpent.TotalHours, estimateTime.TotalHours);
                stringBuilder.AppendLine("\n");
            }

            TimeSpan totalTime = TimeSpan.FromSeconds(jiraIssuesResponse.Issues.Sum(s => s.Field.TimeSpent));

            stringBuilder.AppendFormat("Total {0:0h} {1:0m}", totalTime.Hours, totalTime.Minutes);

            return stringBuilder.ToString();
        }
    }
}
