using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using TVMazeAPI.Models;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TVTracker
{
    public sealed partial class ManageShow : ContentDialog
    {
        public MazeSeries Show { get; private set; }

        public ManageShow()
        {
            this.InitializeComponent();

            this.DataContext = this;
            this.IsPrimaryButtonEnabled = false;
        }

        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNewShowName.Text.ToString().Trim()))
            {
                txtMessage.Text = "Please enter a show name!";
                return;
            }

            //TODO: Search TVMaze API for show name and return episodes but not cast/crew info.
            this.Show = await TVMazeAPI.TVMaze.FindSingleSeries(txtNewShowName.Text.Trim(),true,false);
            this.IsPrimaryButtonEnabled = (this.Show.name.ToLower() != "not found");
            if (this.Show.name.ToLower() != "not found")
                txtMessage.Text = "The show '" + this.Show.name + "' was found - we're in business!";
            else
                txtMessage.Text = "The show '" + txtNewShowName.Text.Trim() + "' was not found.";

        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            this.Show = null;
        }

    }
}
