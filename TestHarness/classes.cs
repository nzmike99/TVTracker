using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*********************************
*
*
**********************************/
namespace TestHarness
{
    public class CalendarEntry
    {
        public string Show { get; set; }
        public string Title { get; set; }
        //public string UID { get; set; }        
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

            for (int i = 0;i < startTags.Count;i++)
            {
                tagName = startTags[i];
                if (i < startTags.Count-1)
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
                                this.URL = calData.Substring(s + tagName.Length, e - (s + tagName.Length));
                                break;

                            case "SUMMARY:":
                                this.Show = calData.Substring(s + tagName.Length, e - (s + tagName.Length));
                                break;

                            case "DESCRIPTION:":
                                this.Title = calData.Substring(s + tagName.Length, e - (s + tagName.Length));
                                this.Title = this.Title.Trim().Replace("\\", "");
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
