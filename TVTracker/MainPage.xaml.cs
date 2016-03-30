using System;
using System.Linq;
using System.Threading.Tasks;
using TVMazeAPI.Models;
using TVTracker.Common;
using TVTracker.Model;
using TVTracker.ViewModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TVTracker
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private MainViewModel vm;
        private TVShow selectedShow;

        public MainPage()
        {
            this.InitializeComponent();

            vm = (MainViewModel)this.DataContext;

            vm.ExceptionOccurred += VMExceptionOccurred;

            //vm.OnStartedLoadingShow += VMStartedLoadingShowEventHandler;
            //vm.OnFinishedLoadingShow += VMFinishedLoadingShowEventHandler;

            //hsDetails.DataContext = vm.SelectedTVShow;

        }

        private async void VMExceptionOccurred(object sender, CustomEventArgs e)
        {
            string msg = string.Format("Oops, something broke in {0}.{1}!\r\n\r\n{2}",
                                       e.CustomException.ClassName,
                                       e.CustomException.MethodName,
                                       e.CustomException.Details);
            await ShowMessageDialog(msg);
        }

        #region Hub section event handlers
        private void Hub_SectionHeaderClick(object sender, HubSectionHeaderClickEventArgs e)
        {

        }
        #endregion

        #region Shows list and flyout handlers
        private void ShowsList_ItemClick(object sender, ItemClickEventArgs e)
        {
            selectedShow = (TVShow)e.ClickedItem;
            vm.SelectedTVShow = selectedShow;
        }

        private void ShowsList_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);
            flyoutBase.ShowAt(senderElement);
        }

        private void FlyoutEditButton_Click(object sender, RoutedEventArgs e)
        {
            TVShow show = (TVShow)(e.OriginalSource as FrameworkElement).DataContext;

            //this datacontext is probably some object of some type T
        }

        private void FlyoutRefreshButton_Click(object sender, RoutedEventArgs e)
        {
            TVShow show = (TVShow)(e.OriginalSource as FrameworkElement).DataContext;

            //this datacontext is probably some object of some type T
        }

        private async void FlyoutDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            TVShow show = (TVShow)(e.OriginalSource as FrameworkElement).DataContext;
            if (show != null)
            {
                if (await ShowMessageDialog("Are you sure you want to delete " + show.Title + " from TVTracker?",false) == true)
                {
                    await vm.DeleteShow(show.TVMazeID);
                }
            }
        }
        #endregion

        #region Button and App bar handlers        
        private async void AddShowButton_Click(object sender, RoutedEventArgs e)
        {
            ManageShow manageShowDialog = new ManageShow();
            await manageShowDialog.ShowAsync();
            MazeSeries tvMazeShow = manageShowDialog.Show;
            if (tvMazeShow != null)
            {
                //TODO: If show already in colelction then show error. Could do it in ManageShow but need access to the shows collection.
                if (vm.AllShows.Count(x => x.TVMazeID == tvMazeShow.id) > 0)
                {
                    await ShowMessageDialog(tvMazeShow.name + " cannot be added as it already exists.");
                    return;
                }

                //TVShow newShow = new TVShow(tvMazeShow);
                await vm.AddNewShow(new TVShow(tvMazeShow));

            }
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            //IEnumerable<TVShow> showTitles = vm.AllShows.Select(x => x.Title).ToList();
            SearchDialog searchDialog = new SearchDialog(vm.AllShows);
            await searchDialog.ShowAsync();
            if (searchDialog.SelectedShow != null)
                vm.SelectedTVShow = searchDialog.SelectedShow;
        }

        //TODO: Could set reminders for new shows in calendar - e.g: "Bosch S02 starts 13/03 in the USA"
        private void WaitButton_Click(object sender, RoutedEventArgs e)
        {
           // pgbLoadingShow.IsActive = !pgbLoadingShow.IsActive;
            //pgbLoadingShow.IsEnabled = pgbLoadingShow.IsActive;
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            if (await ShowMessageDialog("Refresh all shows from database?", false) == true)
            {
                vm.GetData();    //.RefreshShowsList();
            }
        }

        private async void btnViewCrew_Click(object sender, RoutedEventArgs e)
        {
            pgbLoadingShow.IsActive = true;

            try
            {           
                MazeSeries series = await TVMazeAPI.TVMaze.GetSeries((uint)vm.SelectedTVShow.TVMazeID, FetchEpisodes: false, FetchCast: true);
                if (series != null)
                {
                    CastCrewDialog castDialog = new CastCrewDialog(series);
                    await castDialog.ShowAsync();
                }

            }
            catch (Exception ex)
            {
                string msg = ex.ToString();

            }
            finally
            {
                pgbLoadingShow.IsActive = false;
            }

        }
        #endregion

        #region Grid column click handlers (for sorting)
        private void ShowsGridHeading_Tapped(object sender, TappedRoutedEventArgs e)
        {
            TextBlock txt = (TextBlock)sender;
            txt.Tag = vm.SortShowsList(txt.Tag.ToString());
        }

        private void ShowEpisodesHeading_Tapped(object sender, TappedRoutedEventArgs e)
        {
            TextBlock txt = (TextBlock)sender;
            txt.Tag = vm.SortEpisodesList(txt.Tag.ToString());
        }
        #endregion

        private async Task<bool> ShowMessageDialog(string message, bool OKOnly = true)
        {
            var dialog = new Windows.UI.Popups.MessageDialog(message);

            if (OKOnly)
            {
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("OK") { Id = 0 });
            }
            else
            {
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("Yes") { Id = 0 });
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("No") { Id = 1 });
            }

            //if (Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily != "Windows.Mobile")
            //{
            //    // Adding a 3rd command will crash the app when running on Mobile !!!
            //    dialog.Commands.Add(new Windows.UI.Popups.UICommand("Maybe later") { Id = 2 });
            //}

            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 1;

            var result = await dialog.ShowAsync();
            return ((int)result.Id == dialog.DefaultCommandIndex);

            //return (result.Id == 0);
            //var btn = sender as Button;
            //btn.Content = $"Result: {result.Label} ({result.Id})";
        }

    }
}



//private void VMStartedLoadingShowEventHandler(object sender, System.ComponentModel.PropertyChangedEventArgs e)
//{
//Event callback from my ViewModel
//hsDetails.Header = "...LOADING...";

//pgbLoadingShow = new ProgressRing();
//pgbLoadingShow.Visibility = Visibility.Visible;
// pgbLoadingShow.IsActive = true;

//}

//private void VMFinishedLoadingShowEventHandler(object sender, System.ComponentModel.PropertyChangedEventArgs e)
//{
//Event callback from my ViewModel
// hsDetails.Header = "Details";

//pgbLoadingShow.IsActive = false;
// pgbLoadingShow.Visibility = Visibility.Collapsed;
// pgbLoadingShow.
//}