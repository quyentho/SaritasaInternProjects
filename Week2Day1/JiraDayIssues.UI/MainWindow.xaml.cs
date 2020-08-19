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
        private JiraApiClient _jiraApiClient;
        private CancellationTokenSource _cancellationTokenSource;

        public MainWindow()
        {
            //string username = Prompt.GetString("Please provide your username:");
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
        }

        #region Events
        private async void IssuesCell_MouseUp(object sender, RoutedEventArgs e)
        {
            await GetWorklogs();
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
        }
        #endregion

        private async Task GetWorklogs()
        {
            try
            {
                this.PreProcess();

                var issueId = (dgIssues.SelectedItem as Issue).Id;
                JiraWorklogResponse worklogs = await _jiraApiClient.GetWorklogsAsync(issueId, _cancellationTokenSource.Token);

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

        private void DisplayWorklogs(JiraWorklogResponse responseObject)
        {
            dgWorklog.ItemsSource = responseObject.Worklogs;
        }

        private void DisplayIssues(JiraIssueResponse responseObject)
        {
            dgIssues.ItemsSource = responseObject.Issues;
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

                JiraIssueResponse response = await _jiraApiClient.GetIssuesAsync(dateTimePicker.SelectedDate.GetValueOrDefault(), txtWorklogAuthor.Text, _cancellationTokenSource.Token);

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

        private void PostProcess()
        {
            lbLoad.Visibility = Visibility.Hidden;

            btnExecute.Content = "Search";

            _cancellationTokenSource = null;
        }

        private void PreProcess()
        {
            _cancellationTokenSource = new CancellationTokenSource();

            lbLoad.Visibility = Visibility.Visible;

            btnExecute.Content = "Cancel";
        }
    }
}
