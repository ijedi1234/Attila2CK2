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
        Dictionary<int, string> esfID2Job;

        public CharInfoCreator(ImportantPaths importantPaths) {
            esfID2Job = new Dictionary<int, string>();
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
                    if (attr.Name == "path" && attr.InnerText.StartsWith("government")) {
                        string governmentLoc = attr.InnerText;
                        governmentLoc = governmentLoc.Replace("/", "\\");
                        readGovernmentInfo(importantPaths, governmentLoc);
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
                int esfID = 0;
                int treeID = 0;
                int nodeCount = 0;
                bool male = false;
                for (XmlNode node = root.FirstChild; node != null; node = node.NextSibling) {
                    if (nodeCount == 0 && node.Name != "rec") esfID = Int32.Parse(node.InnerText);
                    if (node.Name != "rec") nodeCount++;
                    if (node.Attributes.Count == 0) {
                        continue;
                    }
                    XmlAttribute attr = node.Attributes[0];
                    if (attr.Name == "type" && attr.InnerText == "CHARACTER_DETAILS") {
                        int detailCount = 0;
                        bool i1AryFound = false;
                        int countedUAfterPol = 0;
                        for (XmlNode detailNode = node.FirstChild; detailNode != null; detailNode = detailNode.NextSibling) {
                            if (detailCount == 11) {
                                male = (detailNode.Name == "yes");
                            }
                            if (detailNode.Name == "i1_ary") {
                                i1AryFound = true;
                            }
                            if (detailNode.Name == "u" && i1AryFound) {
                                countedUAfterPol++;
                                if (countedUAfterPol == 5) {
                                    treeID = Int32.Parse(detailNode.InnerText);
                                }
                            }
                            if (detailNode.Name != "rec") {
                                detailCount++;
                            }
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
                CK2Character character = new CK2Character(name, esfID, treeID, male);
                Tuple<String, CK2Character> tuple = Tuple.Create<String, CK2Character>(charXMLLoc.Item1, character);
                charInfo.Add(tuple);
            }
            return charInfo;
        }

        public string extractName(XmlNode node) {
            XmlNode namesBlock = node.FirstChild;
            XmlNode firstNameNode = namesBlock.FirstChild.FirstChild;
            string firstNameObfuscated = firstNameNode.InnerText;
            string firstName = nameIDMap[firstNameObfuscated];
            return firstName;
        }

        private void readGovernmentInfo(ImportantPaths paths, string loc) {
            string govPath = paths.getSavegameXMLPath() + "\\" + loc;
            XmlDocument doc = new XmlDocument();
            try {
                doc.Load(govPath);
            }
            catch (Exception) { return; }
            XmlNode root = doc.DocumentElement;
            for (XmlNode node = root.FirstChild; node != null; node = node.NextSibling) {
                if (node.Attributes.Count == 0) {
                    continue;
                }
                XmlAttribute attr = node.Attributes[0];
                if (attr.Name == "type" && attr.InnerText == "CHARACTER_POSTS") {
                    for (XmlNode charPostContainer = node.FirstChild; charPostContainer != null; charPostContainer = charPostContainer.NextSibling) {
                        for (XmlNode charPost = charPostContainer.FirstChild; charPost != null; charPost = charPost.NextSibling) {
                            if (charPost.Name != "rec") continue;
                            int pos = 0;
                            string office = "";
                            int esfID = 0;
                            for (XmlNode charPostPos = charPost.FirstChild; charPostPos != null; charPostPos = charPostPos.NextSibling) {
                                if (pos == 1) {
                                    office = charPostPos.InnerText;
                                }
                                else if (pos == 2) {
                                    esfID = Int32.Parse(charPostPos.InnerText);
                                }
                                pos++;
                            }
                            if (esfID != 0) {
                                esfID2Job.Add(esfID, office);
                            }
                        }
                    }
                }
            }
        }

        public List<Tuple<String, CK2Character>> getCharInfo() { return charInfo; }
        public string getJob(int esfID) {
            try {
                return esfID2Job[esfID];
            }
            catch (Exception) {
                return null;
            }
        }

    }
}
