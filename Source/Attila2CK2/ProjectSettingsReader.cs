using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Attila2CK2 {
    class ProjectSettingsReader {

        private string savegameXMLLocation;

        public ProjectSettingsReader() {
            string settingsLoc = ImportantPaths.conversionInfoPath() + "\\settings.xml";
            XmlDocument doc = new XmlDocument();
            try {
                doc.Load(settingsLoc);
            }
            catch (Exception) {
                return;
            }
            XmlNode root = doc.DocumentElement;
            for (XmlNode node = root.FirstChild; node != null; node = node.NextSibling) {
                if (node.Name == "savegameXMLLocation") {
                    this.savegameXMLLocation = node.InnerText;
                }
            }
        }

        public string getSavegameXMLLocation() {
            return savegameXMLLocation;
        }
    }
}
