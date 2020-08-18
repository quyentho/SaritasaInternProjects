using JiraDayIssues.Model;
using JiraDayIssues.Service;
using McMaster.Extensions.CommandLineUtils;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace JiraDayIssues.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Issue> issues;
        private ApiManipulation apiManipulation;
        private ResponseHandling responseHandling = new ResponseHandling();
        CancellationTokenSource cancellationToken;

        private string username;
        private string token;

        public MainWindow()
        {
            this.apiManipulation = new ApiManipulation(new RestClient());

            username = Prompt.GetString("Please provide your username:");
            token = Prompt.GetPassword("Please provide your token:");

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(token))
            {
                MessageBox.Show("You must provide correct username and token to use application.");
                Environment.Exit(0);
            }

            InitializeComponent();
        }

        private async void IssuesCell_MouseUp(object sender, RoutedEventArgs e)
        {
            try
            {
                this.PreProcess();

                IRestResponse response = await GetWorklogsResponse();

                ResponseObject responseObject = responseHandling.DeserializeResponse(response);

                DisplayWorklogs(responseObject);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.PostProcess();
            }
        }

        private void DisplayWorklogs(ResponseObject responseObject)
        {
            List<Worklog> worklogs = responseObject.Worklogs;

            WorklogsGrid.ItemsSource = worklogs;
        }

        private async Task<IRestResponse> GetWorklogsResponse()
        {
            var index = IssuesGrid.SelectedIndex;

            IRestRequest request = apiManipulation.ConfigureWorklogsRequest(issues[index].Id);

            IRestResponse response = await apiManipulation.GetResponseAsync(request, username, token, cancellationToken.Token);
            
            return response;
        }

        private async void DateTimePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                PreProcess();

                IRestResponse response = await GetIssuesResponse();

                var responseObject = responseHandling.DeserializeResponse(response);

                DisplayIssues(responseObject);
            }
            catch (JsonReaderException)
            {
                MessageBox.Show("Wrong credentials!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                PostProcess();
            }
        }

        private void DisplayIssues(ResponseObject responseObject)
        {
            issues = responseObject.Issues;

            IssuesGrid.ItemsSource = issues;
        }

        private async Task<IRestResponse> GetIssuesResponse()
        {
            IRestRequest request = apiManipulation.ConfigureIssuesRequest(DateTimePicker.SelectedDate.GetValueOrDefault());

            IRestResponse response = await apiManipulation.GetResponseAsync(request, username, token, cancellationToken.Token);

            return response;
        }

        private void PostProcess()
        {
            loadingLabel.Visibility = Visibility.Hidden;
            cancelButton.IsEnabled = false;
        }

        private void PreProcess()
        {

            loadingLabel.Visibility = Visibility.Visible;
            cancelButton.IsEnabled = true;

            cancellationToken = new CancellationTokenSource();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            cancellationToken.Cancel();
        }
    }
}
