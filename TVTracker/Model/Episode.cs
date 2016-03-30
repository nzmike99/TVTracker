using System;
using TVTracker.ViewModel;

namespace TVTracker.Model
{
    public class Episode : ViewModelBase
    {

        // public virtual ShowTracker.TVShow Show { get; set; }

        //public bool CollectionChanged { get; set; }

        private int id;
        public int ID
        {
            get { return id; }
            set
            {
                ////if (value != id)
                //{
                    id = value;
                    RaisePropertyChanged("ID");
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
                //{
                    showID = value;
                    RaisePropertyChanged("ShowID");
                //}
            }
        }


        private int seasonID;
        public int SeasonID
        {
            get { return seasonID; }
            set
            {
                //if (value != seasonID)
                //{
                    seasonID = value;
                    RaisePropertyChanged("SeasonID");
               // }
            }
        }

        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                //if (value != title)
                //{
                    title = value;
                    RaisePropertyChanged("Title");
               // }
            }
        }

        private int episodeNumber;
        public int EpisodeNumber
        {
            get { return episodeNumber; }
            set
            {
                //if (value != episodeNumber)
                //{
                    episodeNumber = value;
                    RaisePropertyChanged("EpisodeNumber");
                //}
            }
        }

        //TODO: WE DON'T REALLY NEED THIS AS IT CAN JUST BE DERIVED BY TOSTRING()
        private string episodeRef;
        public string EpisodeRef
        {
            get { return episodeRef; }
            set
            {
                //if (value != episodeRef)
               // {
                    episodeRef = value;
                    RaisePropertyChanged("EpisodeRef");
                //}
            }
        }

        private bool? watched;
        public bool? Watched
        {
            get { return watched ?? false; }
            set
            {
                if (value != watched)
                {
                    watched = value;
                    // this.CanSelect = (!(bool)watched);
                    if ((bool)watched)
                    {
                        this.DateWatched = DateTimeOffset.Now.DateTime;   // .UtcNow;

                        // this.Show.LastWatchedEpisode = this.EpisodeNumber;
                        //this.Show.LastWatchedSeason = this.SeasonID;
                        //this.Show.LastWatchedEpisodeRef = this.EpisodeRef;
                    }
                    else
                        this.DateWatched = DateTime.MinValue;

                    RaisePropertyChanged("Watched");

                }
            }
        }

        private DateTime dateWatched;
        public DateTime DateWatched
        {
            get { return dateWatched; }
            set
            {
                dateWatched = value;
                RaisePropertyChanged("DateWatched");
            }
        }

        public string DateWatchedDisplaySub()
        {
            if (dateWatched == DateTime.MinValue)
                return "";
            else
                if (dateWatched < DateTimeOffset.Now.DateTime.AddDays(-7))
                return dateWatched.ToString("dd/MM HH:mm");
            else
                return dateWatched.ToString("ddd HH:mm");
        }

        private string dateWatchedDisplay;
        public string DateWatchedDisplay
        {
            get
            {
                if (dateWatched == DateTime.MinValue)
                    dateWatchedDisplay = "";
                else
                    if (dateWatched < DateTimeOffset.Now.DateTime.AddDays(-7))
                    dateWatchedDisplay = dateWatched.ToString("dd/MM/yy HH:mm");
                else
                    dateWatchedDisplay = dateWatched.ToString("ddd HH:mm");

                return dateWatchedDisplay;
            }
        }


        private DateTime lastUpdated;
        public DateTime LastUpdated
        {
            get { return lastUpdated; }
            set
            {
                lastUpdated = value;
                RaisePropertyChanged("LastUpdated");
            }
        }

        //Helper Properties


        //Used to indicate whether the Watched checkbox is enabled.  If the episode was previously watched
        //and related data saved to the DB then we don't want tyo allow this be checked.
        //TODO: In the settings/edit page for a series, allow this setting to be overriden or episodes for a season to be reset.
        private bool canSelect;
        public bool CanSelect
        {
            get { return canSelect; }
            set
            {
                // canSelect = (!(bool)this.Watched);
                //if (value != canSelect)
                //{
                    canSelect = value;
                    RaisePropertyChanged("CanSelect");
                //}
            }
        }

        public override string ToString()
        {
            return this.episodeRef;   // $"S{Convert.ToInt32(seasonid).ToString("D2")}E{number.ToString("D2")}";
        }


        //TOOD: Do seomthing with these when I work out what they're for
        //public override Task OnNavigatedFrom(NavigationEventArgs args)
        //{
        //    throw new NotImplementedException();
        //}

        //public override Task OnNavigatedTo(NavigationEventArgs args)
        //{
        //    throw new NotImplementedException();
        //}

        public Episode() { }

        public Episode(ShowTrackerServiceReference.Episode episode)   //, TVShow show)
        {
            //Assign reference to the instance of TVshow that contains this episode.
            //this.Show = show;

            ID = episode.ID;
            ShowID = episode.ShowID;
            SeasonID = episode.SeasonID;
            EpisodeNumber = episode.EpisodeNumber;
            episodeRef = episode.EpisodeRef;
            Title = episode.Title;
            Watched = episode.Watched;
            LastUpdated = episode.LastUpdated;

            DateWatched = episode.DateWatched.HasValue ? episode.DateWatched.Value : DateTime.MinValue;
        }

        public Episode(TVMazeAPI.Models.MazeEpisode episode)    //, TVShow show)
        {
            //Assign reference to the instance of TVshow that contains this episode.
            //this.Show = show;

            //this.showID = (int)episode.id;
            SeasonID = episode.season;
            EpisodeNumber = episode.number;
            EpisodeRef = $"S{Convert.ToInt32(seasonID).ToString("D2")}E{episodeNumber.ToString("D2")}";
            Title = episode.name;
            Watched = false;
            LastUpdated = DateTimeOffset.Now.DateTime;
        }

        //public event PropertyChangedEventHandler PropertyChanged;
        //private void RaisePropertyChanged(String propertyName)
        //{
        //    PropertyChangedEventHandler handler = PropertyChanged;
        //    if (null != handler)
        //    {
        //        handler(this, new PropertyChangedEventArgs(propertyName));
        //    }
        //}

    }
}
