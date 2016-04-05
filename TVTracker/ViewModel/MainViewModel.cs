using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using TVTracker.Common;
using TVTracker.Model;

namespace TVTracker.ViewModel
{
    public class MainViewModel : ViewModelBase   //INotifyPropertyChanged
    {
        private bool isLoading;

        private bool loadingShowData;
        private int selectedTVShowIndex;
        private TVShow selectedTVShow;
        private ObservableCollection<TVShow> allShows;
        private Episode selectedEpisode;

        public bool LoadingShowData
        {
            get { return loadingShowData; }
            set
            {
                loadingShowData = value;
                RaisePropertyChanged(nameof(LoadingShowData));
            }
        }

        public ObservableCollection<TVShow> AllShows
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

        public int SelectedTVShowIndex
        {
            get { return selectedTVShowIndex; }
            set
            {
                //if (value >= 0)
                //{
                    selectedTVShowIndex = value;
                    
                    RaisePropertyChanged(nameof(SelectedTVShowIndex));
                    //SelectedTVShow = AllShows[value];
                //}
            }
        }

        public TVShow SelectedTVShow
        {
            get { return selectedTVShow; }
            set
            {
                //if (!isLoading)
                //{
                if (value != null)
                { 
                    selectedTVShow = value;
                    //RaisePropertyChanged(nameof(SelectedTVShow));
                    // DeleteTaskCommand.RaiseCanExecuteChanged();
                    if (!isLoading && selectedTVShow.TVMazeID > 0)
                        PopulateShowDetails();   //.GetAwaiter().GetResult();

                    RaisePropertyChanged(nameof(SelectedTVShow));
                }
            }
        }

        public Episode SelectedEpisode
        {
            get { return selectedEpisode; }
            set
            {
                selectedEpisode = value;
                RaisePropertyChanged(nameof(SelectedEpisode));
               // DeleteTaskCommand.RaiseCanExecuteChanged();
            }
        }
        /*****
        public DelegateCommand<object> DeleteTaskCommand { get; private set; }

        void DeleteTaskCommand_Execute(object arg)
        {
            DeleteTask();
        }
        bool DeleteTaskCommand_CanExecute(object arg)
        {
            Debug.WriteLine((SelectedTask != null && SelectedTask != BasicTask.Default) ? true : false);
            return (SelectedTask != null && SelectedTask != BasicTask.Default) ? true : false;
        }

        private void DeleteTask()
        {
            SelectedTaskList.Items.Remove(SelectedTask);
        }
        *****/
        public MainViewModel()
        {
            GetData();
        }

        public async void GetData()
        {
            this.SelectedTVShow = new TVShow();

            LoadingShowData = true;

            await GetAllShows();

            LoadingShowData = false;
        }

        public async Task GetAllShows()
        {
            isLoading = true;            

            AllShows = new ObservableCollection<TVShow>();

            TVShow show;

            try
            {
                ShowTrackerServiceReference.ShowTrackerServiceClient svc = new ShowTrackerServiceReference.ShowTrackerServiceClient();
                Collection<ShowTrackerServiceReference.ShowData> resp = await svc.GetAllShowsAsync();
                foreach (ShowTrackerServiceReference.ShowData wcfSD in resp)
                {
                    show = new TVShow(wcfSD);
                    //show.ShowUpdated += new ShowUpdatedEventHandler(ShowUpdated);
                    //show.UpdatedEvent += new EventHandler(ShowUpdated);
                    AllShows.Add(show);
                }

                //TODO: Put this back in when the XAML has been restored
                // hsShows.DataContext = AllShows;   

                //isLoading = false;

                //Now populate the Details and Episodes sections with the first show in the Shows list.
                //By setting this the VM chanegd property handler should call PopulateShowDetails().
                //SelectedTVShow.PropertyChanged -= PropertyChanged;
                SelectedTVShow = AllShows.ElementAt(0);
                //await PopulateShowDetails();
                await PopulateShowDetails();
                //SelectedTVShow.PropertyChanged += PropertyChanged;

                AllShows = new ObservableCollection<TVShow>(AllShows.OrderByDescending(x => x.LastWatchedDate).ThenBy(x => x.Title));

                isLoading = false;
            }
            catch (FaultException fex)
            {
                RaiseExceptionOccurred(new CustomException(this.GetType().Name, "GetAllShows", fex.ToString()));
            }
            catch (Exception ex)
            {
                RaiseExceptionOccurred(new CustomException(this.GetType().Name, "GetAllShows", ex.ToString()));
            }
        }

        public async Task PopulateShowDetails()
        {
            bool showUpdated = false;


            try
            {
                //We may have either the DB data and/or the API data already loaded depending on the source of the data.
                //e.g: If a show is clicked on for the first time in a session then we need to get both the DB and API data,
                //but if a new show is added via the API we already have all the data we need so no need to repeat it.
                if (!SelectedTVShow.DBDataLoaded || SelectedTVShow.SeasonData.Count == 0)
                {
                    //Show the progress ring (or keep showing it) while we load the DB and/or API data
                    LoadingShowData = true;

                    ShowTrackerServiceReference.ShowTrackerServiceClient svc = new ShowTrackerServiceReference.ShowTrackerServiceClient();
                    ShowTrackerServiceReference.ShowData resp = await svc.GetShowDataAsync(SelectedTVShow.TVMazeID);

                    //TVShow showData = new TVShow(resp);
                    SelectedTVShow.UpdateShowDetails(resp);

                    //TODO: WHAT DO WE DO IF ABOVE CODE DOESN'T WORK?

                    //if (show != null)
                    //{
                    //    ShowMessageDialog("No data found for " + SelectedTVShow.Title + " in database!", true);
                    //    return;
                    //}
                }

                //Disable the Watched checkbox if the episode has already been watched.
                foreach (Episode ep in SelectedTVShow.EpisodeData)
                    ep.CanSelect = (!(bool)ep.Watched);

                if (!SelectedTVShow.APIDataLoaded)
                {
                    //Show the progress ring (or keep showing it) if we need to load the DB and/or API data)
                    //if (!LoadingShowData)
                    LoadingShowData = true;

                    bool apiOK = await SelectedTVShow.GetTVMazeShowData();
                    if (apiOK)
                    {
                        SelectedTVShow.APIDataLoaded = true;
                        SelectedTVShow.ImageURL = SelectedTVShow.TVMazeSeries.image.medium.AbsoluteUri;

                        //If the show has any new seasons then update the DB.
                        if (selectedTVShow.APIDataChanged)
                        {
                            showUpdated = await SelectedTVShow.UpdateShow();
                        }
                    }
                    else
                        Helpers.ShowMessageDialog("No data found for " + SelectedTVShow.Title + " at TVmaze.com!", true);
                }

                LoadingShowData = false;

                if (showUpdated)
                {
                    RaisePropertyChanged(nameof(AllShows));
                    Helpers.ShowMessageDialog(SelectedTVShow.Title + " has been updated with new seasons and episodes.");
                }
            }
            catch (FaultException fex)
            {
                RaiseExceptionOccurred(new CustomException(this.GetType().Name, "PopulateShowDetails", fex.ToString()));
            }
            catch (Exception ex)
            {
                RaiseExceptionOccurred(new CustomException(this.GetType().Name, "PopulateShowDetails", ex.ToString()));
            }
        }

        private async Task<TVShow> GetShowData(int tvMazeID)
        {
            TVShow show = null;

            try
            {
                ShowTrackerServiceReference.ShowTrackerServiceClient svc = new ShowTrackerServiceReference.ShowTrackerServiceClient();
                ShowTrackerServiceReference.ShowData resp = await svc.GetShowDataAsync(tvMazeID);
                show = new TVShow(resp);
            }
            catch (FaultException fex)
            {
                RaiseExceptionOccurred(new CustomException(this.GetType().Name, "DeleteShow", fex.ToString()));
            }
            catch (Exception ex)
            {
                RaiseExceptionOccurred(new CustomException(this.GetType().Name, "DeleteShow", ex.ToString()));
            }

            return show;
        }

        public async Task<bool> AddNewShow(TVShow newShow)
        {
            //Convert new show to an instance of ShowTrackerServiceReference.ShowData and submit to WCF service.
            try
            {
                ShowTrackerServiceReference.ShowData showData = newShow.CreateSTShow();
                ShowTrackerServiceReference.ShowTrackerServiceClient svc = new ShowTrackerServiceReference.ShowTrackerServiceClient();
                ShowTrackerServiceReference.ServiceResult resp = await svc.AddShowAsync(showData);

                if (resp.Success)
                {
                    newShow.ID = resp.ID;
                    newShow.APIDataLoaded = false;
                    AllShows.Add(newShow);
                    AllShows = new ObservableCollection<TVShow>(AllShows.OrderBy(x => x.Title));
                    //SortShowsList("TITLE,ASC");
                    SelectedTVShow = newShow;

                    //May not need to do this
                    //RaisePropertyChanged(nameof(AllShows));
                }                    
                else
                    RaiseExceptionOccurred(new CustomException(this.GetType().Name, "AddNewShow", resp.Exception));
                return resp.Success;
            }
            catch (FaultException fex)
            {
                RaiseExceptionOccurred(new CustomException(this.GetType().Name, "AddNewShow", fex.ToString()));
                return false;
            }
            catch (Exception ex)
            {
                RaiseExceptionOccurred(new CustomException(this.GetType().Name, "AddNewShow", ex.ToString()));
                return false;
            }
        }

        //public async Task<bool> UpdateShow(int tvMazeID)
        //{
        //    try
        //    {
        //        ShowTrackerServiceReference.ShowTrackerServiceClient svc = new ShowTrackerServiceReference.ShowTrackerServiceClient();
        //        ShowTrackerServiceReference.ServiceResult resp = await svc.UpdateShowAsync(SelectedTVShow.TVMazeID, );
        //        if (resp.Success)
        //            RaisePropertyChanged(nameof(AllShows));
        //        else
        //            RaiseExceptionOccurred(new CustomException(this.GetType().Name, "UpdateShow", resp.Exception));
        //        return resp.Success;
        //    }
        //    catch (FaultException fex)
        //    {
        //        RaiseExceptionOccurred(new CustomException(this.GetType().Name, "UpdateShow", fex.ToString()));
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        RaiseExceptionOccurred(new CustomException(this.GetType().Name, "UpdateShow", ex.ToString()));
        //        return false;
        //    }
        //}

        public async Task<bool> DeleteShow(int tvMazeID)
        {
            try
            {
                ShowTrackerServiceReference.ShowTrackerServiceClient svc = new ShowTrackerServiceReference.ShowTrackerServiceClient();
                ShowTrackerServiceReference.ServiceResult resp = await svc.DeleteShowAsync(tvMazeID);
                if (resp.Success)
                {
                    //Show deleted from DB so now delete locally as well.
                    TVShow show = AllShows.FirstOrDefault(x => x.TVMazeID == tvMazeID);
                    allShows.Remove(show);
                    show = null;

                    RaisePropertyChanged(nameof(AllShows));
                }
                else
                    RaiseExceptionOccurred(new CustomException(this.GetType().Name, "DeleteShow", resp.Exception));
                return resp.Success;
            }
            catch (FaultException fex)
            {
                RaiseExceptionOccurred(new CustomException(this.GetType().Name, "DeleteShow", fex.ToString()));
                return false;
            }
            catch (Exception ex)
            {
                RaiseExceptionOccurred(new CustomException(this.GetType().Name, "DeleteShow", ex.ToString()));
                return false;
            }            
        }

        //public void RefreshShowsList()
        //{
            
        //    RaisePropertyChanged(nameof(AllShows));
        //}

        public string SortShowsList(string sortText)
        {
            bool sorted = false;
            string sortDirection = "";
            string property = sortText.Split(',')[0].ToUpper();
            string direction = sortText.Split(',')[1].ToUpper();

            switch (property)
            {
                case "TITLE":
                    if (direction == "ASC")
                    {
                        AllShows = new ObservableCollection<TVShow>(allShows.OrderByDescending(x => x.Title));
                        sortDirection = "DESC";
                        sorted = true;
                    }
                    else
                    {
                        AllShows = new ObservableCollection<TVShow>(AllShows.OrderBy(x => x.Title));
                        sortDirection = "ASC";
                        sorted = true;
                    }
                    break;

                case "SEASONS":
                    if (direction == "ASC")
                    {
                        AllShows = new ObservableCollection<TVShow>(allShows.OrderByDescending(x => x.Seasons));
                        sortDirection = "DESC";
                        sorted = true;
                    }
                    else
                    {
                        AllShows = new ObservableCollection<TVShow>(AllShows.OrderBy(x => x.Seasons));
                        sortDirection = "ASC";
                        sorted = true;
                    }
                    break;

                case "WATCHED":
                    if (direction == "ASC")
                    {
                        AllShows = new ObservableCollection<TVShow>(allShows.OrderByDescending(x => x.LastWatchedDate));
                        sortDirection = "DESC";
                        sorted = true;
                    }
                    else
                    {
                        AllShows = new ObservableCollection<TVShow>(AllShows.OrderBy(x => x.LastWatchedDate));
                        sortDirection = "ASC";
                        sorted = true;
                    }
                    break;

                case "EPISODE":
                    if (direction == "ASC")
                    {
                        AllShows = new ObservableCollection<TVShow>(allShows.OrderByDescending(x => x.LastWatchedEpisodeRef));
                        sortDirection = "DESC";
                        sorted = true;
                    }
                    else
                    {
                        AllShows = new ObservableCollection<TVShow>(AllShows.OrderBy(x => x.LastWatchedEpisodeRef));
                        sortDirection = "ASC";
                        sorted = true;
                    }
                    break;

                default:
                    break;

            }

            //If we sorted the collection then reset the selected show to the top one in the list and force a refresh.
            if (sorted)
            {
                SelectedTVShow = AllShows.ElementAt(0);

                //Trigger binding update on the shows collection to get it to show sorted.
                RaisePropertyChanged(nameof(AllShows));
            }

            return property + "," + sortDirection;
        }

        public string SortEpisodesList(string sortText)
        {
            if (selectedTVShow != null)
            {
                // TVShow show = SelectedTVShow;

                bool sorted = false;
                string sortDirection = "";
                string property = sortText.Split(',')[0].ToUpper();
                string direction = sortText.Split(',')[1].ToUpper();

                switch (property)
                {
                    case "TITLE":
                        if (direction == "ASC")
                        {
                            SelectedTVShow.EpisodeData = new ObservableCollection<Episode>(SelectedTVShow.EpisodeData.OrderByDescending(x => x.Title));
                            sortDirection = "DESC";
                            sorted = true;
                        }
                        else
                        {
                            SelectedTVShow.EpisodeData = new ObservableCollection<Episode>(SelectedTVShow.EpisodeData.OrderBy(x => x.Title));
                            sortDirection = "ASC";
                            sorted = true;
                        }
                        break;

                    case "EPISODE":
                        if (direction == "ASC")
                        {
                            SelectedTVShow.EpisodeData = new ObservableCollection<Episode>(SelectedTVShow.EpisodeData.OrderByDescending(x => x.EpisodeRef));
                            sortDirection = "DESC";
                            sorted = true;
                        }
                        else
                        {
                            SelectedTVShow.EpisodeData = new ObservableCollection<Episode>(SelectedTVShow.EpisodeData.OrderBy(x => x.EpisodeRef));
                            sortDirection = "ASC";
                            sorted = true;
                        }
                        break;

                    case "WATCHED":
                        if (direction == "ASC")
                        {
                            SelectedTVShow.EpisodeData = new ObservableCollection<Episode>(SelectedTVShow.EpisodeData.OrderByDescending(x => x.Watched));
                            sortDirection = "DESC";
                            sorted = true;
                        }
                        else
                        {
                            SelectedTVShow.EpisodeData = new ObservableCollection<Episode>(SelectedTVShow.EpisodeData.OrderBy(x => x.Watched));
                            sortDirection = "ASC";
                            sorted = true;
                        }
                        break;

                    case "DATEWATCHED":
                        if (direction == "ASC")
                        {
                            SelectedTVShow.EpisodeData = new ObservableCollection<Episode>(SelectedTVShow.EpisodeData.OrderByDescending(x => x.DateWatched));
                            sortDirection = "DESC";
                            sorted = true;
                        }
                        else
                        {
                            SelectedTVShow.EpisodeData = new ObservableCollection<Episode>(SelectedTVShow.EpisodeData.OrderBy(x => x.DateWatched));
                            sortDirection = "ASC";
                            sorted = true;
                        }
                        break;

                    default:
                        break;

                }


                //If we sorted the collection then reset the selected show to the top one in the list and force a refresh.
                if (sorted)
                {
                    //Trigger binding update on the shows collection to get it to show sorted.
                    //RaisePropertyChanged(nameof(SelectedTVshow.Ep));
                }
                return property + "," + sortDirection;
            }

            return sortText;


        }

        public event PropertyChangedEventHandler OnShowsListSorted;
        //public event PropertyChangedEventHandler OnStartedLoadingShow;
        //public event PropertyChangedEventHandler OnFinishedLoadingShow;
        public event PropertyChangedEventHandler VMPropertyChanged;
        private void InvokePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler changed;
            switch (propertyName)
            {
                case "ShowsListSorted":
                    changed = OnShowsListSorted;
                    break;

                //case "StartedLoadingShow":
                //    changed = OnStartedLoadingShow;
                //    break;

                //case "FinishedLoadingShow":
                //    changed = OnFinishedLoadingShow;
                //    break;

                //case "OnSettings":
                //    changed = OnSettings;
                //    break;
                default:
                    changed = VMPropertyChanged;
                    break;
            }
            if (changed != null)
                changed(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
