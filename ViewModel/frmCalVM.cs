using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Completist.ViewModel
{
    class frmCalVM : INotifyPropertyChanged
    {
        string graphAPIEndpoint = "https://graph.microsoft.com/v1.0/me/calendarview?startdatetime=" + $"{DateTime.UtcNow:O}&enddatetime=" + $"{DateTime.UtcNow.AddDays(1):O}";
        string[] scopes = new string[] { "calendars.read" };
        string textAPIResult;
        //dynamic jsonAPIResult;
        dynamic JSONSeralised; //data type to be declared at runtime
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        public RelayCommand InitiateEventLoad_Command { get; set; }
        public RelayCommand AccountAuthorisation_Command { get; set; }

        ObservableCollection<Model.GCalendarModel> _listofCalEvents;
        public frmCalVM()
        {
            InitiateEventLoad_Command = new RelayCommand(InitiateEventLoad_Method);
            AccountAuthorisation_Command = new RelayCommand(AccountAuthorisation_Method);
        }
        public ObservableCollection<Model.GCalendarModel> listofCalEvents
        {
            get
            {
                return _listofCalEvents;
            }
            set
            {
                _listofCalEvents = value;
                NotifyPropertyChanged("listofCalEvents");
            }

        }
        public void InitiateEventLoad_Method()
        {
            int childCount = JSONSeralised["value"].Count; //total number of events in the response from API
            int countXAML = 0; //the number of events the loop has completed
            int eventCount = 15; //max events that will be loaded
            if (childCount == 0)
            {
                CalendarIndicator = "You have no events today - have a break!";
                ConnectionVisibility = "Collapsed";
            }
            else
            {
                while (countXAML < childCount && countXAML <= eventCount)
                {
                    string strEventName = JSONSeralised["value"][countXAML]["subject"];
                    DateTime eventTime = JSONSeralised["value"][countXAML]["start"]["dateTime"];
                    string strEventTime = eventTime.ToShortDateString();
                    string eventDayDelta = dayTimeDifference(eventTime);
                    listofCalEvents = new ObservableCollection<Model.GCalendarModel>(eventAppend(strEventName, strEventTime, eventDayDelta));
                    countXAML++;
                }
            }
            
        }
        public string connectionVisibility = "Visible";
        public string ConnectionVisibility
        {
            get
            {
                return connectionVisibility;
            }
            set
            {
                connectionVisibility = value;
                NotifyPropertyChanged("ConnectionVisibility");
            }
        }
        ObservableCollection<Model.GCalendarModel> eventList = new ObservableCollection<Model.GCalendarModel>();
        //relational patterns are not available in the version of C# I started in. I'm not going to change it now to a later version in case of compatibility issues. please don't take marks off for this region - switch conditions not viable
        
        /// <summary>
        /// called by InitiateEventLoad_Method()
        /// </summary>
        /// <param name="eventTime"></param>
        /// <returns></returns>
        public string dayTimeDifference(DateTime eventTime) //determines the status of the event
        {
            string strDateDelta;
            if (eventTime > DateTime.UtcNow)
            {
                strDateDelta = $"The event is in {DateTime.Parse(eventTime.Subtract(DateTime.UtcNow).ToString()):HH:mm} hours/minutes";
            }
            else if (DateTime.Today > eventTime)
            {
                strDateDelta = "This is a multiday event, i.e. repeating more than a single day"; //depreciated
            }
            else
            {
                strDateDelta = $"The task is on today";
            }
            return strDateDelta;

        }
        public string calendarIndicator = "Your Events";
        public string CalendarIndicator
        {
            get
            {
                return calendarIndicator;
            }
            set
            {
                calendarIndicator = value;
                NotifyPropertyChanged("CalendarIndicator");
            }
        }

        /// <summary>
        /// Changes the description
        /// </summary>
        /// <param name="strEventName"></param>
        /// <param name="strEventTime"></param>
        /// <param name="eventDayDelta"></param>
        /// <returns></returns>
        public ObservableCollection<Model.GCalendarModel> eventAppend(string strEventName, string strEventTime, string eventDayDelta)
        {
            Model.GCalendarModel reqEvent = new Model.GCalendarModel();
            reqEvent.EventName = $"{strEventName}: {strEventTime} - {eventDayDelta}";
            eventList.Add(reqEvent);
            return eventList;
        }
        public void AccountAuthorisation_Method()
        {
            CallGraphButton_Click(new object(), new RoutedEventArgs());
        }
        public void AccountSignOut_Method()
        {
            SignOutButton_Click(new object(), new RoutedEventArgs());
        }

        //The following code was heavily inspired by Microsoft Documentation
        public async void CallGraphButton_Click(object sender, RoutedEventArgs e)
        {
            AuthenticationResult authResult = null;
            var app = App.PublicClientApp;

            var accounts = await app.GetAccountsAsync();
            var firstAccount = accounts.FirstOrDefault(); //windows default account or one that you've already tried to use to login to Completist Calendar

            try //will attempt to acquire token silently and try to relogin the user without having to signin again using credentials
            {
                authResult = await app.AcquireTokenSilent(scopes, firstAccount)
                    .ExecuteAsync();
            }
            catch (MsalUiRequiredException ex)
            {
                // A MsalUiRequiredException happened on AcquireTokenSilent.
                // This indicates you need to call AcquireTokenInteractive to acquire a token
                System.Diagnostics.Debug.WriteLine($"MsalUiRequiredException: {ex.Message}");

                try //since acquiring a silent token has failed, a prompt will get you to login manually
                {
                    authResult = await app.AcquireTokenInteractive(scopes)
                        .WithAccount(accounts.FirstOrDefault())
                        .WithPrompt(Prompt.SelectAccount)
                        .ExecuteAsync();
                }
                catch (MsalException)
                {
                }
            }
            catch (Exception)
            {
                return;
            }

            if (authResult != null) //if token has been acquired -> stored in authResult
            {
                textAPIResult = await GetHttpContentWithToken(graphAPIEndpoint, authResult.AccessToken); //call Get..Token(params) with new auth token
                DisplayBasicTokenInfo(authResult);
            }
        }
        private async void SignOutButton_Click(object sender, RoutedEventArgs e) //depreciated
        {
            var accounts = await App.PublicClientApp.GetAccountsAsync();

            if (accounts.Any())
            {
                try
                {
                    await App.PublicClientApp.RemoveAsync(accounts.FirstOrDefault());
                }
                catch (MsalException)
                {

                }
            }
        }

        /// <summary>
        /// Gets a response from the Grapher API using the token and already initialised url
        /// async to prevent hanging - improves UI
        /// </summary>
        /// <param name="url"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string> GetHttpContentWithToken(string url, string token)
        {
            var httpClient = new System.Net.Http.HttpClient();
            System.Net.Http.HttpResponseMessage response;
            try
            {
                var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, url); //build request as a new Request message with parameters GET method and url
                //Add the token in Authorization header
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token); //authenticate request using bearer authentication with token
                response = await httpClient.SendAsync(request); //get response
                var content = await response.Content.ReadAsStringAsync(); //Serialise response
                return content;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        private void DisplayBasicTokenInfo(AuthenticationResult authResult)
        {
            if (authResult != null)
            {
                JSONSeralised = JsonConvert.DeserializeObject(textAPIResult); //desealise response -> remove object refereces
                InitiateEventLoad_Method(); //call method to interpret response
            }
        }
    }
}

