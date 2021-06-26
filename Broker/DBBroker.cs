using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Completist.Broker
{
    //all conversations with the DB take route through this class
    public class DBBroker
    {
        public static SQLiteConnection connection; //just connection variable 
        public static DBBroker instance; //class constructor

        public static DBBroker openSession()
        {
            if (instance == null)
            {

                instance = new DBBroker();
            }
            return instance;
        }
        public void openConnection() //opens connection with DB
        {
            try
            {
                string baseDB = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Completist.db");

                connection = new SQLiteConnection(@"Data Source=" + baseDB + ";Version=3;");
                connection.Open();

            }
            catch (Exception e)
            {
                string message = e.Message;
            }
        }
        public void closeConnection() //closes connection with DB. Very Important 
        {
            try
            {
                connection.Close();
            }
            catch (Exception)
            {

            }
        }
        //The rest is just SQL queries acted upon the SQLite database
        public int TaskCounter()
        {
            try
            {
                string query = "SELECT * from [Count]";
                var command = connection.CreateCommand();
                int numCompleted = 0;
                command = new SQLiteCommand(query, connection);
                SQLiteDataReader reader = command.ExecuteReader();
                //string numCompleted;
                //string numCompleted = reader["NUMCOMPLETED"];
                while (reader.Read())
                {
                    var numCount = Convert.ToInt32(reader["NUMCOMPLETED"]);
                    return numCount;
                }

                return numCompleted;

            }
            catch (Exception)
            {
                return 1;
            }
        }
        public int CounterIncrement()
        {
            try
            {
                string query = "UPDATE Count SET NUMCOMPLETED = NUMCOMPLETED + 1";
                var command = connection.CreateCommand();
                int counterIncrement = 1;
                command = new SQLiteCommand(query, connection);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var countIncrement = Convert.ToInt32(reader["NUMCOMPLETED"]);
                    return countIncrement;
                }

                return counterIncrement;

            }
            catch (Exception)
            {
                return 1;
            }
        }
        public ObservableCollection<Model.Task> returnAllTasks(string condition = "")
        {
            try
            {
                string query = "SELECT * from [Tasks]" + condition;
                var command = connection.CreateCommand();

                command = new SQLiteCommand(query, connection);
                SQLiteDataReader reader = command.ExecuteReader();

                ObservableCollection<Model.Task> list = new ObservableCollection<Model.Task>();
                while (reader.Read())
                {
                    Model.Task x = new Model.Task();

                    x.Name = reader["NAME"].ToString();
                    x.Due = Convert.ToDateTime(reader["DUE"]);
                    x.Content = reader["CONTENT"].ToString();
                    x.TagList = returnTags(reader["TAGLIST"].ToString());
                    x.Status = Convert.ToInt32(reader["STS"]);
                    x.StrStatus = x.Status == 0 ? x.StrStatus = "New" : x.StrStatus = "Done";
                    x.StrTag = reader["TAGLIST"].ToString();
                    x.TaskID = Convert.ToInt32(reader["TaskID"]);

                    //x.StrDue = x.Due.ToShortDateString();
                    string pro = reader["PRIORITY"].ToString();
                    x.Priority = returnPriority("where NAME='" + pro + "'");
                    if (x.Due == DateTime.Today) 
                    {
                        var high = returnPriority("where NAME='" + "High Priority" + "'");
                        x.Priority = returnPriority("where NAME='" + pro + "'");
                        if (x.Priority.Name != high.Name)
                        {
                            x.Priority = high;
                            //if (this.handleTask(x, "EDIT", x.Name))
                            //{
                                
                            //}
                        }
                        x.StrDue = "Today";                        
                    }
                    else if (x.Due == DateTime.Today.AddDays(-1)) 
                    {
                        x.StrDue = "Yesterday";
                    }
                    else if (x.Due == DateTime.Today.AddDays(1)) 
                    {
                        x.StrDue = "Tomorrow";
                    }
                    else if (x.Due == DateTime.Today.AddDays(2)) 
                    {
                        x.StrDue = "Day after tomorrow";
                    }
                    else
                    {
                        var left = (x.Due - DateTime.Now).TotalDays;
                        int daysleft = Convert.ToInt32(left);
                        if (daysleft < 0)
                        {
                            x.StrDue = x.Due.ToShortDateString(); ;
                        }
                        else
                        {
                            x.StrDue = daysleft.ToString() + " days left";
                        }

                    }
                    list.Add(x);
                }

                return list;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return null;
            }
        }
        public ObservableCollection<Model.AllColors> returnColors()
        {
            try
            {
                string query = "SELECT * from [Colors]";
                var command = connection.CreateCommand();

                command = new SQLiteCommand(query, connection);
                SQLiteDataReader reader = command.ExecuteReader();

                ObservableCollection<Model.AllColors> list = new ObservableCollection<Model.AllColors>();
                while (reader.Read())
                {
                    Model.AllColors x = new Model.AllColors();
                    x.Name = reader["NAME"].ToString();
                    x.Color = reader["COLOR"].ToString();
                    
                    list.Add(x);
                }

                return list;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return null;
            }
        }
        public ObservableCollection<Model.ShareExchange> ReturnAllExchanges(/*string condition*/)
        {
            try
            {
                string query = "SELECT * from [STOCKEXCHANGE]" /*+ condition*/;
                var command = connection.CreateCommand();

                command = new SQLiteCommand(query, connection);
                SQLiteDataReader reader = command.ExecuteReader();

                ObservableCollection<Model.ShareExchange> list = new ObservableCollection<Model.ShareExchange>();
                while (reader.Read())
                {
                    ViewModel.MainWindowVM mainWindowVM = new ViewModel.MainWindowVM();
                    string strShareName;
                    Model.ShareExchange x = new Model.ShareExchange
                    {
                        
                        ShareName = strShareName = reader["StockName"].ToString(),
                        ShareIndex = Convert.ToInt32(reader["StockID"]),
                    };
                    mainWindowVM.FinanceResultsRESTAsync(strShareName, x);
                    list.Add(x);
                }

                return list;
            }
            catch (Exception ex)
            {
                string message = ex.Message; //read SQL error message using breakpoint + watch expression
                return null;
            }
        }
        public ObservableCollection<Model.Priority> returnAllPriorities(string condition)
        {
            try
            {
                string query = "SELECT * from [Priority] " + condition;
                var command = connection.CreateCommand();

                command = new SQLiteCommand(query, connection);
                SQLiteDataReader reader = command.ExecuteReader();

                ObservableCollection<Model.Priority> list = new ObservableCollection<Model.Priority>();
                while (reader.Read())
                {
                    Model.Priority x = new Model.Priority();

                    x.Name = reader["NAME"].ToString();
                    x.Color = reader["COLOR"].ToString();
                    x.Status = Convert.ToInt32(reader["STS"]);
                    x.StrStatus = x.Status == 0 ? x.StrStatus = "Active" : x.StrStatus = "Inactive";
                    x.StrName = "  " + x.Name + "  ";
                    list.Add(x);
                }

                return list;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return null;
            }
        }

        public ObservableCollection<Model.Tag> returnAllTags(string condition)
        {
            try
            {
                string query = "SELECT * from [Tag] " + condition;
                var command = connection.CreateCommand();

                command = new SQLiteCommand(query, connection);
                SQLiteDataReader reader = command.ExecuteReader();

                ObservableCollection<Model.Tag> list = new ObservableCollection<Model.Tag>();
                while (reader.Read())
                {
                    Model.Tag x = new Model.Tag();

                    x.Name = reader["NAME"].ToString();
                    x.Color = reader["COLOR"].ToString();
                    x.Status = Convert.ToInt32(reader["STS"]);
                    x.StrStatus = x.Status == 0 ? x.StrStatus = "Active" : x.StrStatus = "Inactive";
                    x.StrName = "  " + x.Name + "  ";
                    list.Add(x);
                }

                return list;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return null;
            }
        }

        public Model.Priority returnPriority(string condition)
        {
            try
            {
                string query = "SELECT * FROM [Priority] " + condition;
                var command = connection.CreateCommand();

                command = new SQLiteCommand(query, connection);
                SQLiteDataReader reader = command.ExecuteReader();

                Model.Priority x = new Model.Priority();
                while (reader.Read())
                {
                    x.Name = reader["NAME"].ToString();
                    x.Color = reader["COLOR"].ToString();
                    x.Status = Convert.ToInt32(reader["STS"]);
                    x.StrStatus = x.Status == 0 ? x.StrStatus = "Active" : x.StrStatus = "Inactive";
                    x.StrName = "  " + x.Name + "  ";
                }

                return x;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public Model.AllColors returnColor(string condition)
        {
            try
            {
                string query = "SELECT * FROM [colors] " + condition;
                var command = connection.CreateCommand();

                command = new SQLiteCommand(query, connection);
                SQLiteDataReader reader = command.ExecuteReader();

                Model.AllColors x = new Model.AllColors();
                while (reader.Read())
                {
                    x.Name = reader["NAME"].ToString();
                    x.Color = reader["COLOR"].ToString();
                }

                return x;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public ObservableCollection<Model.Tag> returnTags(string tags) 
        {
            try
            {
                ObservableCollection<Model.Tag> list = new ObservableCollection<Model.Tag>();

                if (String.IsNullOrEmpty(tags)) { return list; }

                string[] myArray = tags.Split(';');

                foreach (string item in myArray)
                {
                    if (!String.IsNullOrEmpty(item))
                    {
                        Model.Tag x = returnTag("where NAME='" + item.Trim() + "'");
                        list.Add(x);
                    }
                  
                }

                return list;
            }
            catch (Exception)
            {
                return new ObservableCollection<Model.Tag>();
            }
        }

        public Model.Tag returnTag(string condition)
        {
            try
            {
                string query = "SELECT * FROM [Tag] " + condition;
                var command = connection.CreateCommand();

                command = new SQLiteCommand(query, connection);
                SQLiteDataReader reader = command.ExecuteReader();

                Model.Tag x = new Model.Tag();
                while (reader.Read())
                {
                    x.Name = reader["NAME"].ToString();
                    x.Color = reader["COLOR"].ToString();
                    x.Status = Convert.ToInt32(reader["STS"]);
                    x.StrStatus = x.Status == 0 ? x.StrStatus = "Active" : x.StrStatus = "Inactive";
                    x.StrName = "  " + x.Name + "  ";
                }

                return x;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool editTag(Model.Tag tag)
        {
            try
            {
                string query = "UPDATE [TAG] SET STS=" + tag.Status + ", COLOR='" + tag.Color + "' WHERE NAME='" + tag.Name + "'";
                var command = connection.CreateCommand();

                command = new SQLiteCommand(query, connection);
                command.ExecuteScalar();

                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool editPriority(Model.Priority priority)
        {
            try
            {
                string query = "UPDATE [Priority] SET STS=" + priority.Status + ", COLOR='" + priority.Color + "' WHERE NAME='" + priority.Name + "'";
                var command = connection.CreateCommand();

                command = new SQLiteCommand(query, connection);
                command.ExecuteScalar();

                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool removePriority(Model.Priority priority)
        {
            try
            {
                string query = "DELETE FROM [PRIORITY] WHERE NAME='" + priority.Name + "'";
                var command = connection.CreateCommand();

                command = new SQLiteCommand(query, connection);
                command.ExecuteScalar();

                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool removeTag(Model.Tag tag)
        {
            try
            {
                string query = "DELETE FROM [TAG] WHERE NAME='" + tag.Name + "'";
                var command = connection.CreateCommand();

                command = new SQLiteCommand(query, connection);
                command.ExecuteScalar();

                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }


        public bool handleTask(Model.Task task, string action, string referenceName)
        {
            try
            {
                string query = "";
                switch (action)
                {
                    case "REMOVE":
                        query = "UPDATE [TASKS] SET STS=9 WHERE TASKID='" + task.TaskID + "'";
                        break;
                    case "UNDO":
                        query = "UPDATE [TASKS] SET STS=0 WHERE TASKID='" + task.TaskID + "'";
                        break;
                    case "DELETE":
                        query = "DELETE FROM [TASKS] WHERE TASKID='" + task.TaskID + "'"; //please revert if casualties
                        break;
                    case "COMPLETE":
                        query = "UPDATE [TASKS] SET STS=1 WHERE TASKID='" + task.TaskID + "'";
                        break;
                    case "EDIT":
                        string list = "";
                        foreach (Model.Tag item in task.TagList)
                        {
                            if (!String.IsNullOrEmpty(item.Name.Trim()))
                            {
                                list += item.Name + ";";
                            }
                            //else
                            //{
                            //    list = ";";
                            //}
                        }
                        query = "UPDATE [TASKS] SET NAME='" + task.Name + "',Due='"+task.Due+"' ,CONTENT='" + task.Content + "', PRIORITY='" + task.Priority.Name + "', TAGLIST='" + list + "' WHERE NAME='" + referenceName + "'";
                        break;
                    default:
                        break;
                }

                var command = connection.CreateCommand();

                command = new SQLiteCommand(query, connection);
                command.ExecuteScalar();

                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool createTask(Model.Task task)
        {
            try
            {
                string query = "INSERT INTO [TASKS] (NAME, PRIORITY, CONTENT, DUE, TAGLIST, STS) Values ('" + task.Name + "','" + task.Priority.Name + "','" + task.Content + "','" + task.Due.ToString("yyyy-MM-dd") + "','" + task.StrTag + "',0)";
                var command = connection.CreateCommand();

                command = new SQLiteCommand(query, connection);
                command.ExecuteScalar();

                return true;

            }
            catch (Exception ex)
            {
                string e = ex.Message;
                return false;
            }
        }
        public bool createTag(Model.Tag tag)
        {
            try
            {
                string query = "INSERT INTO [TAG] (NAME, STS, COLOR) Values ('" + tag.Name + "'," + tag.Status + ",'" + tag.Color + "')";
                var command = connection.CreateCommand();

                command = new SQLiteCommand(query, connection);
                command.ExecuteScalar();

                return true;

            }
            catch (Exception ex)
            {
                string e = ex.Message;
                return false;
            }
        }
        public bool createPriority(Model.Priority priority)
        {
            try
            {
                string query = "INSERT INTO [PRIORITY] (NAME, STS, COLOR) Values ('" + priority.Name + "'," + priority.Status + ",'" + priority.Color + "')";
                var command = connection.CreateCommand();

                command = new SQLiteCommand(query, connection);
                command.ExecuteScalar();

                return true;

            }
            catch (Exception ex)
            {
                string e = ex.Message;
                return false;
            }
        }
    }
}
