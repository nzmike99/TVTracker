using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVTracker.ViewModel;

namespace TVTracker.Model
{
    public class CalendarEntry : ViewModelBase
    {
        public string Show { get; set; }
        public string Title { get; set; }
        public string Episode { get; set; }        
        public DateTime AirDate { get; set; }
        public string Timezone { get; set; }
        public string URL { get; set; }

        public CalendarEntry(string calData)
        {
            List<string> startTags = new List<string>()
            {
                "DTSTART;", "URL:", "SUMMARY:", "DESCRIPTION:", "DTSTAMP:"
            };

            int s = 0;
            int e = 0;
            string tagName;
            string nextTagName;

            for (int i = 0; i < startTags.Count; i++)
            {
                tagName = startTags[i];
                if (i < startTags.Count - 1)
                    nextTagName = startTags[i + 1];
                else
                    //We're done, no more tags so break out of loop
                    break;

                s = calData.IndexOf(tagName, s);
                if (s >= e)
                {
                    e = calData.IndexOf(nextTagName, s + tagName.Length);
                    if (e > s + tagName.Length)
                    {
                        switch (tagName)
                        {
                            case "DTSTART;":
                                //First, extract the timezone description
                                int ss = calData.IndexOf("=", s + tagName.Length);
                                if (ss > s)
                                {
                                    ss++;
                                    int ee = calData.IndexOf(":", ss);
                                    this.Timezone = calData.Substring(ss, ee - ss);

                                    //Now, get the next 15 characters after the ':' which is the show datetime.
                                    this.AirDate = DateTime.ParseExact(calData.Substring(ee + 1, 15), "yyyyMMddTHHmmss", CultureInfo.InvariantCulture);

                                }
                                //else
                                ///WTF do we do here ???

                                break;

                            case "URL:":
                                //For some reason the URLs often have one or more CRLFs in them so ensure these are expunged.
                                this.URL = calData.Substring(s + tagName.Length, e - (s + tagName.Length));
                                this.URL = this.URL.Replace(Environment.NewLine, "");
                                break;

                            case "SUMMARY:":
                                string show = calData.Substring(s + tagName.Length, e - (s + tagName.Length));
                                string[] bits = show.Split(':');
                                this.Show = bits[0].Trim().Replace("\\", "");
                                if (bits.Count() > 1)
                                    this.Episode = bits[1].Trim();
                                break;

                            case "DESCRIPTION:":
                                string title = calData.Substring(s + tagName.Length, e - (s + tagName.Length));
                                this.Title = title.Trim().Replace("\\", "");
                                break;

                            default:
                                break;
                        }
                        s = e;
                    }

                }
            }
        }
    }
}
