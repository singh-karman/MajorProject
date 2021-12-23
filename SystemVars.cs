using Completist.Model;
using Completist.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Completist
{
    //System Class - I have defined it as static as none of these objects should at anytime should change. It is also public as this class needs to be accessed from everywhere within the app
    public static class SystemVars //I have stored values that I will be using in more than one namespace/interface
    {
        private static frmTask frmTaskWindow;
        private static frmPriority frmPriorityWindow;
        private static GCalendar GCalendarWindow;
        private static Priority selectedPriority;
        private static frmTag frmTagWindow;
        private static ObservableCollection<Tag> selectedTagList;
        private static frmDate frmDateWindow;
        private static DateTime selectedDateX;

        public static frmPriority FrmPriorityWindow { get => frmPriorityWindow; set => frmPriorityWindow = value; }
        public static GCalendar FrmGCalendar { get => GCalendarWindow; set => GCalendarWindow = value; }
        public static Priority SelectedPriority { get => selectedPriority; set => selectedPriority = value; }
        public static frmTag FrmTagWindow { get => frmTagWindow; set => frmTagWindow = value; }
        public static ObservableCollection<Tag> SelectedTagList { get => selectedTagList; set => selectedTagList = value; }
        public static frmTask FrmTaskWindow { get => frmTaskWindow; set => frmTaskWindow = value; }
        public static frmDate FrmDateWindow { get => frmDateWindow; set => frmDateWindow = value; }
        public static DateTime SelectedDateX { get => selectedDateX; set => selectedDateX = value; }
    }
}
