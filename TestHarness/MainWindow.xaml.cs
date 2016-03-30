using System.Collections.Generic;
using System.Windows;
using System.IO;
using System.Net.Http;

namespace TestHarness
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<CalendarEntry> allShows;

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;
        }

        private async void txtLoadCalender_Click(object sender, RoutedEventArgs args)
        {
            string srcUri = txtCalenderURL.Text.Trim().ToLower();

            if (!string.IsNullOrEmpty(srcUri))
            {
                //TOOD: When adding this to TVTracker make a ViewModel containing the collection of CalendarEntry and do loading etc.
                

                allShows = new List<CalendarEntry>();
                string calData = "";

                if (srcUri.StartsWith("http://"))
                {
                    using (HttpClient client = new HttpClient())
                    using (HttpResponseMessage response = await client.GetAsync(srcUri))
                    using (HttpContent content = response.Content)
                    {
                        // Read the string.
                        calData = await content.ReadAsStringAsync();

                        // Display the result.
                        if (calData == null || calData.Length < 50)
                        {
                            MessageBox.Show("Unable to download " + srcUri);
                            //Console.WriteLine(result.Substring(0, 50) + "...");
                            return;
                        }
                    }
                }
                else
                {
                    calData = File.ReadAllText(srcUri);
                }

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
                            allShows.Add(calEntry);
                            // txtOutput.Text += "Added " + calEntry.Summary + "\r\n";
                        }
                        else
                            foundShow = false;
                    }
                    else
                        foundShow = false;
                } while (foundShow);

                //txtOutput.Text += "\r\nDONE!! Added " + allShows.Count.ToString() + " shows.";

                dgShows.ItemsSource = allShows;
                
            }
        }
    }
}
