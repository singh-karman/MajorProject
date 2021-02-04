using System;
using System.Windows;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;

namespace Completist.View
{
    /// <summary>
    /// Interaction logic for GCalendar.xaml
    /// </summary>
    public partial class GCalendar : Window
    {
        static string[] Scopes = { CalendarService.Scope.CalendarReadonly };
        static string ApplicationName = "Google Calendar API .NET Quickstart";
        public GCalendar()
        {
            InitializeComponent();
            GoogleAPI();
        }

        private void GoogleAPI()
        {
            UserCredential credential;

            using (var stream =
                new FileStream("credentials(MAIN).json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            // Create Google Calendar API service.
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define parameters of request.
            EventsResource.ListRequest request = service.Events.List("primary");
            request.TimeMin = DateTime.Now;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = 7;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            // List events.
            Events events = request.Execute();
            if (events.Items != null && events.Items.Count > 0)
            {
                CalendarEvents.Text = "";
                foreach (var eventItem in events.Items)
                {
                    CalendarEvents.Text += eventItem.Summary + Environment.NewLine;
                }
            }
            else
            {
                CalendarEvents.Text = "No Upcoming Events";
            }
        }
    }
}


//private void ChoosePriority_Method()
//{
//    if (myTask == null) { return; }

//    View.frmPriority window = new View.frmPriority();
//    SystemVars.FrmPriorityWindow = window;
//    window.ShowDialog();

//    myTask.Priority = SystemVars.SelectedPriority;
//    SystemVars.FrmPriorityWindow = null;
//}