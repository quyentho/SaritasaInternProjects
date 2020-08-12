using JIRADayIssues.Model;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace JIRADayIssues.Service
{
    /// <summary>
    /// Converts response from API requests to appropriate value.
    /// </summary>
    class ResponseHandling
    {
        /// <summary>
        /// Deserialize response to get required value.
        /// </summary>
        /// <param name="response">Response from API request</param>
        /// <returns>Response object.</returns>
        public ResponseObject DeserializeResponse(IRestResponse response)
        {
            return JsonConvert.DeserializeObject<ResponseObject>(response.Content);
        }

        /// <summary>
        /// Format and display result from API response.
        /// </summary>
        /// <param name="responseObject">Object to display.</param>
        public void DisplayResponse(ResponseObject responseObject)
        {
            foreach (var issue in responseObject.Issues)
            {
                Console.WriteLine(issue.Field.Project.Name);
                var timeSpent = TimeSpan.FromSeconds(Convert.ToDouble(issue.Field.TimeSpent));
                var estimateTime = TimeSpan.FromSeconds(Convert.ToDouble(issue.Field.EstimateTime));
                Console.WriteLine(string.Format("{0,40} {1:0h}/{2:0h}", issue.Field.Summary, timeSpent.TotalHours, estimateTime.TotalHours));
            }
        }
    }
}
