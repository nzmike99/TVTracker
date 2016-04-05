using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TVTracker.Model;
using TVTracker.UserControls;
using TVTracker.ViewModel;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TVTracker
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShowCalendar : Page
    {
        public ICommand MyCommand { get; set; }
        CalendarViewModel cvm;

        public ShowCalendar()
        {
            this.InitializeComponent();

           // pgbLoading.IsActive = true;
            ShowBackbutton(true);

            cvm = new CalendarViewModel();

            this.DataContext = cvm;

            LoadCalendarWebPage();
            //PopulateControls();

            pgbLoading.IsEnabled = false;
            //pgbLoading.Visibility = Visibility.Collapsed;

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private async void LoadCalendarWebPage()
        {

            try
            {
                var html = await Windows.Storage.PathIO.ReadTextAsync(@"ms-appx:///ShowCalendar.html");
                if (html == null)
                    html = "<html><h2>Cannot find ShowCalendar.html</h2></html>";
                else
                {
                    //Now build the HTML for each day
                    List<CalendarEntry> entries = null;
                    DateTime sd = DateTime.Now.AddDays(-7);

                    //TODO: Need to work out how to work out how many columns we can fit on the Lumia screen.                    
                    //Could look into doing it landscape so we could get 7 days across?
                    int defaultRows = 7;

                    string weekStart = "<tr>\r\n<table>\r\n";
                    string weekEnd = "</table>\r\n</tr>\r\n";
                    string content = weekStart;
                    string dayHtml = "";

                    for (int d = 0; d < 28; d++)
                    {
                        //At end of each row add all controls.
                        if (d > 0 && d % defaultRows == 0)
                        {
                            //End current table row and start new one
                            content += weekEnd + weekStart;
                        }

                        dayHtml = CreateHTMLCalendarEntry(sd);
                        content += dayHtml;

                        //Increment our show date
                        sd = sd.AddDays(1);
                    }

                    //Close off the row for the last week.
                    content += weekEnd;

                    html = html.Replace("[CONTENT]", content);

                    webView1.NavigateToString(html);
                }

            }
            catch(Exception ex)
            {

            }
            
        }

        private string CreateHTMLCalendarEntry(DateTime sd)
        {
            List<CalendarEntry> entries = cvm.GetShowsForDate(sd);

            string html = "<td>\r\n<table class=\"day";

            //If we're on the current date add the extra style so the day is highlighted.
            if (sd.Date == DateTime.Now.Date)
                html += " today";
            html += "\">\r\n<tr><th>" + sd.ToString("ddd dd/MM") + "</th></tr>\r\n";

            foreach (CalendarEntry ce in entries.OrderBy(x => x.Show).ThenBy(x => x.Episode))
            {
                html += "<tr><td><a href=\"" + ce.URL + "\" title=\"" + ce.Title + "\">" + ce.Show + ": " + ce.Episode + "</a></td></tr>\r\n";
            }
            html += "</table>\r\n</td>";
            return html;
        }

        private void PopulateControls()
        {
            List<CalendarEntry> entries = null;
            CalendarEntryControl ceCtrl = null;
            DateTime sd = DateTime.Now.AddDays(-7);

            //TODO: Need to work out howto work out how many columns we can fit on the Lumia screen.
            List<CalendarEntryControl> ceCtrls = new List<CalendarEntryControl>();
            int defaultRows = 7;
            int ctrlHeight = 0;
            int maxHeight = 0;
            string calEntry = "";
            string html = "";
            for (int d = 0; d < 28; d++)
            {
                entries = cvm.GetShowsForDate(sd);
                html += CreateHTMLCalendarEntry(sd);



                //ceCtrl = new CalendarEntryControl(sd, entries);
                ceCtrl.VerticalAlignment = VerticalAlignment.Stretch;
                ctrlHeight = 25 + (entries.Count * 25);
                if (ctrlHeight > maxHeight)
                    maxHeight = ctrlHeight;
                ceCtrls.Add(ceCtrl);
                //grdCalendar.Items.Add(ceCtrl);                

                //At end of each row add all controls.
                if (d > 0 && d % defaultRows == 0)
                {
                    //END CURRENT TABLE ROW

                    foreach (CalendarEntryControl ctrl in ceCtrls)
                    {
                        ceCtrl.Height = maxHeight;
                        //grdCalendar.Items.Add(ceCtrl);
                    }

                    //Reset collection and maximu height for next row of controls.
                    ceCtrls = new List<CalendarEntryControl>();
                    maxHeight = 0;
                }

                //Increment our show date
                sd = sd.AddDays(1);
            }

        }



            /*****
                        for(int d = 0; d < 28; d += defaultRows)
            {
                entries = cvm.GetShowsForDate(sd);
                ceCtrl = new CalendarEntryControl(sd, entries);
                ceCtrl.VerticalAlignment = VerticalAlignment.Stretch;
                ctrlHeight = 25 + (entries.Count * 25);
                if (ctrlHeight > maxHeight)
                    maxHeight = ctrlHeight;
                ceCtrls.Add(ceCtrl);
                //grdCalendar.Items.Add(ceCtrl);                

                //At end of each row add all controls.
                if (d > 0 && d % defaultRows == 0)
                {
                    foreach (CalendarEntryControl ctrl in ceCtrls)
                    {
                        ceCtrl.Height = maxHeight;
                        grdCalendar.Items.Add(ceCtrl);
                    }

                    //Reset collection and maximu height for next row of controls.
                    ceCtrls = new List<CalendarEntryControl>();
                    maxHeight = 0;
                }

                //Increment our show date
                sd = sd.AddDays(1);
            }
            ********/



        //TODO: If adding other pages could create an abstract Page class and use this event and the handler. 

        private void ShowBackbutton(bool showBackButton)
        {
            var currentView = SystemNavigationManager.GetForCurrentView();
            if (showBackButton)
            {
                currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                currentView.BackRequested += backButton_Tapped;
            }
            else
            {
                currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                currentView.BackRequested -= backButton_Tapped;
            }
        }

        private void backButton_Tapped(object sender, BackRequestedEventArgs e)
        {
            // Remove back button and handler before going back to main page.

            if (Frame.CanGoBack)
            {
                ShowBackbutton(false);
                Frame.GoBack();
            }
        }

        private void ShowEpisodesHeading_Tapped(object sender, TappedRoutedEventArgs e)
        {
            TextBlock txt = (TextBlock)sender;
            txt.Tag = cvm.SortShowsList(txt.Tag.ToString());
        }


    }
}
