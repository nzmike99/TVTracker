using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVTracker.ViewModel;
using Windows.UI.Xaml.Navigation;

namespace TVTracker.Model
{
    public class Season  : ViewModelBase
    {
        //public bool CollectionChanged { get; set; }

        private int id;
        public int ID
        {
            get { return id; }
            set
            {
                id = value;
                RaisePropertyChanged(nameof(ID));
                //if (value != id)
                //{
                //    id = value;
                //    NotifyPropertyChanged("ID");
                //}
            }
        }

        private int showID;
        public int ShowID
        {
            get { return showID; }
            set
            {
                //if (value != showID)
               // {
                    showID = value;
                    RaisePropertyChanged(nameof(ShowID));
                    //NotifyPropertyChanged("ShowID");
                //}
            }
        }

        private int seasonNumber;
        public int SeasonNumber
        {
            get { return seasonNumber; }
            set
            {
                //if (value != seasonNumber)
                //{
                    seasonNumber = value;
                    RaisePropertyChanged(nameof(SeasonNumber));
                    //NotifyPropertyChanged("SeasonNumber");

                //}
            }
        }

        private int episodes;
        public int Episodes
        {
            get { return episodes; }
            set
            {
                //if (value != episodes)
                //{
                    episodes = value;
                RaisePropertyChanged(nameof(Episodes));
               // NotifyPropertyChanged("Episodes");
                //}
            }
        }

        private bool watched;
        public bool Watched
        {
            get { return watched; }
            set
            {
                //if (value != watched)
                //{
                    watched = value;
                    RaisePropertyChanged(nameof(Watched));
                    //NotifyPropertyChanged("Watched");
                //}
            }
        }

        private DateTime lastUpdated;
        public DateTime LastUpdated
        {
            get { return lastUpdated; }
            set
            {
                //if (value != lastUpdated)
                //{
                    lastUpdated = value;
                RaisePropertyChanged(nameof(LastUpdated));
               // NotifyPropertyChanged("LastUpdated");
                //}
            }
        }

        public Season() { }

        public Season(ShowTrackerServiceReference.Season season)
        {
            //load fields from WCF service STSeason instance
            this.ID = season.ID;
            this.ShowID = season.ShowID;
            this.SeasonNumber = season.SeasonNumber;
            this.Episodes = season.Episodes;
            this.Watched = season.Watched;
            this.LastUpdated = season.LastUpdated;
        }


        //TODO: Not really sure what these are for - test with a breakpoint.
        //public override Task OnNavigatedFrom(NavigationEventArgs args)
        //{
        //    throw new NotImplementedException();
        //}

        //public override Task OnNavigatedTo(NavigationEventArgs args)
        //{
        //    throw new NotImplementedException();
        //}

        //public event PropertyChangedEventHandler PropertyChanged;
        //private void NotifyPropertyChanged(String propertyName)
        //{
        //    PropertyChangedEventHandler handler = PropertyChanged;
        //    if (null != handler)
        //    {
        //        handler(this, new PropertyChangedEventArgs(propertyName));
        //    }
        //}

    }
}
