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
        string graphAPIEndpoint = "https://graph.microsoft.com/v1.0/me/calendars";
        string[] scopes = new string[] { "calendars.read" };
        string textAPIResult;
        dynamic jsonAPIResult;
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
            jsonAPIResult = textAPIResult;
            JObject eventObject = JObject.Parse(jsonAPIResult);
            JArray eventArray = (JArray)eventObject["value"];
            IList<Events> events = eventArray.ToObject<IList<Events>>();
            string tempVariable = events[0].Name;
            listofCalEvents = eventAppend(tempVariable);
            //Model.GCalendarModel eventList = new Model.GCalendarModel { EventName = $"{events[0].Name}" };\
        }
        public ObservableCollection<Model.GCalendarModel> eventAppend(string tempVariable)
        {
            ObservableCollection<Model.GCalendarModel> eventList = new ObservableCollection<Model.GCalendarModel>();
            Model.GCalendarModel reqEvent = new Model.GCalendarModel();
            reqEvent.EventName = tempVariable;
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
                jsonAPIResult = JsonConvert.DeserializeObject(textAPIResult);
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
