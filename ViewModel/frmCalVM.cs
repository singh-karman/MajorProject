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
        dynamic JSONSeralised;
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
        public class Events
        {
            public string Name { get; set; }
        }
        public void InitiateEventLoad_Method()
        {
            //listofCalEvents = jsonAPIResult;
            //jsonAPIResult = textAPIResult;
            //JObject eventObject = JObject.Parse(jsonAPIResult);
            //JArray eventArray = (JArray)eventObject["value"];

            //IList<Events> events = eventArray.ToObject<IList<Events>>();
            //string tempVariable = events[0].Name;
            int childCount = JSONSeralised["value"].Count;
            int countXAML = 0;
            int eventCount = 15;
            if (childCount == 0)
            {
                CalendarIndicator = "You have no events today - have a break!";
                ConnectionVisibility = "Collapsed";
            }
            else
            {
                //while (countXAML < childCount && countXAML < eventCount)
                //{
                //    ObservableCollection<Model.GCalendarModel> eventList = new ObservableCollection<Model.GCalendarModel>();
                //    Model.GCalendarModel requestedEvent = new Model.GCalendarModel();
                //    string eventXAMLName = JSONSeralised["value"][countXAML]["subject"];
                //    DateTime eventTime = JSONSeralised["value"][countXAML]["start"]["dateTime"];
                //    string strEventTime = eventTime.ToString();
                //    requestedEvent.EventName = eventXAMLName;
                //    requestedEvent.EventTime = strEventTime;
                //    countXAML++;
                //    listofCalEvents = new ObservableCollection<Model.GCalendarModel>(eventList);
                //}


                //while (countXAML < childCount && countXAML < eventCount)
                //{
                //    //Model.GCalendarModel reqEvent = new Model.GCalendarModel();
                //    string varWatch = APIResult["value"][countXAML]["subject"];
                //    DateTime eventTime = APIResult["value"][countXAML]["start"]["dateTime"];
                //    string strEventTime = eventTime.ToShortDateString();
                //    listofCalEvents = new ObservableCollection<Model.GCalendarModel>(eventAppend(reqEvent, varWatch));
                //    //DateCalEvent = new ObservableCollection<Model.GCalendarModel>(eventDateAppend(reqEvent, strEventTime));
                //    countXAML++;
                //}
                //for (int i = 0; i < childCount && i < eventCount; i++) //children start at zero - as in the JSON header children. much like real life children 
                //{
                //    //Model.GCalendarModel reqEvent = new Model.GCalendarModel();
                //    string varWatch = JSONSeralised["value"][i]["subject"];
                //    DateTime eventTime = JSONSeralised["value"][i]["start"]["dateTime"];
                //    string strEventTime = eventTime.ToShortDateString();
                //    //eventAppend(/*tempVariable,*/ varWatch);
                //    listofCalEvents = new ObservableCollection<Model.GCalendarModel>(eventAppend(varWatch, strEventTime));
                //    //DateCalEvent = new ObservableCollection<Model.GCalendarModel>(eventDateAppend(reqEvent, strEventTime));
                //    //listofCalEvents.Add(eventAppend(varWatch));

                //}
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
            
            
            //Model.GCalendarModel eventList = new Model.GCalendarModel { EventName = $"{events[0].Name}" };\
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
        //I'm a freakin idgiot - relational patterns are not available in the version of C# I started in. I'm not going to change it now to a later version in case of compatibility issues. please don't take marks off for this region - switch conditions not viable
        public string dayTimeDifference(DateTime eventTime)
        {
            string strDateDelta;
            if (eventTime > DateTime.UtcNow)
            {
                strDateDelta = $"The event is in {DateTime.Parse(eventTime.Subtract(DateTime.UtcNow).ToString()):HH:mm} hours/minutes";
            }
            else if (DateTime.Today > eventTime)
            {
                strDateDelta = "This is a multiday event, i.e. repeating more than a single day";
            }
            else
            {
                strDateDelta = $"The task is on today";
            }
            return strDateDelta;

            //switch (DateTime.Today)  
            //{
            //    case > eventTime:
            //        strDateDelta = "";
            //        break;
            //    case == eventTime:
            //        strDateDelta = "";
            //        break;
            //    default:
            //        strDateDelta = "";
            //        break;
            //}
            //return strDateDelta;
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
        public ObservableCollection<Model.GCalendarModel> eventAppend(/*string tempVariable, Model.GCalendarModel */ string strEventName, string strEventTime, string eventDayDelta)
        {
            Model.GCalendarModel reqEvent = new Model.GCalendarModel();
            reqEvent.EventName = $"{strEventName}: {strEventTime} - {eventDayDelta}";
            eventList.Add(reqEvent);
            return eventList;
        }
        //public ObservableCollection<Model.GCalendarModel> eventDateAppend(Model.GCalendarModel reqEvent, string eventTime)
        //{
        //    reqEvent.EventTime = eventTime;
        //    eventList.Add(reqEvent);
        //    return eventList;
        //}
        public void AccountAuthorisation_Method()
        {
            CallGraphButton_Click(new object(), new RoutedEventArgs());
        }
        public void AccountSignOut_Method()
        {
            SignOutButton_Click(new object(), new RoutedEventArgs());
        }

        public async void CallGraphButton_Click(object sender, RoutedEventArgs e)
        {
            AuthenticationResult authResult = null;
            var app = App.PublicClientApp;
            //ResultText.Text = string.Empty;
            //TokenInfoText.Text = string.Empty;

            var accounts = await app.GetAccountsAsync();
            var firstAccount = accounts.FirstOrDefault();

            try
            {
                authResult = await app.AcquireTokenSilent(scopes, firstAccount)
                    .ExecuteAsync();
            }
            catch (MsalUiRequiredException ex)
            {
                // A MsalUiRequiredException happened on AcquireTokenSilent.
                // This indicates you need to call AcquireTokenInteractive to acquire a token
                System.Diagnostics.Debug.WriteLine($"MsalUiRequiredException: {ex.Message}");

                try
                {
                    authResult = await app.AcquireTokenInteractive(scopes)
                        .WithAccount(accounts.FirstOrDefault())
                        .WithPrompt(Prompt.SelectAccount)
                        .ExecuteAsync();
                }
                catch (MsalException msalex)
                {
                    //ResultText.Text = $"Error Acquiring Token:{System.Environment.NewLine}{msalex}";
                }
            }
            catch (Exception ex)
            {
                //ResultText.Text = $"Error Acquiring Token Silently:{System.Environment.NewLine}{ex}";
                return;
            }

            if (authResult != null)
            {
                textAPIResult = await GetHttpContentWithToken(graphAPIEndpoint, authResult.AccessToken);
                //ResultText.Text = textAPIResult;
                DisplayBasicTokenInfo(authResult);
                //this.SignOutButton.Visibility = Visibility.Visible;
            }
        }
        private async void SignOutButton_Click(object sender, RoutedEventArgs e)
        {
            var accounts = await App.PublicClientApp.GetAccountsAsync();

            if (accounts.Any())
            {
                try
                {
                    await App.PublicClientApp.RemoveAsync(accounts.FirstOrDefault());
                    //this.ResultText.Text = "User has signed-out";
                    //this.CallGraphButton.Visibility = Visibility.Visible;
                    //this.SignOutButton.Visibility = Visibility.Collapsed;
                }
                catch (MsalException ex)
                {
                    //ResultText.Text = $"Error signing-out user: {ex.Message}";
                }
            }
        }
        public async Task<string> GetHttpContentWithToken(string url, string token)
        {
            var httpClient = new System.Net.Http.HttpClient();
            System.Net.Http.HttpResponseMessage response;
            try
            {
                var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, url);
                //Add the token in Authorization header
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                response = await httpClient.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        private void DisplayBasicTokenInfo(AuthenticationResult authResult)
        {
            //TokenInfoText.Text = "";
            if (authResult != null)
            {
                JSONSeralised = JsonConvert.DeserializeObject(textAPIResult);
                InitiateEventLoad_Method();
                //TokenInfoText.Text += $"Username: {authResult.Account.Username}" + Environment.NewLine;
                //TokenInfoText.Text += $"Token Expires: {authResult.ExpiresOn.ToLocalTime()}" + Environment.NewLine;
                //TokenInfoText.Text += jsonAPIResult;
            }
        }
    }
}

//private async void CallGraphButton_Click(object sender, RoutedEventArgs e)
//{
//    AuthenticationResult authResult = null;
//    var app = App.PublicClientApp;
//    //ResultText.Text = string.Empty;
//    //TokenInfoText.Text = string.Empty;

//    var accounts = await app.GetAccountsAsync();
//    var firstAccount = accounts.FirstOrDefault();

//    try
//    {
//        authResult = await app.AcquireTokenSilent(scopes, firstAccount)
//            .ExecuteAsync();
//    }
//    catch (MsalUiRequiredException ex)
//    {
//        // A MsalUiRequiredException happened on AcquireTokenSilent.
//        // This indicates you need to call AcquireTokenInteractive to acquire a token
//        System.Diagnostics.Debug.WriteLine($"MsalUiRequiredException: {ex.Message}");

//        try
//        {
//            authResult = await app.AcquireTokenInteractive(scopes)
//                .WithAccount(accounts.FirstOrDefault())
//                .WithPrompt(Prompt.SelectAccount)
//                .ExecuteAsync();
//        }
//        catch (MsalException msalex)
//        {
//            //ResultText.Text = $"Error Acquiring Token:{System.Environment.NewLine}{msalex}";
//        }
//    }
//    catch (Exception ex)
//    {
//        //ResultText.Text = $"Error Acquiring Token Silently:{System.Environment.NewLine}{ex}";
//        return;
//    }

//    if (authResult != null)
//    {
//        textAPIResult = await GetHttpContentWithToken(graphAPIEndpoint, authResult.AccessToken);
//        //ResultText.Text = textAPIResult;
//        DisplayBasicTokenInfo(authResult);
//        //this.SignOutButton.Visibility = Visibility.Visible;
//    }
//}
