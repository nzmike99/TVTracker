using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

using TVTracker.Model;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TVTracker.UserControls
{
    public sealed partial class CalendarEntryControl : UserControl
    {
        public DateTime ShowDate { get; set; }
        public List<CalendarEntry> CalendarEntries { get; set; }
        public List<string> ShowsList { get; set; }

        public string AirDateDisplay
        {
            get { return ShowDate.ToString("ddd dd/MM"); }
        }

        public CalendarEntryControl(DateTime showDate, List<CalendarEntry> calendarEntries)
        {
            this.InitializeComponent();

            this.ShowsList = new List<string>();
            this.ShowDate = showDate;
            this.CalendarEntries = calendarEntries;
           // this.Height = txt

            foreach (CalendarEntry ce in CalendarEntries)
                ShowsList.Add(ce.Show + ": " + ce.Episode);            

            this.DataContext = this;
            lvwShows.ItemsSource = ShowsList;

        }
    }
}
