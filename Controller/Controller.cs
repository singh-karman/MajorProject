using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Completist.Broker;

namespace Completist.Controller //CONTROLLER - 
{
    //acts as a 'mediator' between DBbroker allowing to connect to the database on a local/hard disk connection 
    public class Controller
    {
        //same logic for all methods within this class
        public ObservableCollection<Model.Task> returnAllTasks(string condition = "")
        {
            //connection is oppened to the DB
            DBBroker.openSession().openConnection();
            try
            {   //if successful, the method is called in the DBbroker
                ObservableCollection<Model.Task> list = DBBroker.openSession().returnAllTasks(condition);
                return list;
            }
            catch (Exception)
            {
                return null; //if an error is encountered, it will return nothing- null. However, once everthing else is finished, I might get it to return a new list for a new user 
            }
            finally { DBBroker.openSession().closeConnection(); } //DO NOT DELETE. Crucial when interacting with DB that we close the connection otherwise the app will begin to hang. 
        }

        //copy past above comments but adjusting for different methods 
        public int TaskCounter()
        {
            DBBroker.openSession().openConnection();
            try
            {
                int completedTasks = DBBroker.openSession().TaskCounter();
                return completedTasks;
            }
            catch (Exception)
            {
                return 1;
            }
            finally { DBBroker.openSession().closeConnection(); }
        }
        public int CounterIncrement()
        {
            DBBroker.openSession().openConnection();
            try
            {
                int completedTasks = DBBroker.openSession().CounterIncrement();
                return completedTasks;
            }
            catch (Exception)
            {
                return 1;
            }
            finally { DBBroker.openSession().closeConnection(); }
        }
        public int CounterReset()
        {
            DBBroker.openSession().openConnection();
            try
            {
                int completedTasks = DBBroker.openSession().CounterReset();
                return completedTasks;
            }
            catch (Exception)
            {
                return 1;
            }
            finally { DBBroker.openSession().closeConnection(); }
        }
        public void TutorialReset()
        {
            DBBroker.openSession().openConnection();
            try
            {
                DBBroker.openSession().TutorialReset();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            finally { DBBroker.openSession().closeConnection(); }
        }
        public ObservableCollection<Model.ShareExchange> ReturnAllExchanges(/*string condition*/)
        {
            DBBroker.openSession().openConnection();
            try
            {
                ObservableCollection<Model.ShareExchange> list = DBBroker.openSession().ReturnAllExchanges(/*condition*/);
                return list;
            }
            catch (Exception)
            {
                return null;
            }
            finally { DBBroker.openSession().closeConnection(); }
        }
        public ObservableCollection<Model.Priority> returnAllPriorities(string condition)
        {
            DBBroker.openSession().openConnection();
            try
            {
                ObservableCollection<Model.Priority> list = DBBroker.openSession().returnAllPriorities(condition);
                return list;
            }
            catch (Exception)
            {
                return null;
            }
            finally { DBBroker.openSession().closeConnection(); }
        }
        public ObservableCollection<Model.Tag> returnAllTags(string condition)
        {
            DBBroker.openSession().openConnection();
            try
            {
                ObservableCollection<Model.Tag> list = DBBroker.openSession().returnAllTags(condition);
                return list;
            }
            catch (Exception)
            {
                return null;
            }
            finally { DBBroker.openSession().closeConnection(); }
        }
        public ObservableCollection<Model.AllColors> returnColors()
        {
            DBBroker.openSession().openConnection();
            try
            {
                ObservableCollection<Model.AllColors> list = DBBroker.openSession().returnColors();
                return list;
            }
            catch (Exception)
            {
                return null;
            }
            finally { DBBroker.openSession().closeConnection(); }
        }
        public Model.AllColors returnColor(string condition)
        {
            DBBroker.openSession().openConnection();
            try
            {
                Model.AllColors x = DBBroker.openSession().returnColor(condition);
                return x;
            }
            catch (Exception)
            {
                return null;
            }
            finally { DBBroker.openSession().closeConnection(); }
        }

        public bool editPriority(Model.Priority priority)
        {
            DBBroker.openSession().openConnection();
            try
            {
                bool res = DBBroker.openSession().editPriority(priority);
                return res;
            }
            catch (Exception)
            {
                return false;
            }
            finally { DBBroker.openSession().closeConnection(); }
        }
        public bool removePriority(Model.Priority priority)
        {
            DBBroker.openSession().openConnection();
            try
            {
                bool res = DBBroker.openSession().removePriority(priority);
                return res;
            }
            catch (Exception)
            {
                return false;
            }
            finally { DBBroker.openSession().closeConnection(); }
        }
        public bool editTag(Model.Tag tag)
        {
            DBBroker.openSession().openConnection();
            try
            {
                bool res = DBBroker.openSession().editTag(tag);
                return res;
            }
            catch (Exception)
            {
                return false;
            }
            finally { DBBroker.openSession().closeConnection(); }
        }

        public bool removeTag(Model.Tag tag)
        {
            DBBroker.openSession().openConnection();
            try
            {
                bool res = DBBroker.openSession().removeTag(tag);
                return res;
            }
            catch (Exception)
            {
                return false;
            }
            finally { DBBroker.openSession().closeConnection(); }
        }

        public bool handleTask(Model.Task task, string action, string referenceName)
        {
            DBBroker.openSession().openConnection();
            try
            {
                bool res = DBBroker.openSession().handleTask(task, action, referenceName);
                return res;
            }
            catch (Exception)
            {
                return false;
            }
            finally { DBBroker.openSession().closeConnection(); }
        }
        public bool createTask(Model.Task task)
        {
            DBBroker.openSession().openConnection();
            try
            {
                bool res = DBBroker.openSession().createTask(task);
                return res;
            }
            catch (Exception)
            {
                return false;
            }
            finally { DBBroker.openSession().closeConnection(); }
        }
        public bool createTag(Model.Tag tag)
        {
            DBBroker.openSession().openConnection();
            try
            {
                bool res = DBBroker.openSession().createTag(tag);
                return res;
            }
            catch (Exception)
            {
                return false;
            }
            finally { DBBroker.openSession().closeConnection(); }
        }
        public bool createPriority(Model.Priority priority)
        {
            DBBroker.openSession().openConnection();
            try
            {
                bool res = DBBroker.openSession().createPriority(priority);
                return res;
            }
            catch (Exception)
            {
                return false;
            }
            finally { DBBroker.openSession().closeConnection(); }
        }
    }
}
