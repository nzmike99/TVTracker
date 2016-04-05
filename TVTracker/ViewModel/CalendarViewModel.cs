using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TVTracker.Common;
using TVTracker.Model;

namespace TVTracker.ViewModel
{
    public class CalendarViewModel : ViewModelBase
    {
        //To allow the constructor to run an Async operation the NotifyTaskCompletion class was copied from
        //https://msdn.microsoft.com/en-us/magazine/dn605875.aspx
        public NotifyTaskCompletion<bool> taskcomplete { get; private set; }

        public ObservableCollection<CalendarEntry> allShows { get; private set; }
        public bool DataLoaded { get; private set; }

        public ObservableCollection<CalendarEntry> AllShows
        {
            get
            {
                return allShows;
            }
            set
            {
                allShows = value;
                RaisePropertyChanged(nameof(AllShows));
            }
        }

        private bool loadingShowData;
        public bool LoadingCalendarData
        {
            get { return loadingShowData; }
            set
            {
                loadingShowData = value;
                RaisePropertyChanged(nameof(LoadingCalendarData));
            }
        }

        public CalendarViewModel()
        {
            GetData();
            // taskcomplete = new NotifyTaskCompletion<bool>(DownloadCalendar());
            //DownloadCalendar();
        }

        public async void GetData()
        {
            LoadingCalendarData = true;

            await DownloadCalendar();

            LoadingCalendarData = false;
        }

        public async Task DownloadCalendar()
        {
            DataLoaded = false;

            //TODO: Need to put this URL in the settings
            string url = "http://api.tvmaze.com/ical/followed?token=Ha37NIWW9YublRrMkjbUCTt5WIDwiwpZ";

            AllShows = new ObservableCollection<CalendarEntry>();
            string calData = "";          

            HttpClient client = new HttpClient();
            var t = client.GetStringAsync(url);   
            calData = t.Result;

            // Display the result.
            if (calData == null || calData.Length < 50)
            {
                DataLoaded = false;
            }
            else
            {

                CalendarEntry calEntry;
                string showData = "";
                bool foundShow = true;
                string startTag = "BEGIN:VEVENT";
                string endTag = "END:VEVENT";
                int s = 0;
                int e = 0;

                do
                {
                    s = calData.IndexOf(startTag, e);
                    if (s > e)
                    {
                        e = calData.IndexOf(endTag, s + startTag.Length);
                        if (e > s + startTag.Length)
                        {
                            showData = calData.Substring(s + startTag.Length, e - (s + startTag.Length));
                            calEntry = new CalendarEntry(showData);
                            AllShows.Add(calEntry);
                        }
                        else
                            foundShow = false;
                    }
                    else
                        foundShow = false;
                } while (foundShow);



                //}

                this.DataLoaded = true;
            }

        }             

        private async Task<string> DownloadWebPage(string url)
        {
            string calData = "";

            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(url))
            using (HttpContent content = response.Content)
            {
                // Read the string.
                calData = await content.ReadAsStringAsync();

                // Display the result.
                if (calData == null || calData.Length < 50)
                {
                    this.DataLoaded = false;
                    return "";
                }
            }

            return calData;
        }

        public List<CalendarEntry> GetShowsForDate(DateTime showDateTime)
        {
            DateTime startDate = new DateTime(showDateTime.Year, showDateTime.Month, showDateTime.Day);
            DateTime endDate = startDate.AddDays(1);

            //Create an empty list up front in case we have no followed shows for the specified date.
            List<CalendarEntry> entries = new List<CalendarEntry>();
            entries.AddRange(this.AllShows.Where(x => x.AirDate >= startDate && x.AirDate < endDate));
            return entries;
        }

        public string SortShowsList(string sortText)
        {
            bool sorted = false;
            string sortDirection = "";
            string property = sortText.Split(',')[0].ToUpper();
            string direction = sortText.Split(',')[1].ToUpper();

            switch (property)
            {
                case "SHOW":
                    if (direction == "ASC")
                    {
                        AllShows = new ObservableCollection<CalendarEntry>(allShows.OrderByDescending(x => x.Show));
                        sortDirection = "DESC";
                        sorted = true;
                    }
                    else
                    {
                        AllShows = new ObservableCollection<CalendarEntry>(AllShows.OrderBy(x => x.Show));
                        sortDirection = "ASC";
                        sorted = true;
                    }
                    break;

                case "EPISODE":
                    if (direction == "ASC")
                    {
                        AllShows = new ObservableCollection<CalendarEntry>(allShows.OrderByDescending(x => x.Episode));
                        sortDirection = "DESC";
                        sorted = true;
                    }
                    else
                    {
                        AllShows = new ObservableCollection<CalendarEntry>(AllShows.OrderBy(x => x.Episode));
                        sortDirection = "ASC";
                        sorted = true;
                    }
                    break;

                case "AIRDATE":
                    if (direction == "ASC")
                    {
                        AllShows = new ObservableCollection<CalendarEntry>(allShows.OrderByDescending(x => x.AirDate));
                        sortDirection = "DESC";
                        sorted = true;
                    }
                    else
                    {
                        AllShows = new ObservableCollection<CalendarEntry>(AllShows.OrderBy(x => x.AirDate));
                        sortDirection = "ASC";
                        sorted = true;
                    }
                    break;

                default:
                    break;

            }

            return property + "," + sortDirection;
        }
    }
}
