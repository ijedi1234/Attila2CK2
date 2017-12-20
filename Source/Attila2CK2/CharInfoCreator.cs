using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace Attila2CK2 {
    class CharInfoCreator {

        private Dictionary<String, String> nameIDMap;
        List<Tuple<String, CK2Character>> charInfo;

        public CharInfoCreator(ImportantPaths importantPaths) {
            List<Tuple<String, String>> charXMLLocs = getCharXMLLocs(importantPaths);
            nameIDMap = generateNameIDMap();
            charInfo = generateCharInfo(importantPaths, charXMLLocs);
        }

        private List<Tuple<String, String>> getCharXMLLocs(ImportantPaths importantPaths) {
            string factionsPath = importantPaths.getSavegameXMLPath() + "\\factions";
            string[] factionXMLs = Directory.GetFiles(factionsPath);
            List<Tuple<String, String>> charXMLs = new List<Tuple<String, String>>();
            foreach (string xmlPath in factionXMLs) {
                string[] dirItems = xmlPath.Split('\\');
                string absoluteName = dirItems[dirItems.Length - 1];
                if (!absoluteName.StartsWith("att_fact_"))
                    continue;
                XmlDocument doc = new XmlDocument();
                try {
                    doc.Load(xmlPath);
                }
                catch (Exception) {
                    continue;
                }
                XmlNode root = doc.DocumentElement;
                for (XmlNode node = root.FirstChild; node != null; node = node.NextSibling) {
                    if (node.Attributes.Count == 0) {
                        continue;
                    }
                    XmlAttribute attr = node.Attributes[0];
                    if (attr.Name == "type" && attr.InnerText == "CHARACTER_ARRAY") {
                        for (XmlNode charNode = node.FirstChild; charNode != null; charNode = charNode.NextSibling) {
                            string path = charNode.Attributes[0].InnerText;
                            //Garrison leader
                            if (path.StartsWith("character/colonel")) continue;
                            string faction = absoluteName.Substring(0,absoluteName.Length - 4);
                            Tuple<String, String> tuple = Tuple.Create<String, String>(faction, path);
                            charXMLs.Add(tuple);
                        }
                    }
                }
            }
            return charXMLs;
        }

        private Dictionary<String, String> generateNameIDMap() {
            string nameMapFile = ImportantPaths.conversionInfoPath() + "\\characters\\namesMap.csv";
            Dictionary<String, String> nameIDMap = new Dictionary<String, String>();
            using (var nameMapReader = new StreamReader(nameMapFile)) {
                while (!nameMapReader.EndOfStream) {
                    var line = nameMapReader.ReadLine();
                    var lineValues = line.Split(',');
                    nameIDMap.Add(lineValues[0], lineValues[1]);
                }
            }
            return nameIDMap;
        }

        private List<Tuple<String, CK2Character>> generateCharInfo(ImportantPaths importantPaths, List<Tuple<String, String>> charXMLLocs) {
            List<Tuple<String, CK2Character>> charInfo = new List<Tuple<String, CK2Character>>();
            string savegamePath = importantPaths.getSavegameXMLPath();
            foreach (Tuple<String, String> charXMLLoc in charXMLLocs) {
                string name = "";

                XmlDocument doc = new XmlDocument();
                try {
                    doc.Load(savegamePath + "\\" + charXMLLoc.Item2);
                }
                catch (Exception) {
                    continue;
                }
                XmlNode root = doc.DocumentElement.FirstChild;
                for (XmlNode node = root.FirstChild; node != null; node = node.NextSibling) { 
                    if (node.Attributes.Count == 0) {
                        continue;
                    }
                    XmlAttribute attr = node.Attributes[0];
                    if (attr.Name == "type" && attr.InnerText == "CHARACTER_DETAILS") {
                        for (XmlNode detailNode = node.FirstChild; detailNode != null; detailNode = detailNode.NextSibling) {
                            if (detailNode.Attributes.Count == 0) {
                                continue;
                            }
                            XmlAttribute detailAttr = detailNode.Attributes[0];
                            if (detailAttr.Name == "type" && detailAttr.InnerText == "CHARACTER_NAME") {
                                name = extractName(detailNode);
                            }
                        }
                    }
                }
                CK2Character character = new CK2Character(name);
                Tuple<String, CK2Character> tuple = Tuple.Create<String, CK2Character>(charXMLLoc.Item1, character);
                charInfo.Add(tuple);
            }
            return charInfo;
        }

        private string extractName(XmlNode node) {
            XmlNode namesBlock = node.FirstChild;
            XmlNode firstNameNode = namesBlock.FirstChild.FirstChild;
            string firstNameObfuscated = firstNameNode.InnerText;
            string firstName = nameIDMap[firstNameObfuscated];
            return firstName;
        }

        public List<Tuple<String, CK2Character>> getCharInfo() { return charInfo; }

    }
}
