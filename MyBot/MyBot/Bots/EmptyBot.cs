// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Options;
using MyBot.Dtos;
using MyBot.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace MyBot.Bots
{
    public class EmptyBot : ActivityHandler
    {
        private readonly BotState _userState;
        private readonly BotState _conversationState;
        private readonly IOptions<Credentials> _credentials;


        public EmptyBot(ConversationState conversationState, UserState userState, IOptions<Credentials> credentials)
        {
            _conversationState = conversationState;
            _userState = userState;
            _credentials = credentials;
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            await turnContext.SendActivityAsync("Hello! Type any key to continue", null, null, cancellationToken);
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            // Make request.
            AuthenticationResultDto queryResult = await CallAuthenticationApi(cancellationToken);

            UserDto userDto = await CallGetMyInfoApi(queryResult.AccessToken, cancellationToken);

            await turnContext.SendActivityAsync($"Your name is {userDto.Name}", null, null, cancellationToken);
            await turnContext.SendActivityAsync($"Your department is {userDto.DepartmentName}", null, null, cancellationToken);
        }

        private async Task<AuthenticationResultDto> CallAuthenticationApi(CancellationToken cancellationToken)
        {
            var client = new RestClient("https://crm.saritasa.com/");
            var request = new RestRequest("oauth/Token", Method.POST);

            request.AddJsonBody(new Dictionary<string, object>
            {
                ["username"] = _credentials.Value.Username,
                ["password"] = _credentials.Value.Password
            });

            var response = await client.ExecuteAsync(request, cancellationToken);

            return JsonConvert.DeserializeObject<AuthenticationResultDto>(response.Content);
        }

        //private async Task ControlConversationFLow(ConversationFlow flow, Credentials credentials, ITurnContext turnContext, CancellationToken cancellationToken)
        //{
        //    var input = turnContext.Activity.Text;

        //    switch (flow.LastQuestionAsked)
        //    {
        //        case ConversationFlow.Question.None:
        //            await turnContext.SendActivityAsync("Please provide your username?", null, null, cancellationToken);
        //            flow.LastQuestionAsked = ConversationFlow.Question.Username;
        //            break;
        //        case ConversationFlow.Question.Username:
        //            if (!string.IsNullOrEmpty(input))
        //            {
        //                credentials.Username = input;

        //                await turnContext.SendActivityAsync("Please provide your password?", null, null, cancellationToken);

        //                flow.LastQuestionAsked = ConversationFlow.Question.Password;
        //                break;
        //            }
        //            else
        //            {
        //                await turnContext.SendActivityAsync("Username cannot be null.", null, null, cancellationToken);
        //                break;
        //            }
        //        case ConversationFlow.Question.Password:
        //            if (!string.IsNullOrEmpty(input))
        //            {
        //                credentials.Password = input;

        //                // Make request.
        //                AuthenticationResultDto queryResult = await CallAuthenticationApi(cancellationToken);

        //                if (queryResult.Success)
        //                {
        //                    await turnContext.SendActivityAsync($"Your access token is {queryResult.AccessToken}", null, null, cancellationToken);

        //                    UserDto userDto = await CallGetMyInfoApi(queryResult.AccessToken,cancellationToken);

        //                    await turnContext.SendActivityAsync($"Your name is {userDto.Name}", null, null, cancellationToken);
        //                    await turnContext.SendActivityAsync($"Your department is {userDto.DepartmentName}", null, null, cancellationToken);
        //                }
        //                else
        //                {
        //                    await turnContext.SendActivityAsync($"{queryResult.Error}", null, "NotAcceptingInput", cancellationToken);
        //                }

        //                // restart state.
        //                await turnContext.SendActivityAsync("Type any key to start a new conversation.", null, null, cancellationToken);

        //                flow.LastQuestionAsked = ConversationFlow.Question.None;

        //                credentials = new Credentials();
        //                break;
        //            }
        //            else
        //            {
        //                await turnContext.SendActivityAsync("Password cannot be null.", null, null, cancellationToken);
        //                break;
        //            }
        //    }
        //}

        private async Task<UserDto> CallGetMyInfoApi(string queryResultAccessToken, CancellationToken cancellationToken)
        {
            var client = new RestClient("https://crm.saritasa.com/");
            var request = new RestRequest("api/users/Me", Method.GET);

            client.AddDefaultHeader("Authorization", string.Format($"Bearer {queryResultAccessToken}"));

            var response = await client.ExecuteAsync(request, cancellationToken);

            return JObject.Parse(response.Content.ToString()).SelectToken("data").ToObject<UserDto>();
        }
    }
}
