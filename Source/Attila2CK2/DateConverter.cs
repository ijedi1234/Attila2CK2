using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Attila2CK2 {
    class DateConverter {

        //In Attila:
        //1147 <-> 395
        //For us:
        //currentDate <-> 769 startdate

        DateTime currentAttilaDate;

        public DateConverter(ImportantPaths paths) {
            string campaignModelPath = paths.getSavegameXMLPath() + "\\campaign_env\\campaign_model-0000.xml";
            findCurrentDate(campaignModelPath);
        }

        private void findCurrentDate(string campaignModelPath) {
            XmlDocument doc = new XmlDocument();
            try {
                doc.Load(campaignModelPath);
            }
            catch (Exception) { return; }
            XmlNode root = doc.DocumentElement;
            for (XmlNode node = root.FirstChild; node != null; node = node.NextSibling) {
                if (node.Attributes.Count == 0) {
                    continue;
                }
                XmlAttribute attr = node.Attributes[0];
                if (attr.Name == "type" && attr.InnerText == "CAMPAIGN_CALENDAR") {
                    for (XmlNode calendarNode = node.FirstChild; calendarNode != null; calendarNode = calendarNode.NextSibling) {
                        if (calendarNode.Name == "date2") {
                            readCurrentDate(calendarNode.InnerText);
                            return;
                        }
                    }
                }
            }
        }

        private void readCurrentDate(string dateStr) {
            string[] dateItems = dateStr.Split(' ');
            int season = Int32.Parse(dateItems[2]);
            int attilaYear = Int32.Parse(dateItems[3]);
            int month = DateConverter.convertSeason(season);
            //If I manage to shift back the startdate.
            int ck2Year = 395 + (attilaYear - 1147);
            this.currentAttilaDate = new DateTime(attilaYear, month, 1);
        }

        //Season -> Month
        public static int convertSeason(int season) {
            switch (season) {
                case 0:
                    return 1;
                case 1:
                    return 4;
                case 2:
                    return 7;
                case 3:
                    return 10;
                default:
                    return 12;
            }
        }

        public DateTime convertDate(string dateStr) {
            string[] dateItems = dateStr.Split(' ');
            int season = Int32.Parse(dateItems[2]);
            int attilaYear = Int32.Parse(dateItems[3]);
            int month = DateConverter.convertSeason(season);
            int year = 769 + (attilaYear - currentAttilaDate.Year);
            if (attilaYear == 0) return new DateTime(1, 1, 1);
            DateTime dt = new DateTime(year, month, 2);
            return dt;
        }

    }
}
