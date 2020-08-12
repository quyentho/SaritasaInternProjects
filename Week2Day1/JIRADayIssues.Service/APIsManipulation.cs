using JIRADayIssues.Model;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JIRADayIssues.Service
{
    public class APIsManipulation
    {
        public IRestResponse GetResponse(DateTime date,string username, string token)
        {
            var client = new RestClient($"https://saritasa.atlassian.net/rest/api/2/search");
            client.Timeout = -1;
            client.Authenticator = new HttpBasicAuthenticator(username, token);

            var request = new RestRequest(Method.GET);
            request.AddParameter("jql", $"worklogDate = { DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd")} AND worklogAuthor = currentuser()");
            IRestResponse response = client.Execute(request);

            return response;
        }


    }
}
