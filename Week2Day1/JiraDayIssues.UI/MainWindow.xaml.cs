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
        private List<Issue> _issues;
        private JiraApiClient _jiraApiClient;
        private CancellationTokenSource _cancellationTokenSource;

        private string _username;
        private string _token;

        public MainWindow()
        {

            _username = Prompt.GetString("Please provide your username:");
            _token = Prompt.GetPassword("Please provide your token:");

            if (string.IsNullOrWhiteSpace(_username) || string.IsNullOrWhiteSpace(_token))
            {
                MessageBox.Show("You must provide correct username and token to use application.");
                Environment.Exit(0);
            }
            
            this._jiraApiClient = new JiraApiClient(new RestClient(), this._username, this._token);
            
            InitializeComponent();
        }

        private async void IssuesCell_MouseUp(object sender, RoutedEventArgs e)
        {
            try
            {
                this.PreProcess();

                JiraWorklogResponse worklogs = await GetWorklogsResponse();

                DisplayWorklogs(worklogs);
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

        private void DisplayWorklogs( JiraWorklogResponse responseObject)
        {
            List<Worklog> worklogs = responseObject.Worklogs;

            WorklogsGrid.ItemsSource = worklogs;
        }

        private async Task<JiraWorklogResponse> GetWorklogsResponse()
        {
            var index = IssuesGrid.SelectedIndex;


            JiraWorklogResponse response = await _jiraApiClient.GetWorklogsAsync(_issues[index].Id, _cancellationTokenSource.Token);

            return response;
        }

        private async void DateTimePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                PreProcess();

                JiraIssueResponse response = await GetIssuesResponse();


                DisplayIssues(response);
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

        private void DisplayIssues(JiraIssueResponse responseObject)
        {
            _issues = responseObject.Issues;

            IssuesGrid.ItemsSource = _issues;
        }

        private async Task<JiraIssueResponse> GetIssuesResponse()
        {
            JiraIssueResponse response = await _jiraApiClient.GetIssuesAsync(DateTimePicker.SelectedDate.GetValueOrDefault(), WorkLogAuthorTextBox.Text, _cancellationTokenSource.Token);

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

            _cancellationTokenSource = new CancellationTokenSource();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
