using JiraDayIssues.Model;
using JiraDayIssues.Service;
using McMaster.Extensions.CommandLineUtils;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JiraDayIssues.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Issue> issues ;
        private ApiManipulation apiManipulation = new ApiManipulation();
        private ResponseHandling responseHandling = new ResponseHandling();
        
        private string username;
        private string token;

        public MainWindow()
        {
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
            IRestResponse response = await GetWorklogsResponse();

            ResponseObject responseObject = responseHandling.DeserializeResponse(response);
            
            DisplayWorklogs(responseObject);
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

            LoadingLabel.Visibility = Visibility.Visible;
            IRestResponse response = await apiManipulation.GetResponseAsync(request, username, token);
            LoadingLabel.Visibility = Visibility.Hidden;
            return response;
        }

        private async void DateTimePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            IRestResponse response = await GetIssuesResponse();

            var responseObject = responseHandling.DeserializeResponse(response);
            
            DisplayIssues(responseObject);
        }

        private void DisplayIssues(ResponseObject responseObject)
        {
            issues = responseObject.Issues;

            IssuesGrid.ItemsSource = issues;
        }

        private async Task<IRestResponse> GetIssuesResponse()
        {
            IRestRequest request = apiManipulation.ConfigureIssuesRequest(DateTimePicker.SelectedDate.GetValueOrDefault());

            LoadingLabel.Visibility = Visibility.Visible;
            IRestResponse response = await apiManipulation.GetResponseAsync(request, username, token);
            LoadingLabel.Visibility = Visibility.Hidden;
           
            return response;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
