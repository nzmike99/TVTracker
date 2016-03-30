using System;
using TVMazeAPI.Models;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TVTracker
{
    public sealed partial class CastCrewDialog : ContentDialog
    {

        public CastCrewDialog(MazeSeries series)
        {
           // GetCastInfo(tvMazeID);           

            this.InitializeComponent();

            this.DataContext = series;
            lvwCastList.ItemsSource = series.Cast;

        }

        //private async void GetCastInfo(int tvMazeID)
        //{
        //    try
        //    {
        //        MazeSeries series = await TVMazeAPI.TVMaze.GetSeries((uint)tvMazeID, FetchEpisodes: false, FetchCast: true);
        //        if (series != null)
        //        {
        //            //if (this.tvMazeSeries.image != null && this.tvMazeSeries.image.medium != null)
        //            //    this.ImageURL = this.tvMazeSeries.image.medium.AbsoluteUri;
        //            this.DataContext = series;
        //            lvwCastList.ItemsSource = series.Cast;


        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        string msg = ex.ToString();

        //    }
        //}

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
