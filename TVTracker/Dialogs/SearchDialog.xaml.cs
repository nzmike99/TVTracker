using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TVTracker.Model;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TVTracker
{
    public sealed partial class SearchDialog : ContentDialog
    {
        private IEnumerable<TVShow> _allShows;
        public IEnumerable<TVShow> FoundShows { get; private set; }
        public TVShow SelectedShow { get; private set; }

        public SearchDialog(IEnumerable<TVShow> allShows)
        {
            this.InitializeComponent();

            this._allShows = allShows;
            this.DataContext = this;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNewShowName.Text.ToString().Trim()))
            {
                txtMessage.Text = "Please enter a show name!";
                return;
            }

            //TODO: Search TVMaze API for show name and return episodes but not cast/crew info.
            FoundShows = _allShows.Where(x => x.Title.ToLower().Trim().StartsWith(txtNewShowName.Text.ToString().Trim()));

            if (FoundShows.Count() == 0)
                txtMessage.Text = "'" + txtNewShowName.Text.ToString().Trim() + "' not in current shows";
            else
                showsList.ItemsSource = FoundShows;


            //this.Show = this.Parent.GetValue( await TVMazeAPI.TVMaze.FindSingleSeries(txtNewShowName.Text.Trim(), true, false);
            //this.IsPrimaryButtonEnabled = (this.Show.name.ToLower() != "not found");
            //if (this.Show.name.ToLower() != "not found")
            //    txtMessage.Text = "The show '" + txtNewShowName.Text.Trim() + " was found - we're in business!";
            //else
            //    txtMessage.Text = "The show '" + txtNewShowName.Text.Trim() + "' was not found. ";

        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = e.AddedItems?.FirstOrDefault();
            SelectedShow = (TVShow)item;
            //txtMessage.Text = SelectedShow.Title;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            SelectedShow = null;
        }
    }
}
