﻿using JiraDayIssues.Model;
using JiraDayIssues.Service;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace JiraDayIssues.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IJiraApiClient _jiraApiClient;
        private CancellationTokenSource _cancellationTokenSource;


        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public MainWindow()
        {
            // string username = Prompt.GetString("Please provide your username:");
            string username = "quyen.tho@saritasa.com";
            //string token = Prompt.GetPassword("Please provide your token:");
            string token = "muvykLqdTaYg3qbbdFmf37FE";

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(token))
            {
                MessageBox.Show("You must provide correct username and token to use application.");

                _logger.Info("Not input username or password .Exit application.");

                Environment.Exit(0);
            }

            _jiraApiClient = new CachedJiraApiClient(new JiraApiClient(username, token));

            InitializeComponent();

            SetDefaultSelectedDate();
        }

        private void SetDefaultSelectedDate()
        {
            if (DateTime.Today.DayOfWeek == DayOfWeek.Sunday)
            {
                datePicker.SelectedDate = DateTime.Today.AddDays(-2);
            }
            else if (DateTime.Today.DayOfWeek == DayOfWeek.Monday)
            {
                datePicker.SelectedDate = DateTime.Today.AddDays(-3);
            }
            else
            {
                datePicker.SelectedDate = DateTime.Today.AddDays(-1);
            }
        }

        #region Events
        private async void IssuesCell_MouseUp(object sender, RoutedEventArgs e)
        {
            string issueId = (dgIssues.SelectedItem as Issue).Id;
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
        }
        #endregion

        private async Task<List<Worklog>> GetWorklogs(string issueId)
        {
            try
            {
                _cancellationTokenSource = new CancellationTokenSource();

                List<Worklog> worklogs =
                    await _jiraApiClient.GetWorklogsAsync(issueId, _cancellationTokenSource.Token);

                return worklogs;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
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

                DisplayIssues(response.Issues);
            }
            catch (JsonReaderException)
            {
                MessageBox.Show("Wrong credentials!");
                _logger.Error("Cannot deserialize response.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                _logger.Error(ex.Message);
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

            _cancellationTokenSource.Dispose();

            _cancellationTokenSource = null;
        }

        private void PreProcess()
        {
            _cancellationTokenSource = new CancellationTokenSource();

            (_jiraApiClient as CachedJiraApiClient).CachedWorklogs.Clear();

            lbLoad.Visibility = Visibility.Visible;

            btnExecute.Content = "Cancel";
        }
    }
}
