using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Attila2CK2 {
    class CK2Dynasty {

        private static int newIDnum = deriveFirstDynastyID();

        private int id;
        private string name;

        public CK2Dynasty() {
            this.name = "Holder";
            this.id = CK2Dynasty.newIDnum;
            CK2Dynasty.newIDnum++;
        }

        public CK2Dynasty(string name) {
            string[] nameSections = name.Trim().Split(' ');
            this.name = nameSections[nameSections.Length - 1];
            this.id = CK2Dynasty.newIDnum;
            CK2Dynasty.newIDnum++;
        }

        private static int deriveFirstDynastyID() {
            string settingsLoc = ImportantPaths.conversionInfoPath() + "\\settings.xml";
            XmlDocument doc = new XmlDocument();
            try {
                doc.Load(settingsLoc);
            }
            catch (Exception) {
                return 1000001;
            }
            XmlNode root = doc.DocumentElement;
            for (XmlNode node = root.FirstChild; node != null; node = node.NextSibling) {
                if (node.Name == "firstDynastyID") {
                    return Int32.Parse(node.InnerText);
                }
            }
            return 1000001;
        }

        public int getID() { return id; }
        public string getName() { return name; }

    }
}
