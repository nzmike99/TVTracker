using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using TVMazeAPI.Models;
using TVTracker.Common;
using TVTracker.ViewModel;

namespace TVTracker.Model
{
    public class TVShow : ViewModelBase     // IRaisePropertyChanged
    {
        //Indicates if all data in the database has been loaded via the web service.
        private bool dbDataLoaded;
        public bool DBDataLoaded
        {
            get { return dbDataLoaded; }
            set
            {
                if (value != dbDataLoaded)
                {
                    dbDataLoaded = value;
                    RaisePropertyChanged("DBDataLoaded");
                    //App.ViewModel.CollectionChanged = true;
                }
            }
        }

        //Indicates if all the data from the TVaze API has been downloaded.
        private bool apiDataLoaded;
        public bool APIDataLoaded
        {
            get { return apiDataLoaded; }
            set
            {
                if (value != apiDataLoaded)
                {
                    apiDataLoaded = value;
                    RaisePropertyChanged("APIDataLoaded");
                    //App.ViewModel.CollectionChanged = true;
                }
            }
        }

        //set to true if the data from the TVMaze APi is different to the current data
        //This would generally occur when a new season of the show is added.
        private bool apiDatachanged;
        public bool APIDataChanged
        {
            get { return apiDatachanged; }
            set
            {
                if (value != apiDatachanged)
                {
                    apiDatachanged = value;
                    RaisePropertyChanged("APIDataChanged");
                    //App.ViewModel.CollectionChanged = true;
                }
            }
        }

        private int id;
        public int ID
        {
            get { return id; }
            set
            {
                if (value != id)
                {
                    id = value;
                    RaisePropertyChanged("ID");
                }
            }
        }

        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                if (value != title)
                {
                    title = value;
                    RaisePropertyChanged(nameof(Title));
                }
            }
        }

        private int tvMazeID;
        public int TVMazeID
        {
            get { return tvMazeID; }
            set
            {
                if (value != tvMazeID)
                {
                    tvMazeID = value;
                    RaisePropertyChanged("TVMazeID");
                }
            }
        }

        private int imdbID;
        public int IMDBID
        {
            get { return imdbID; }
            set
            {
                if (value != imdbID)
                {
                    imdbID = value;
                    RaisePropertyChanged("IMDBID");
                }
            }
        }

        private int tvRageID;
        public int TVRageID
        {
            get { return tvRageID; }
            set
            {
                if (value != tvRageID)
                {
                    tvRageID = value;
                    RaisePropertyChanged("TVRageID");
                }
            }
        }

        //Removed for now as we don't really need to store the description when we're getting it from the TVMaze API.
        //private string description;
        //public string Description
        //{
        //    get { return description; }
        //    set
        //    {
        //        if (value != description)
        //        {
        //            description = value;
        //            RaisePropertyChanged("Description");
        //        }
        //    }
        //}

        private string imageURL;
        public string ImageURL
        {
            get { return imageURL; }
            set
            {
                if (value != imageURL)
                {
                    imageURL = value;
                    RaisePropertyChanged("ImageURL");
                }
            }
        }

        //TOOD: Add to STShow table and the WCF service
        private bool isActive;
        public bool IsActive
        {
            get { return isActive; }
            set
            {
                if (value != isActive)
                {
                    isActive = value;
                    RaisePropertyChanged("IsActive");
                    //App.ViewModel.CollectionChanged = true;
                }
            }
        }

        private int season;
        public int Seasons
        {
            get { return season; }
            set
            {
                if (value != season)
                {
                    season = value;
                    RaisePropertyChanged("Seasons");
                }
            }
        }

        private DateTime lastUpdated;
        public DateTime LastUpdated
        {
            get { return lastUpdated; }
            set
            {
                if (value != lastUpdated)
                {
                    lastUpdated = value;
                    RaisePropertyChanged("LastUpdated");
                }
            }
        }

        private DateTime lastWatchedDate;
        public DateTime LastWatchedDate
        {
            get { return lastWatchedDate; }
            set
            {
                //if (value != lastWatchedDate)
                //{
                lastWatchedDate = value;
                RaisePropertyChanged("LastWatchedDate");
                //}
            }
        }

        private int lastWatchedSeason;
        public int LastWatchedSeason
        {
            get { return lastWatchedSeason; }
            set
            {
                if (value != lastWatchedSeason)
                {
                    lastWatchedSeason = value;
                    RaisePropertyChanged("LastWatchedSeason");
                }
            }
        }

        private int lastWatchedEpisode;
        public int LastWatchedEpisode
        {
            get { return lastWatchedEpisode; }
            set
            {
                if (value != lastWatchedEpisode)
                {
                    lastWatchedEpisode = value;
                    RaisePropertyChanged("LastWatchedEpisode");
                }
            }
        }

        private string status;
        public string Status
        {
            get { return status; }
            set
            {
                if (value != status)
                {
                    status = value;
                    RaisePropertyChanged("Status");
                }
            }
        }

        private string summary;
        public string Summary
        {
            get { return summary; }
            set
            {
                if (value != summary)
                {
                    summary = value;
                    RaisePropertyChanged("Summary");
                }
            }
        }

        private string premiered;
        public string Premiered
        {
            get { return premiered; }
            set
            {
                if (value != premiered)
                {
                    premiered = value;
                    RaisePropertyChanged("Premiered");
                }
            }
        }

        private string network;
        public string Network
        {
            get { return network; }
            set
            {
                if (value != network)
                {
                    network = value;
                    RaisePropertyChanged("Network");
                }
            }
        }

        private int runtime;
        public int Runtime
        {
            get { return runtime; }
            set
            {
                if (value != runtime)
                {
                    runtime = value;
                    RaisePropertyChanged("Runtime");
                }
            }
        }

        private MazeSeries tvMazeSeries;
        public MazeSeries TVMazeSeries
        {
            get { return tvMazeSeries; }
            set
            {
                //if (value != tvMazeSeries)
                //{
                tvMazeSeries = value;
                RaisePropertyChanged("TVMazeSeries");
                //}
            }
        }

        private ObservableCollection<Season> seasonData;
        public ObservableCollection<Season> SeasonData
        {
            get { return seasonData; }
            set
            {
                if (value != seasonData)
                {
                    seasonData = value;
                    RaisePropertyChanged("SeasonData");
                }
            }
        }

        private ObservableCollection<Episode> episodeData;
        public ObservableCollection<Episode> EpisodeData
        {
            get { return episodeData; }
            set
            {
                if (value != episodeData)
                {
                    episodeData = value;
                    RaisePropertyChanged("EpisodeData");
                }
            }
        }


        //Helper properties

        // private string lastWatchedDateDisplay;
        //public string LastWatchedDateDisplay
        //{
        //    get
        //    {
        //        return lastWatchedDate > DateTime.MinValue ? lastWatchedDate.ToString("dd-MM-yy") : "";
        //        //RaisePropertyChanged("LastWatchedDateDisplay");
        //    }
        //}

        private string lastWatchedEpisodeRef;
        public string LastWatchedEpisodeRef
        {
            get
            {
                return lastWatchedEpisodeRef;
            }
            set
            {
                if (value != lastWatchedEpisodeRef)
                {
                    // RaisePropertyChanged("LastWatchedEpsiodeRef");
                    // lastWatchedEpisodeRef = value;

                    //return f this.lastWatchedSeason > 0 ? $"S{Convert.ToInt32(this.lastWatchedSeason).ToString("D2")}E{this.lastWatchedEpisode.ToString("D2")}" : "";
                    if (this.lastWatchedSeason > 0)
                        lastWatchedEpisodeRef = $"S{Convert.ToInt32(this.lastWatchedSeason).ToString("D2")}E{this.lastWatchedEpisode.ToString("D2")}";
                    else
                        lastWatchedEpisodeRef = "";

                    RaisePropertyChanged("LastWatchedEpisodeRef");
                }
            }
        }

        public override string ToString()
        {
            return Title;
            //this.lastWatchedSeason > 0 ? $"S{Convert.ToInt32(this.lastWatchedSeason).ToString("D2")}E{this.lastWatchedEpisode.ToString("D2")}" : "";
        }

        public Exception Exception { get; private set; }


        //public string DateLastWatchedDisplay
        //{
        //    get
        //    {
        //        if (lastWatchedDate == DateTime.MinValue)
        //            return "";
        //        else
        //            return lastWatchedDate.ToString("dd-MM-yy");
        //    }
        //}

        //public event PropertyChangedEventHandler PropertyChanged;
        //private void RaisePropertyChanged(String propertyName)
        //{
        //    PropertyChangedEventHandler handler = PropertyChanged;
        //    if (null != handler)
        //    {
        //        handler(this, new PropertyChangedEventArgs(propertyName));
        //    }
        //}


        //protected virtual void OnShowUpdated(ShowUpdatedEventArgs e)
        //{
        //    ShowUpdatedEventHandler handler = ShowUpdated;
        //    if (handler != null)
        //    {
        //        ShowUpdated(this, e);
        //    }
        //}

        //Allow null constructor as we need to instantiate SelectedShow in the MainPage initialisation
        public TVShow()
        {
            Initialise();
        }

        private void Initialise()
        {
            this.SeasonData = new ObservableCollection<Season>();
            this.EpisodeData = new ObservableCollection<Episode>();

            //SeasonData.CollectionChanged += season_CollectionChanged;
            EpisodeData.CollectionChanged += episode_CollectionChanged;
        }

        //private void season_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        //{
        //    if (e.NewItems != null)
        //    {
        //        foreach (Object item in e.NewItems)
        //        {
        //            (item as IRaisePropertyChanged).PropertyChanged += new PropertyChangedEventHandler(season_PropertyChanged);
        //        }
        //    }
        //    if (e.OldItems != null)
        //    {
        //        foreach (Object item in e.OldItems)
        //        {
        //            (item as IRaisePropertyChanged).PropertyChanged -= new PropertyChangedEventHandler(season_PropertyChanged);
        //        }
        //    }
        //}

        private void episode_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Object item in e.NewItems)
                {
                    //(item as IRaisePropertyChanged).
                    (item as Episode).PropertyChanged += new PropertyChangedEventHandler(episode_PropertyChanged);
                }
            }
            if (e.OldItems != null)
            {
                foreach (Object item in e.OldItems)
                {
                    (item as Episode).PropertyChanged -= new PropertyChangedEventHandler(episode_PropertyChanged);
                }
            }
        }

        //void season_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //var season = sender as Season;

        //Update the database now!

        //One note:
        //The ObservableCollection raises its change event as each item changes.
        //You should consider a method of batching the changes (probably using an ICommand)
        //}

        protected async void episode_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            { 
                var episode = sender as Episode;

                string propName = e.PropertyName;

                if (propName.ToLower() == "watched" && (bool)episode.Watched)
                {
                    //If all episodes have been watched in the season, set the Watched flag.
                    Season season = this.SeasonData.FirstOrDefault(x => x.SeasonNumber == episode.SeasonID);
                    int epsWatched = this.EpisodeData.Count(x => x.SeasonID == season.SeasonNumber && (bool)x.Watched == true);
                    if (epsWatched == season.Episodes)
                        season.Watched = true;

                    DateTime now = DateTimeOffset.Now.DateTime;   //.UtcNow;

                    this.LastWatchedSeason = episode.SeasonID;
                    this.LastWatchedEpisode = episode.EpisodeNumber;
                    this.LastWatchedEpisodeRef = this.ToString();
                    this.LastWatchedDate = now;


                    //EventArgs args = new EventArgs();
                    // OnUpdatedEvent(EventArgs.Empty);
                    //OnShowUpdated(new ShowUpdatedEventArgs(this.TVMazeID));

                    //RaisePropertyChanged(null);

                    ShowTrackerServiceReference.ShowTrackerServiceClient svc = new ShowTrackerServiceReference.ShowTrackerServiceClient();
                    ShowTrackerServiceReference.ServiceResult resp = 
                        await svc.UpdateEpisodeAsync(episode.ShowID, episode.SeasonID, episode.EpisodeNumber,true,now);
                    if (!resp.Success)
                        throw new Exception(resp.Exception.ToString());
                    //    RaisePropertyChanged(nameof(SelectedTVShow));
                    // return resp.Success;


                    //Update the database now - make sure this is async so it runs in the b/g


                }
            }
            catch (Exception ex)
            {
                this.Exception = ex;
            }

        }

        public bool UpdateShowDetails(ShowTrackerServiceReference.ShowData sd)
        {
            try
            {
                //Initialse the season and episode collections and set up property change events.
                Initialise();

                //this.ID = sd.ID;
                //this.Title = sd.Title;
                //this.TVMazeID = (int)sd.TVMazeID;
                this.Seasons = sd.Seasons;
                this.Status = sd.Status;
                this.LastUpdated = sd.LastUpdated;
                this.LastWatchedEpisode = (int)sd.LastWatchedEpisode;
                this.LastWatchedSeason = (int)sd.LastWatchedSeason;
                this.LastWatchedEpisodeRef = this.ToString();

                //DateTime dtmLastWatched = DateTime.MinValue;
                this.LastWatchedDate = (DateTime)sd.LastWatchedDate;

                Season season = null;
                foreach (ShowTrackerServiceReference.Season sn in sd.SeasonsData)
                {
                    season = new Season(sn);
                    this.SeasonData.Add(season);

                    //Get the episodes for the season\
                    //Episode episode = null;
                    //var episodes = context.STEpisodes.Where(x => x.ShowID == tvMazeID && x.SeasonID == sn.SeasonNumber);
                    //foreach (STEpisode ep in episodes)
                    //{
                    //    showData.EpisodesData.Add(new Episode(ep));
                    //}

                    //Load episodes - DO WE WANT TO STORE EPISODES IN EACH SEASON OR IN THEIR OWN COLLECTION IN SHOWDATA?????
                }

                Episode episode = null;
                //var episodes = context.STEpisodes.Where(x => x.ShowID == tvMazeID);
                foreach (ShowTrackerServiceReference.Episode ep in sd.EpisodesData)
                {
                    episode = new Episode(ep);
                    this.EpisodeData.Add(episode);
                }

                //Set our flags - at this stage we only have the DB data and not the API data.
                this.dbDataLoaded = true;
                this.apiDataLoaded = false;

                RaisePropertyChanged("");

                return true;
            }
            catch(Exception ex)
            {
                string msg = ex.ToString();
                return false;
            }

        }

        /// <summary>
        /// Create instance from a WCF ShowData object
        /// </summary>
        /// <param name="sd">Instance of ShowData from WCF ShowTracker service</param>
        public TVShow(ShowTrackerServiceReference.ShowData sd)
        {
            try
            { 
                //Initialse the season and episode collections and set up property change events.
                Initialise();

                this.ID = sd.ID;
                this.Title = sd.Title;
                this.TVMazeID = (int)sd.TVMazeID;
                this.Seasons = sd.Seasons;
                this.Status = sd.Status;
                this.LastUpdated = sd.LastUpdated;
                this.LastWatchedEpisode = (int)sd.LastWatchedEpisode;
                this.LastWatchedSeason = (int)sd.LastWatchedSeason;
                this.LastWatchedEpisodeRef = this.ToString();

                //DateTime dtmLastWatched = DateTime.MinValue;
                this.LastWatchedDate = (DateTime)sd.LastWatchedDate;

                Season season = null;
                foreach (ShowTrackerServiceReference.Season sn in sd.SeasonsData)
                {
                    season = new Season(sn);
                    this.SeasonData.Add(season);

                    //Get the episodes for the season\
                    //Episode episode = null;
                    //var episodes = context.STEpisodes.Where(x => x.ShowID == tvMazeID && x.SeasonID == sn.SeasonNumber);
                    //foreach (STEpisode ep in episodes)
                    //{
                    //    showData.EpisodesData.Add(new Episode(ep));
                    //}

                    //Load episodes - DO WE WANT TO STORE EPISODES IN EACH SEASON OR IN THEIR OWN COLLECTION IN SHOWDATA?????
                }

                Episode episode = null;
                //var episodes = context.STEpisodes.Where(x => x.ShowID == tvMazeID);
                foreach (ShowTrackerServiceReference.Episode ep in sd.EpisodesData)
                {
                    episode = new Episode(ep);
                    this.EpisodeData.Add(episode);
                }

                //Set our flags - at this stage we only have the DB data and not the API data.
                this.dbDataLoaded = true;
                this.apiDataLoaded = false;
            }
            catch (Exception ex)
            {
                this.Exception = ex;
            }

        }

        /// <summary>
        /// Create instance of TVShow from TVMaze API object (used when adding a new show)
        /// </summary>
        /// <param name="mazeShow">Instance of TVMazeAPI.Models.MazeSeries</param>
        public TVShow(MazeSeries sd)
        {
            try
            { 
                //Initialse the season and episode collections and set up property change events.
                Initialise();

                DateTime now = DateTimeOffset.Now.DateTime;

                // this.ID = sd.id;
                this.Title = sd.name;
                this.TVMazeID = (int)sd.id;
                this.Status = sd.status;
                this.LastUpdated = now;
                this.LastWatchedEpisode = 0;
                this.LastWatchedSeason = 0;

                //DateTime dtmLastWatched = DateTime.MinValue;
                //this.LastWatchedDate = (DateTime)sd.LastWatchedDate;

                this.Seasons = sd.Episodes.Max(x => x.season);

                //Create new collections fopr seasons and episodes
                //NB: Use Initialise() instead as it assigns the episode changed handler.
                //this.SeasonData = new ObservableCollection<Season>();
                //this.EpisodeData = new ObservableCollection<Episode>();
                //Initialise();

                Season season = null;
                Episode episode = null;
                foreach (TVMazeAPI.Models.MazeEpisode ep in sd.Episodes.OrderBy(x => x.ToString()))
                {
                    //Since there is no Season object in the TVMazeAPI we have to create the new season below rather 
                    //than having a constructor in our Season model we could call to load the TV maze season info.
                    if (ep.number == 1)
                    {
                        season = new Season();
                        season.SeasonNumber = ep.season;
                        season.ShowID = (int)sd.id;
                        season.LastUpdated = DateTimeOffset.Now.DateTime;
                        season.Episodes = sd.Episodes.Count(x => x.season == ep.season);
                        season.Watched = false;
                        this.SeasonData.Add(season);
                    }

                    //Create the episode from the episode being enumerated.
                    episode = new Episode(ep);
                    episode.ShowID = (int)sd.id;

                    //Update the last updated so show, seasons and episodes all have the same time
                    // episode.LastUpdated = now;

                    this.EpisodeData.Add(episode);  //TODO: once debugged call add with (new Episode(ep))

                }

                //At this stage we have all the data - although technically the DB flag should be false... 
                //however we set it to true as we have enough data to show.
                this.dbDataLoaded = true;
                this.apiDataLoaded = true;
            }
            catch (Exception ex)
            {
                this.Exception = ex;
            }

        }

        public ShowTrackerServiceReference.ShowData CreateSTShow()
        {
            ShowTrackerServiceReference.ShowData sd = null;

            DateTime now = DateTimeOffset.Now.DateTime;

            try
            {
                sd = new ShowTrackerServiceReference.ShowData();
                sd.Title = this.title;
                sd.TVMazeID = this.tvMazeID;
                sd.Seasons = this.Seasons;
                sd.Status = this.status;
                sd.Active = true;
                sd.LastUpdated = now;

                //Seasons
                sd.SeasonsData = new ObservableCollection<ShowTrackerServiceReference.Season>();
                foreach (Season s in this.SeasonData)
                {
                    sd.SeasonsData.Add(new ShowTrackerServiceReference.Season
                    {
                        ShowID = s.ShowID,
                        SeasonNumber = s.SeasonNumber,
                        Episodes = this.EpisodeData.Count(x => x.SeasonID == s.SeasonNumber),
                        Watched = false,
                        LastUpdated = now
                    });
                }

                sd.EpisodesData = new ObservableCollection<ShowTrackerServiceReference.Episode>();
                foreach (Episode e in this.EpisodeData)
                {
                    sd.EpisodesData.Add(new ShowTrackerServiceReference.Episode
                    {
                        ShowID = e.ShowID,
                        SeasonID = e.SeasonID,
                        EpisodeNumber = e.EpisodeNumber,
                        EpisodeRef = e.EpisodeRef,
                        Title = e.Title,
                        Watched = false,
                        LastUpdated = now
                    });
                }

                //Do we submit the webservice from here or do it in the calling code???
               // return sd;
            }
            catch (Exception ex)
            {
                this.Exception = ex;
            }

            return sd;

        }

        public async Task<bool> GetTVMazeShowData()
        {
            try
            { 
                this.tvMazeSeries = await TVMazeAPI.TVMaze.GetSeries((uint)this.tvMazeID, FetchEpisodes: true);
                if (this.tvMazeSeries != null)
                {
                    if (this.tvMazeSeries.image != null && this.tvMazeSeries.image.medium != null)
                        this.ImageURL = this.tvMazeSeries.image.medium.AbsoluteUri;
                    this.Summary = this.tvMazeSeries.summary == null ? "" : this.tvMazeSeries.summary;
                    this.Premiered = this.tvMazeSeries.premiered == null ? "" : this.tvMazeSeries.premieredDisplay;
                    this.Network = this.tvMazeSeries.network == null ? "" : this.tvMazeSeries.network.name;
                    this.Runtime = this.tvMazeSeries.runtime.Value;

                    //Check if we have the same numbner of seasons as the show - if not, update from TVMaze APi data.
                    int seasons = this.tvMazeSeries.Episodes.Max(x => x.season);
                    if (this.Seasons < seasons)
                    {
                        this.APIDataChanged = true;                      
                    }

                }
                return (this.tvMazeSeries != null);
            }
            catch (Exception ex)
            {
                this.Exception = ex;
                return false;
            }
        }

        /// <summary>
        /// Add new seasons and episodes for this show.
        /// </summary>
        /// <returns>Boolean indicating success or failure from the web service</returns>
        public async Task<bool> UpdateShow()
        {
            bool retVal = false;
            Season season = null;
            Episode episode = null;
            int episodes;
            int showID;
            ObservableCollection<ShowTrackerServiceReference.STSeason> stSeasons = new ObservableCollection<ShowTrackerServiceReference.STSeason>();
            ObservableCollection<ShowTrackerServiceReference.STEpisode> stEpisodes = new ObservableCollection<ShowTrackerServiceReference.STEpisode>();
            List<Season> newSeasons = new List<Season>();
            List<Episode> newEpisodes = new List<Episode>();
            DateTime now = DateTimeOffset.Now.DateTime;

            foreach (TVMazeAPI.Models.MazeEpisode ep in this.tvMazeSeries.Episodes.Where(x => x.season > this.Seasons))
            {
                //Since there is no Season object in the TVMazeAPI we have to create the new season below rather 
                //than having a constructor in our Season model we could call to load the TV maze season info.
                if (ep.number == 1)
                {
                    episodes = this.tvMazeSeries.Episodes.Count(x => x.season == ep.season);
                    showID = (int)this.tvMazeSeries.id;

                    season = new Season();
                    season.SeasonNumber = ep.season;
                    season.ShowID = showID;
                    season.LastUpdated = now;
                    season.Episodes = episodes;
                    season.Watched = false;

                    //Add new season to our local collection which gets applied if the new data is updated in the DB ok.
                    newSeasons.Add(season);                    

                    //Add a new instance of the web service EF season object to the collection.
                    stSeasons.Add(new ShowTrackerServiceReference.STSeason
                    {
                        ShowID = showID,
                        SeasonNumber = ep.season,
                        Episodes = episodes,
                        Watched = false,
                        LastUpdated = now
                    });
                }

                //Create the new episode and add to our local collection which gets applied if the new data is updated in the DB ok.
                episode = new Episode(ep);
                episode.ShowID = (int)this.tvMazeSeries.id;
                episode.CanSelect = true;       //Nothing watched since we're adding new so user can select episode.
                newEpisodes.Add(episode);

                //Add a new instance of the web service EF episode object to the collection.
                stEpisodes.Add(new ShowTrackerServiceReference.STEpisode
                {
                    ShowID = (int)this.tvMazeSeries.id,
                    SeasonID = ep.season,
                    EpisodeNumber = ep.number,
                    EpisodeRef = ep.episodeReference,
                    Title = ep.name,
                    Watched = false,
                    LastUpdated = now
                });
            }

            //Call the Updateshow service to update the DB.
            try
            {
                ShowTrackerServiceReference.ShowTrackerServiceClient svc = new ShowTrackerServiceReference.ShowTrackerServiceClient();
                ShowTrackerServiceReference.ServiceResult resp = await svc.UpdateShowAsync(tvMazeID, stSeasons, stEpisodes);
                if (resp.Success)
                {
                    //We only want to update the local data if we know the show has been successfully updated in the database.
                    this.Seasons += newSeasons.Count();

                    foreach (Season s in newSeasons)
                        this.SeasonData.Add(s);

                    foreach (Episode e in newEpisodes)
                        this.EpisodeData.Add(e);

                    retVal = true;
                }
                else
                    RaiseExceptionOccurred(new CustomException(this.GetType().Name, "UpdateShow", resp.Exception));
                //return resp.Success;
            }
            catch (FaultException fex)
            {
                RaiseExceptionOccurred(new CustomException(this.GetType().Name, "UpdateShow", fex.ToString()));
                retVal = false;
            }
            catch (Exception ex)
            {
                RaiseExceptionOccurred(new CustomException(this.GetType().Name, "UpdateShow", ex.ToString()));
                retVal = false;
            }

            

            return retVal;
        }

    } 
}
    

