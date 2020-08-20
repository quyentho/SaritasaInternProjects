using JiraDayIssues.Model;
using JiraDayIssues.Service;
using McMaster.Extensions.CommandLineUtils;
using Newtonsoft.Json;
using NLog.Time;
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
        private JiraApiClient _jiraApiClient;
        private CancellationTokenSource _cancellationTokenSource;
        private List<Issue> _cachedIssues = new List<Issue>();
        private Dictionary<string, List<Worklog>> _cachedWorklogs = new Dictionary<string, List<Worklog>>();
        public MainWindow()
        {
            // string username = Prompt.GetString("Please provide your username:");
            string username = "quyen.tho@saritasa.com";
            //string token = Prompt.GetPassword("Please provide your token:");
            string token = "muvykLqdTaYg3qbbdFmf37FE";

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(token))
            {
                MessageBox.Show("You must provide correct username and token to use application.");
                Environment.Exit(0);
            }

            _jiraApiClient = new JiraApiClient(username, token);

            InitializeComponent();
            datePicker.SelectedDate = DateTime.Today.AddDays(-1);
        }

        #region Events
        private async void IssuesCell_MouseUp(object sender, RoutedEventArgs e)
        {
            string issueId = _cachedIssues[dgIssues.SelectedIndex].Id;
            List<Worklog> worklogs = await GetWorklogs(issueId);
            if (worklogs is null)
            {
                return;
            }
            DisplayWorklogs(worklogs);
        }

        private async void btnExecute_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtWorklogAuthor.Text))
            {
                this.ClearDataGrids();
                return;
            }

            if (_cancellationTokenSource != null) // This times click to cancel.
            {
                _cancellationTokenSource.Cancel();
                return;
            }

            await GetIssues();
           
            await CacheWorklogs(); // BUG: Task will be canceled if click the get button when it running in background.
        }                          // Fix idea: allow to cancel and handle the exception occur by logging.

        private async Task CacheWorklogs()
        {
            foreach (var issue in _cachedIssues)
            {
                List<Worklog> worklogs = await GetWorklogs(issue.Id);
                _cachedWorklogs.Add(issue.Id, worklogs);
            }
        }
        #endregion

        private async Task<List<Worklog>> GetWorklogs(string issueId)
        {
            try
            {
                _cancellationTokenSource = new CancellationTokenSource();
                if (_cachedWorklogs.ContainsKey(issueId))
                {
                    List<Worklog> worklogs = _cachedWorklogs[issueId];
                    return worklogs;
                }

                JiraWorklogResponse worklogResponse = 
                    await _jiraApiClient.GetWorklogsAsync(issueId, _cancellationTokenSource.Token);
                return worklogResponse.Worklogs;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            finally
            {
                PostProcess();
            }
        }

        private void DisplayWorklogs(List<Worklog> worklogs)
        {
            dgWorklog.ItemsSource = worklogs;
        }

        private void DisplayIssues(List<Issue> issues)
        {
            dgIssues.ItemsSource = issues;
        }

        private void ClearDataGrids()
        {
            dgIssues.ItemsSource = null;
            dgIssues.Items.Refresh();
            dgWorklog.ItemsSource = null;
            dgWorklog.Items.Refresh();
        }

        private async Task GetIssues()
        {
            try
            {
                PreProcess();

                JiraIssueResponse response = await _jiraApiClient.GetIssuesAsync(datePicker.SelectedDate.GetValueOrDefault(), txtWorklogAuthor.Text, _cancellationTokenSource.Token);
                _cachedIssues = response.Issues;

                DisplayIssues(_cachedIssues);
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

        private void PostProcess()
        {
            lbLoad.Visibility = Visibility.Hidden;

            btnExecute.Content = "Search";

            _cancellationTokenSource = null;
        }

        private void PreProcess()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _cachedIssues.Clear();
            _cachedWorklogs.Clear();

            lbLoad.Visibility = Visibility.Visible;

            btnExecute.Content = "Cancel";
        }
    }
}
