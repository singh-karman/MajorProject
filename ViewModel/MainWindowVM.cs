using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Completist.Model;
using System.Windows;
using System.Windows.Media.Animation;
using System.Threading;
using System.Windows.Threading;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Completist.ViewModel
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        //private int count = 0;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        #region Commands
        public RelayCommand Loaded_Command { get; private set; }
        public RelayCommand NewTask_Command { get; private set; }
        public RelayCommand Refresh_Command { get; private set; }
        public RelayCommand Search_Command { get; private set; }
        public RelayCommand History_Command { get; private set; }
        public RelayCommand Data_Command { get; private set; }
        public RelayCommand GCalendar_Command { get; private set; }
        public RelayCommand CompleteTask_Command { get; private set; }
        public RelayCommand RemoveTask_Command { get; private set; }
        public RelayCommand EditTask_Command { get; private set; }
        public RelayCommand lstSelectionChanged_Command { get; private set; }
        public RelayCommand ChangePriority_Command { get; private set; }
        public RelayCommand ChangeTag_Command { get; private set; }
        public RelayCommand SearchTaskText_Command { get; private set; }
        public RelayCommand RemoveSelection_Command { get; private set; }
        public RelayCommand UndoDeletion_Command { get; private set; }
        public RelayCommand ActivateAssistant_Command { get; private set; }
        #endregion

        #region Variables & Objects

        bool searchIsActive;

        Tag _selectedTag_Search;
        public Tag selectedTag_Search
        {
            get
            {
                return _selectedTag_Search;
            }
            set
            {
                _selectedTag_Search = value;
                NotifyPropertyChanged("selectedTag_Search");
            }
        }

        Priority _selectedPriority_Search;
        public Priority selectedPriority_Search
        {
            get
            {
                return _selectedPriority_Search;
            }
            set
            {
                _selectedPriority_Search = value;
                NotifyPropertyChanged("selectedPriority_Search");
            }
        }

        string _filterText;
        public string filterText 
        {
            get 
            {
                return _filterText;
            }
            set 
            {
                _filterText = value;
                NotifyPropertyChanged("filterText");
            }
        }

        double _searchHeight;
        public double searchHeight 
        {
            get 
            {
                return _searchHeight;
            }
            set 
            {
                _searchHeight = value;
                NotifyPropertyChanged("searchHeight");
            }
        }
        int _undoCountdown = 5;
        public int undoCountdown
        {
            get
            {
                return _undoCountdown;
            }
            set
            {
                _undoCountdown = value;
                NotifyPropertyChanged("undoCountdown");
            }
        }
        string _undoBannerVisibility = "Collapsed";
        public string undoBannerVisibility 
        {
            get 
            {
                return _undoBannerVisibility;
            }
            set
            {
                _undoBannerVisibility = value;
                NotifyPropertyChanged("undoBannerVisibility");
            }
        }
        string _clockVisibility = "Collapsed";
        public string clockVisibility
        {
            get
            {
                return _clockVisibility;
            }
            set
            {
                _clockVisibility = value;
                NotifyPropertyChanged("clockVisibility");
            }
        }

        //double _sharePrice;
        //public double sharePrice
        //{
        //    get
        //    {
        //        return _sharePrice;
        //    }
        //    set
        //    {
        //        _sharePrice = value;
        //        NotifyPropertyChanged("sharePrice");
        //    }
        //}
        string _assistantHeightProperty = "Bottom";
        public string assistantHeightProperty
        {
            get
            {
                return _assistantHeightProperty;
            }
            set
            {
                _assistantHeightProperty = value;
                NotifyPropertyChanged("assistantHeightProperty");
            }
        }
        string _lastSelectedName;
        public string lastSelectedName 
        {
            get 
            {
                return _lastSelectedName;
            }
            set 
            {
                _lastSelectedName = value;
                NotifyPropertyChanged("lastSelectedName");
            }
        }
        ObservableCollection<Priority> _listOfPriorities;
        public ObservableCollection<Priority> listOfPriorities
        {
            get
            {
                return _listOfPriorities;
            }
            set
            {
                _listOfPriorities = value;
                NotifyPropertyChanged("listOfPriorities");
            }
        }

        ObservableCollection<Tag> _listOfTags;
        public ObservableCollection<Tag> listOfTags
        {
            get
            {
                return _listOfTags;
            }
            set
            {
                _listOfTags = value;
                NotifyPropertyChanged("listOfTags");
            }
        }
        double _myHeight;
        public double myHeight
        {
            get
            {
                return _myHeight;
            }
            set
            {
                _myHeight = value;
                NotifyPropertyChanged("myHeight");
            }
        }
        ObservableCollection<Model.Task> _myContent;
        public ObservableCollection<Model.Task> myContent
        {
            get
            {
                return _myContent;
            }
            set
            {
                _myContent = value;
                NotifyPropertyChanged("myContent");
            }
        }
        ObservableCollection<Model.ShareExchange> _collectionShares;
        public ObservableCollection<Model.ShareExchange> CollectionShares
        {
            get
            {
                return _collectionShares;

            }
            set
            {
                _collectionShares = value;
                NotifyPropertyChanged("collectionShares");
            }
        }
        Model.Task _selectedTask;
        public Model.Task selectedTask
        {
            get
            {

                return _selectedTask;
            }
            set
            {
                _selectedTask = value;
                NotifyPropertyChanged("selectedTask");
            }
        }

        string _myCondition_Name;
        public string myCondition_Name
        {
            get 
            {
                return _myCondition_Name;
            }
            set 
            {
                _myCondition_Name = value;
                NotifyPropertyChanged("myCondition_Name");
            }
        }

        string _myCondition_Tag;
        public string myCondition_Tag
        {
            get
            {
                return _myCondition_Tag;
            }
            set
            {
                _myCondition_Tag = value;
                NotifyPropertyChanged("myCondition_Tag");
            }
        }

        string _myCondition_Priority;
        public string myCondition_Priority
        {
            get
            {
                return _myCondition_Priority;
            }
            set
            {
                _myCondition_Priority = value;
                NotifyPropertyChanged("myCondition_Priority");
            }
        }

        string _title;
        public string title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                NotifyPropertyChanged("title");
            }
        }

        ICollectionView _myContent_View;
        public ICollectionView myContent_View
        {
            get
            {
                return _myContent_View;
            }
            set
            {
                _myContent_View = value;
                NotifyPropertyChanged("myContent_View");
            }
        }

        string _visibleNew;
        public string visibleNew 
        {
            get 
            {
                return _visibleNew;
            }
            set 
            {
                _visibleNew = value;
                NotifyPropertyChanged("visibleNew");
            }
        }

        string _visibleExit;
        public string visibleExit
        {
            get
            {
                return _visibleExit;
            }
            set
            {
                _visibleExit = value;
                NotifyPropertyChanged("visibleExit");
            }
        }

        string _complete;

        public string complete
        {
            get
            {
                return _complete;
            }
            set
            {
                _complete = value;
                NotifyPropertyChanged("complete");
            }
        }

        Controller.Controller con;

        #endregion

        public MainWindowVM()
        {
            con = new Controller.Controller();
            searchIsActive = false;

            selectedPriority_Search = new Priority();
            selectedTag_Search = new Tag();
            #region Command to methods
            Loaded_Command = new RelayCommand(Loaded_Method);
            NewTask_Command = new RelayCommand(NewTask_Method);
            Refresh_Command = new RelayCommand(Refresh_Method);
            Search_Command = new RelayCommand(Search_Method);
            History_Command = new RelayCommand(History_Method);
            Data_Command = new RelayCommand(Data_Method);
            GCalendar_Command = new RelayCommand(GCalendar_Method);
            RemoveTask_Command = new RelayCommand(RemoveTask_Method);
            CompleteTask_Command = new RelayCommand(CompleteTask_Method);
            EditTask_Command = new RelayCommand(EditTask_Method);
            lstSelectionChanged_Command = new RelayCommand(lstSelectionChanged_Method);
            ChangePriority_Command = new RelayCommand(ChangePriority_Method);
            ChangeTag_Command = new RelayCommand(ChangeTag_Method);
            SearchTaskText_Command = new RelayCommand(SearchTaskText_Method);
            RemoveSelection_Command = new RelayCommand(RemoveSelection_Method);
            UndoDeletion_Command = new RelayCommand(UndoDeletion_Method);
            ActivateAssistant_Command = new RelayCommand(ActivateAssistant_Method);
            #endregion
        }
        #region DEPRECIATED
        public void Refresh_Method() //called when refresh button is clicked
        {
            Loaded_Method(); //trigger upon first load of application
        }
        #endregion

        private void SearchTaskText_Method() //Button in View calls method in ViewModel
        {
            filterMe();
        }

        public void filterMe()
        {
            //building the connection, the necessary condition for filter, afterwhich, we call controller and the DB
            string condition = " where STS=0 and name like '%" + filterText + "%'";

            if (selectedTag_Search != null)
            {
                condition += " AND TAGLIST LIKE '%" + selectedTag_Search.Name + "%'";
            }

            if (selectedPriority_Search != null)
            {
                condition += " AND PRIORITY LIKE '%" + selectedPriority_Search.Name + "%'";
            }
            myContent = con.returnAllTasks(condition); //calling method in external class to return all task that fit new condition. Method in controller returns collection received by Broker interface
            myContent = new ObservableCollection<Model.Task>(myContent.OrderBy(x => x.Due).ThenBy(v => v.Name)); //LINQ statement to order returned tasks By dueDate and then by Alphabetical order
        }

        private void ChangeTag_Method()
        {
            if (selectedTask == null)
            {
                MessageBox.Show("First click on task in order to select it");
                return;
            }
            //Opens tags window for changes 
            View.frmTag window = new View.frmTag(); //Creates a new instance of frmTag class accessed by 'window'. This is done as making this method static as another way of creating a new instance of frmTag window will make the XAML controls inaccessible.
            SystemVars.FrmTagWindow = window; //Set the system variable to current instance of Tag Window in the case of multiple windows
            window.ShowDialog(); //Display instance of frmTag View

            selectedTask.TagList = SystemVars.SelectedTagList; //tags selected in opened window that were temperoraily stored in System variable are now set as the collection associated with task object
            SystemVars.FrmTagWindow = null; //instance set to null ready for next call
        }
        private void RemoveSelection_Method()
        {
            selectedTask = null; //removes focus off the listview item. kinda dodgy - a better implementation would be to use ICommand [Relay Command] via interaction triggers in View page
        }
        DateTime _now;
        public void ActivateAssistant_Method() //RelayCommand activating method in VM page
        {
            _now = DateTime.Now; //calling System method to get date and time
            DispatcherTimer timer = new DispatcherTimer(); //creates new instance of DispatcherTimer class which allows the queing of tasks and processes
            timer.Interval = TimeSpan.FromMilliseconds(1000); //sets the interval before the next task to 1000 Miliseconds -> 1 Second. The FromMilliseconds method uses given argument instead of default case: Polymorphism
            timer.Tick += new EventHandler(timer_Tick); //adds event handler
            timer.Start();
            //FinanceResultsRESTAsync();
            if (assistantHeightProperty != "stretch")
            {
                assistantHeightProperty = "stretch";
                clockVisibility = "Visible";
            }
            else
            {
                assistantHeightProperty = "bottom";
                clockVisibility = "Collapsed";
                timer.Stop();
            }
            CollectionShares = con.ReturnAllExchanges(); //Returns collection of stocks which are then binded to view
        }
        public DateTime CurrentDateTime  //source: Microsoft Documentation for C# .Net Framework
        {
            get { return _now; }
            private set
            {
                _now = value;
                PropertyChanged(this, new PropertyChangedEventArgs("CurrentDateTime"));
            }
        }
        void timer_Tick(object sender, EventArgs e) //source: Microsoft Documentation for C# .Net Framework
        {
            CurrentDateTime = DateTime.Now;
        }
        public async void FinanceResultsRESTAsync(string stockName, ShareExchange x)
        {
            
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://yfapi.net/v6/finance/quote?symbols=" + $"{stockName}"),
                Headers =
                {
                    { "x-api-key", "o47mcKBIAA5fxmpPJ28zh27q94nUbP2Q8W4PYpGh" },
                    //{ "x-rapidapi-host", "yahoo-finance-low-latency.p.rapidapi.com" },
                },
            };
            using (var response = await client.SendAsync(request))
            {
                try
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    //var JSONContent = await GetHTTPContent();
                    //string JSONSeralised = (string)JsonConvert.DeserializeObject(body);
                    //string sharePrice = JSONSeralised["quoteResponse"].ToString()["value"][0]["regularMarketPrice"];   //["value"][0]["regularMarketPrice"];
                    
                    var detailsJSON = JObject.Parse(body);
                    var conditionFinanceJSON = detailsJSON["quoteResponse"]["result"][0];
                    //if (true)
                    //{

                    //}
                    x.SharePrice = double.Parse(conditionFinanceJSON["regularMarketPrice"].ToString());
                    double shareDelta = double.Parse(conditionFinanceJSON["regularMarketChange"].ToString());
                    if (shareDelta >= 0)
                    {
                        x.ShareIndicatorColor = "#05C46B";
                    }
                    else
                    {
                        x.ShareIndicatorColor = "#E63946";
                    }
                }
                catch (Exception ex)
                {
                    string message = ex.Message;
                }

            }
        }
        private void ChangePriority_Method()
        {
            if (selectedTask == null) 
            {
                MessageBox.Show("First click on task in order to select it");
                return; 
            }
            //Opens priority window for changes 
            View.frmPriority window = new View.frmPriority();
            SystemVars.FrmPriorityWindow = window;
            window.ShowDialog();

            selectedTask.Priority = SystemVars.SelectedPriority;
            SystemVars.FrmPriorityWindow = null;
        }
        
        private void lstSelectionChanged_Method()
        {
            //if there are no selected tasks then the menu bar says 'Inbox'
            if (selectedTask == null || String.IsNullOrEmpty(selectedTask.Name)) { selectedTask = new Model.Task(); title = "Inbox"; return; }
            //lastSelectedName = selectedTask.Name;
            title = "Editing [" + selectedTask.Name + "]";
            //if there is a task selected the menu bar says 'Editing'
            Application.Current.MainWindow.Opacity = 0.5; //background opacity change - I am aiming to mirror the prototype I created for the requirements doc
            View.frmTask window = new View.frmTask(selectedTask);
            SystemVars.FrmTaskWindow = window;
            window.ShowDialog();
            Application.Current.MainWindow.Opacity = 1; //return to normal
            title = "Inbox"; //menu bar header returns to 'Inbox'
            SystemVars.FrmTaskWindow = null;
            myContent = con.returnAllTasks(" where STS=0");
            myContent = new ObservableCollection<Model.Task>(myContent.OrderBy(x => x.Due).ThenBy(v => v.Name)); //LINQ sorting, sort by due date then by name

        }

        private void History_Method()
        {
            View.frmHistory window = new View.frmHistory();
            window.ShowDialog();
        }

        private void CompleteTask_Method() //needs to be refactored - status unchanged prior commit 
        {
            //if (selectedTask == null)
            //{
            //    MessageBox.Show("Task is not selected ", "", MessageBoxButton.OK);
            //    return;
            //}
            con.handleTask(selectedTask, "COMPLETE", selectedTask.TaskID.ToString());
            myContent = con.returnAllTasks("where STS=0");
            title = "Inbox";
            con.CounterIncrement();
            RefreshCount();

            //if (MessageBox.Show("Complete task [" + selectedTask.Name + "]?", "", MessageBoxButton.YesNo) == MessageBoxResult.No) { return; }

            //if (con.handleTask(selectedTask, "COMPLETE", selectedTask.Name)) { myContent = con.returnAllTasks("where STS=0"); title = "Inbox"; con.CounterIncrement(); refreshCount(); /*count = count + 1; complete = count.ToString();*/ }
        }
        static System.Timers.Timer undoTimer;
        public void RemoveTask_Method()
        {
            if (undoTimer != null)
            {
                undoTimer.Dispose();
            }
            //undoTimer = new System.Timers.Timer(1000);
            //undoTimer.Dispose();
            taskDelete = selectedTask;
            strTaskDelete = selectedTask.Name;
            con.handleTask(selectedTask, "REMOVE", selectedTask.TaskID.ToString());
            myContent = con.returnAllTasks("where STS=0"); title = "Inbox";
            undoTimer = new System.Timers.Timer(1000);
            undoTimer.Elapsed += TimerEvent;
            undoTimer.AutoReset = false;
            undoTimer.Enabled = true;
            undoCountdown = 5;
            undoBannerVisibility = "Visible";
            
        }
        void TimerEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            undoCountdown--;
            if (undoCountdown == 0 && undoRequested == false)
            {
                undoTimer.Dispose();
                con.handleTask(taskDelete, "REMOVE", taskDelete.TaskID.ToString());
                myContent = con.returnAllTasks("where STS=0"); title = "Inbox";
                undoBannerVisibility = "Collapsed";
                undoCountdown = 5;
            }
            else
            {
                undoTimer.Start();
            }
            if (undoCountdown < 0) //My method of implementation has been prone to instances where if the user clicks on the delete button at a certain time, the two timers may run concurrently
            {
                undoTimer.Dispose();
            }
        }
        public void UndoDeletion_Method()
        {
            undoTimer.Dispose();
            undoBannerVisibility = "Collapsed";
            undoRequested = true;
            con.handleTask(taskDelete, "UNDO", taskDelete.TaskID.ToString());
            myContent = con.returnAllTasks("where STS=0"); title = "Inbox";
            undoCountdown = 5;
        }
        bool undoRequested = false;
        Model.Task taskDelete;
        string strTaskDelete;
        #region Depreciated
        //private void RemoveTask_Method()
        //{
        //    taskDelete = selectedTask;
        //    strTaskDelete = selectedTask.Name;
        //    con.handleTask(selectedTask, "REMOVE", selectedTask.Name);
        //    myContent = con.returnAllTasks("where STS=0"); title = "Inbox";

        //    undoBannerVisibility = "Visible";
        //    undoCountdown = 5;
        //    DelayMethod();
        //    //UndoPeriod(taskDelete, strTaskDelete);
        //    //System.Threading.Tasks.Task.Delay(5000);
        //    //undoBannerVisibility = "Collapsed";
        //    //BannerCollapse(taskDelete, strTaskDelete);
        //    //MainWindow mainWindow = new MainWindow();
        //    //mainWindow.UndoBannerReveal();
        //    //frame element show
        //    #region No Longer Relevant -> UI Changes
        //    //if (selectedTask == null)
        //    //{
        //    //    MessageBox.Show("Task is not selected ", "", MessageBoxButton.OK);
        //    //    return;
        //    //}

        //    //if (MessageBox.Show("Remove task [" + selectedTask.Name + "]?", "", MessageBoxButton.YesNo) == MessageBoxResult.No) { return; }

        //    //if (con.handleTask(selectedTask, "REMOVE", selectedTask.Name)) { myContent = con.returnAllTasks("where STS=0"); title = "Inbox"; }
        //    #endregion
        //}
        //public async void DelayMethod()
        //{
        //    for (int i = 0; i < 6; i++)
        //    {
        //        await System.Threading.Tasks.Task.Delay(1000);
        //        undoCountdown--;
        //    }
        //    undoBannerVisibility = "Collapsed";
        //    if (undoRequested == false)
        //    {
        //        con.handleTask(taskDelete, "DELETE", strTaskDelete);
        //        myContent = con.returnAllTasks("where STS=0"); title = "Inbox";
        //    }
        //    undoCountdown = 5;
        //}

        //private void UndoPeriod(Model.Task taskDelete, string strTaskDelete)
        //{
        //    con.handleTask(taskDelete, "UNDO", strTaskDelete);
        //    myContent = con.returnAllTasks("where STS=0"); title = "Inbox";
        //}
        //private void BannerCollapse(Model.Task taskDelete, string strTaskDelete /*CancellationToken token*/)
        //{
        //    //System.Threading.Tasks.Task.Delay(5000/*, token*/).ContinueWith(_ =>
        //    // {
        //    //     undoBannerVisibility = "Collapsed";
        //    //     if (undoRequested == false)
        //    //     {
        //    //         con.handleTask(taskDelete, "DELETE", strTaskDelete);
        //    //         myContent = con.returnAllTasks("where STS=0"); title = "Inbox";
        //    //     }
        //    //     else
        //    //     {
        //    //         con.handleTask(taskDelete, "UNDO", strTaskDelete);
        //    //         myContent = con.returnAllTasks("where STS=0"); title = "Inbox";
        //    //     }
        //    // }
        //    //);
        //    System.Threading.Tasks.Task.Delay(5000);
        //    //undoBannerVisibility = "Collapsed";
        //    if (undoRequested == false)
        //    {
        //        con.handleTask(taskDelete, "DELETE", strTaskDelete);
        //        myContent = con.returnAllTasks("where STS=0"); title = "Inbox";
        //    }
        //    else
        //    {
        //        con.handleTask(taskDelete, "UNDO", strTaskDelete);
        //        myContent = con.returnAllTasks("where STS=0"); title = "Inbox";
        //    }



        //}
        //public void UndoDeletion_Method()
        //{
        //    undoRequested = true;
        //    undoBannerVisibility = "Collapsed";
        //    con.handleTask(taskDelete, "UNDO", strTaskDelete);
        //    myContent = con.returnAllTasks("where STS=0"); title = "Inbox";
        //    //var tokenSource = new CancellationTokenSource();
        //    //BannerCollapse(tokenSource.Token);
        //    //tokenSource.Cancel();
        //} 
        #endregion

        private void EditTask_Method()
        {
            if (selectedTask == null) { return; }
            if (String.IsNullOrEmpty(lastSelectedName)) { lastSelectedName = selectedTask.Name; }
            if (con.handleTask(selectedTask, "EDIT", selectedTask.TaskID.ToString())) { myContent = con.returnAllTasks("where STS=0"); selectedTask = new Model.Task(); title = "Inbox"; }
        }


        private async void start_Ini()
        {
            await System.Threading.Tasks.Task.Run(() => start());
        }
        public string RefreshCount()
        {
            try
            {
                complete = con.TaskCounter().ToString();
                return complete;
            }
            catch (Exception)
            {

                return complete;
            }
        }
        private void start() 
        {
            try
            {
                complete = con.TaskCounter().ToString(); //count.ToString();
                title = "Inbox"; //initial values
                visibleExit = "Hidden";
                visibleNew = "Visible";
                searchHeight = 0;
                //undoBannerVisibility = "Collapsed";
                myCondition_Name = "";
                myCondition_Priority = "";
                myContent = con.returnAllTasks(" where STS=0");
                myContent = new ObservableCollection<Model.Task>(myContent.OrderBy(x => x.Due).ThenBy(v => v.Name));
                listOfPriorities = con.returnAllPriorities("where STS=0");
                listOfTags = con.returnAllTags("where STS=0");
            }
            catch (Exception ex)
            {
                string exMessage = ex.Message;
            }
        }
        private void Data_Method()
        {
            View.frmData window = new View.frmData();
            window.ShowDialog();
        }

        private void GCalendar_Method()
        {
            View.GCalendar window = new View.GCalendar();
            SystemVars.FrmGCalendar = window;
            window.ShowDialog();
        }


        private void Search_Method()
        {
            filterText = "";
            if (searchIsActive)
            {
                title = "Inbox";
                visibleExit = "Hidden";
                visibleNew = "Visible";
                
                searchHeight = 0;
                searchIsActive = false;
            }
            else 
            {
                title = "Search";
                visibleExit = "Hidden";
                visibleNew = "Visible";

                searchHeight = 280;
                searchIsActive = true;
            }
        }

        private void NewTask_Method()
        {
            //method for creating a new task
            title = "New Task";
            Application.Current.MainWindow.Opacity = 0.5;
            View.frmTask window = new View.frmTask(null);
            SystemVars.FrmTaskWindow = window;
            window.ShowDialog();
            Application.Current.MainWindow.Opacity = 1;
            title = "Inbox";
            SystemVars.FrmTaskWindow = null;
            myContent = con.returnAllTasks(" where STS=0");
            myContent = new ObservableCollection<Model.Task>(myContent.OrderBy(x => x.Due).ThenBy(v => v.Name));
        }

        private void Loaded_Method()
        {
            start_Ini();
        }

    }
}
