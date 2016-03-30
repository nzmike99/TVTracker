using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace TVTracker.Common
{
    public static class Helpers
    {
        public static async void ShowMessageDialog(string message, bool OKOnly = true)
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

            //var btn = sender as Button;
            //btn.Content = $"Result: {result.Label} ({result.Id})";
        }
    }

    public class DateTimeFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return null;

            if (parameter == null)
                return value;

            DateTime dtm = (DateTime)value;

            if (dtm == DateTime.MinValue)
                return "";
            else
                if (dtm < DateTimeOffset.Now.DateTime.AddDays(-7))
                return dtm.ToString("dd/MM HH:mm");
            else
                return dtm.ToString("ddd HH:mm");

            // return string.Format((string)parameter, value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class StringFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return null;

            if (parameter == null)
                return value;

            return string.Format((string)parameter, value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
