using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace Attila2CK2 {
    class AttilaRegionInfo {

        private ImportantPaths importantPaths;
        private int idNum;
        private string idStr;
        private bool burned;
        private List<Tuple<string, double>> religionBreakdown;
        private string strongestReligion;
        private List<String> ck2Regions;
        private FactionInfo owningFaction;

        public AttilaRegionInfo(ImportantPaths importantPaths, string regionXML, RegionMapper map, FactionsInfo factions) {
            this.importantPaths = importantPaths;
            idNum = 0; idStr = ""; burned = true;
            religionBreakdown = new List<Tuple<string, double>>();
            ck2Regions = new List<String>();
            readRegionXML(regionXML, factions);
            readPopulation();
            strongestReligion = this.deriveMostPowerfulReligion();
            string[] foundCK2regions = map.getCK2Regions(idStr);
            if (foundCK2regions != null) {
                foreach (string region in foundCK2regions) {
                    ck2Regions.Add(region);
                }
            }
        }

        private void readRegionXML(string regionXML, FactionsInfo factions) {
            XmlDocument doc = new XmlDocument();
            doc.Load(regionXML);
            XmlNode root = doc.DocumentElement;
            int nodePos = 0;
            for (XmlNode node = root.FirstChild; node != null; node = node.NextSibling) {
                if (node.Name == "rec" || node.Name == "xml_include" || node.Name == "traits") continue;
                if (nodePos == 0) idNum = Int32.Parse(node.InnerText);
                else if (nodePos == 1) idStr = node.InnerText;
                else if (nodePos == 10) {
                    int ownerNumID = Int32.Parse(node.InnerText);
                    FactionInfo faction = factions.getFactionByNumID(ownerNumID);
                    if (faction != null) this.owningFaction = faction;
                }
                else if (nodePos == 23) {
                    if (node.Name == "yes") burned = true;
                    else burned = false;
                }
                nodePos++;
            }
            //string regionSlotPath = importantPaths.getSavegameXMLPath() + "\\region_slot\\" + idStr + "-0.xml";
            //readRegionSlot0(regionSlotPath);
        }
        /*
        private void readRegionSlot0(string regionSlotPath) {
            XmlDocument doc = new XmlDocument();
            doc.Load(regionSlotPath);
            XmlNode root = doc.DocumentElement;
            XmlNode innerRoot = root.FirstChild;
            for (XmlNode node = innerRoot.FirstChild; node != null; node = node.NextSibling) {
                if (node.Attributes.Count == 0) {
                    continue;
                }
                XmlAttribute attr = node.Attributes[0];
                if (attr.Name == "type" && attr.InnerText == "REGION_BUILDING_MANAGER") {
                    for (XmlNode node2 = node.FirstChild; node2 != null; node2 = node2.NextSibling) {
                        if (node2.Attributes.Count == 0) {
                            continue;
                        }
                        XmlAttribute attr2 = node2.Attributes[0];
                        if (attr2.Name == "type" && attr2.InnerText == "BUILDING_BASE") {
                            for (XmlNode node3 = node2.FirstChild; node3 != null; node3 = node3.NextSibling) {
                                if (node3.InnerText.StartsWith("att_fact")) {
                                    this.owningFaction = node3.InnerText;
                                }
                            }
                        }
                    }
                }
            }
        }
        */
        private void readPopulation() {
            string populationPath = importantPaths.getSavegameXMLPath() + "\\population\\" + idStr + ".xml";
            XmlDocument doc = new XmlDocument();
            doc.Load(populationPath);
            XmlNode religiousBreakdownNode = getToReligiousBreakdownNode(doc.DocumentElement);
            for (XmlNode nodeWrapper = religiousBreakdownNode.FirstChild; nodeWrapper != null; nodeWrapper = nodeWrapper.NextSibling) {
                string religion = nodeWrapper.FirstChild.InnerText;
                double power = Double.Parse(nodeWrapper.FirstChild.NextSibling.InnerText);
                religionBreakdown.Add(Tuple.Create<string, double>(religion, power));
            }
        }

        private XmlNode getToReligiousBreakdownNode(XmlNode root) {
            XmlNode region_factors = root.FirstChild;
            for (XmlNode node = region_factors.FirstChild; node != null; node = node.NextSibling) {
                if (node.Name == "ary" && node.Attributes.Count > 0) {
                    XmlAttribute attr = node.Attributes[0];
                    if(attr.Name == "type" && attr.InnerText == "RELIGION_BREAKDOWN")
                        return node;
                }
            }
            return null;
        }

        private string deriveMostPowerfulReligion() {
            Tuple<string, double> mostPowerful = Tuple.Create<string, double>("att_rel_chr_catholic", 0.0);
            foreach (var breakdown in religionBreakdown) {
                if (breakdown.Item2 > mostPowerful.Item2) {
                    mostPowerful = breakdown;
                }
            }
            return mostPowerful.Item1;
        }

        public int getIDNum() { return idNum; }
        public string getIDStr() { return idStr; }
        public bool getIsBurned() { return burned; }
        public string getMostPowerfulReligion() { return strongestReligion; }
        public List<String> getCK2Regions() { return ck2Regions; }
        public FactionInfo getOwningFaction() { return owningFaction; }

    }
}
