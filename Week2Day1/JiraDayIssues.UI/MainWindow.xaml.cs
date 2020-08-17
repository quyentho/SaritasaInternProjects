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
        private string userName;

        private string token;

        public MainWindow()
        {
            userName = Prompt.GetString("Please provide your username:");

            token = Prompt.GetPassword("Please provide your token:");
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(token))
            {
                MessageBox.Show("You must provide correct username and token to use application.");
                Environment.Exit(0);
            }

            InitializeComponent();
        }

        private void IssuesCell_MouseUp(object sender, RoutedEventArgs e)
        {
            var index = IssuesGrid.SelectedIndex;

            IRestRequest request = apiManipulation.ConfigureGetWorklogRequest(issues[index].Id);
            IRestResponse response = apiManipulation.GetResponse(request, userName, token);
            ResponseObject responseObject = responseHandling.DeserializeResponse(response);


            List<Worklog> worklogs = responseObject.Worklogs;

            WorklogsGrid.ItemsSource = worklogs;
        }

        private void DateTimePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            IRestRequest request = apiManipulation.ConfigureGetIssuesRequest(DateTimePicker.SelectedDate.GetValueOrDefault());
            IRestResponse response = apiManipulation.GetResponse(request, userName, token);

            var responseObject = responseHandling.DeserializeResponse(response);

            issues = responseObject.Issues;

            IssuesGrid.ItemsSource = issues;
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
