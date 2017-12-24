using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Xml;

namespace Attila2CK2 {
    class FactionInfo {

        private string id;
        private int numID;
        private string name;
        private string adjective;
        private Color color;
        private string ck2Title;
        private bool isNewTitle;
        private bool exists;
        private CK2Character owner;
        private CK2Dynasty dynasty;
        private HashSet<String> countiesOwned;

        public FactionInfo(string id, string name, string adjective, string ck2Title, bool isNewTitle) {
            this.id = id;
            this.name = name;
            this.adjective = adjective;
            this.ck2Title = ck2Title;
            this.color = Color.FromArgb(100, 100, 100);
            this.isNewTitle = isNewTitle;
            countiesOwned = new HashSet<String>();
            searchForFactionDynasty();
        }

        private void searchForFactionDynasty() {
            string conversionInfoPath = ImportantPaths.conversionInfoPath();
            string factionDynastiesPath = conversionInfoPath + "\\factions\\ck2Titles\\titlesDynastiesMap.csv";
            using (var factionDynastiesReader = new StreamReader(@factionDynastiesPath)) {
                while (!factionDynastiesReader.EndOfStream) {
                    var line = factionDynastiesReader.ReadLine();
                    var lineValues = line.Split(',');
                    string faction = lineValues[0];
                    string dynastyStr = lineValues[1];
                    if (faction == id) {
                        CK2Dynasty dynasty = new CK2Dynasty(dynastyStr);
                        this.dynasty = dynasty;
                    }
                }
            }
        }

        public void readFactionColor(ImportantPaths importantPaths) {
            string savegameFactionPath = importantPaths.getSavegameXMLPath() + "\\factions\\" + this.id + ".xml";
            XmlDocument doc = new XmlDocument();
            try {
                doc.Load(savegameFactionPath);
            }
            catch (Exception) {
                return;
            }
            XmlNode root = doc.DocumentElement;
            for (XmlNode node = root.FirstChild; node != null; node = node.NextSibling) {
                if (node.Attributes.Count == 0) {
                    continue;
                }
                XmlAttribute attr = node.Attributes[0];
                if (attr.Name == "type" && attr.InnerText == "FACTION_FLAG_AND_COLOURS") {
                    XmlNode redNode = node.FirstChild.NextSibling;
                    XmlNode greenNode = redNode.NextSibling;
                    XmlNode blueNode = greenNode.NextSibling;
                    int red = Int32.Parse(redNode.InnerText);
                    int green = Int32.Parse(greenNode.InnerText);
                    int blue = Int32.Parse(blueNode.InnerText);
                    this.color = Color.FromArgb(red, green, blue);
                    return;
                }
            }
        }

        public void readFactionNumID(ImportantPaths importantPaths) {
            string savegameFactionPath = importantPaths.getSavegameXMLPath() + "\\factions\\" + this.id + ".xml";
            XmlDocument doc = new XmlDocument();
            try {
                doc.Load(savegameFactionPath);
            }
            catch (Exception) {
                return;
            }
            XmlNode root = doc.DocumentElement;
            this.numID = Int32.Parse(root.FirstChild.FirstChild.InnerText);
        }
        
        public void addCounty(String county) { countiesOwned.Add(county); }
        public void setExists(bool exists) { this.exists = exists; }
        public void setOwner(CK2Character owner) { this.owner = owner; }
        public void setDynasty(CK2Dynasty dynasty) { this.dynasty = dynasty; }
        public string getID() { return id; }
        public CK2Dynasty getDynasty() { return dynasty; }
        public CK2Character getOwner() { return owner; }
        public HashSet<String> getOwnedCounties() { return countiesOwned; }
        public string getCK2Title() { return ck2Title; }
        public bool getIsNewTitle() { return isNewTitle; }
        public bool getExists() { return exists; }
        public Color getColor() { return color; }
        public string getName() { return name; }
        public string getAdjective() { return adjective; }
        public int getNumID() { return numID; }

    }
}
