using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Attila2CK2 {
    class FactionsInfo {

        private ImportantPaths importantPaths;

        //Key, Name, Adjective
        private List<FactionInfo> factionPreProcessMappings;

        public FactionsInfo() {
            List<Tuple<string, string>> factionNamesMapping = readFactionNames();
            List<Tuple<string, string>> factionAdjectivesMapping = readFactionAdjectives();
            if (factionNamesMapping.Count != factionAdjectivesMapping.Count) {
                Console.Error.WriteLine("Names and Adjectives length do not match.");
                Environment.Exit(1);
            }
            factionPreProcessMappings = new List<FactionInfo>();
            List<Tuple<string, string, bool>> ck2TitleMap = readFactionCK2Equivalent();
            for (int ind = 0; ind < factionNamesMapping.Count; ind++) {
                string faction = factionNamesMapping[ind].Item1;
                string factionName = factionNamesMapping[ind].Item2;
                string factionAdjective = factionAdjectivesMapping[ind].Item2;
                string ck2Equiv = null; bool isNew = false;
                foreach (Tuple<string, string, bool> info in ck2TitleMap) {
                    if (info.Item1 == faction) {
                        ck2Equiv = info.Item2;
                        if (ck2Equiv == "null") ck2Equiv = null;
                        isNew = info.Item3;
                    }
                }
                factionPreProcessMappings.Add(new FactionInfo(faction, factionName, factionAdjective, ck2Equiv, isNew));
            }
        }

        private List<Tuple<string, string>> readFactionNames() {
            string conversionInfoPath = ImportantPaths.conversionInfoPath();
            string factionNamesPath = conversionInfoPath + "\\factions\\faction_names.csv";
            List<Tuple<string, string>> factionNames = new List<Tuple<string, string>>();
            using (var factionNamesReader = new StreamReader(@factionNamesPath)) {
                while (!factionNamesReader.EndOfStream) {
                    var line = factionNamesReader.ReadLine();
                    var lineValues = line.Split(',');
                    string a = lineValues[0]; //att_fact_*
                    if (a.Substring(0, 3) != "att") continue;
                    string b = lineValues[1]; //Screen Name
                    Tuple<string, string> mapping = Tuple.Create<string, string>(a, b);
                    factionNames.Add(mapping);
                }
            }
            return factionNames;
        }

        private List<Tuple<string, string>> readFactionAdjectives() {
            string conversionInfoPath = ImportantPaths.conversionInfoPath();
            string factionAdjectivesPath = conversionInfoPath + "\\factions\\faction_adjectives.csv";
            List<Tuple<string, string>> factionAdjectives = new List<Tuple<string, string>>();
            using (var factionAdjectivesReader = new StreamReader(@factionAdjectivesPath)) {
                while (!factionAdjectivesReader.EndOfStream) {
                    var line = factionAdjectivesReader.ReadLine();
                    var lineValues = line.Split(',');
                    string a = lineValues[0]; //att_fact_*
                    if (a.Substring(0, 3) != "att") continue;
                    string b = lineValues[1]; //Adjective
                    Tuple<string, string> mapping = Tuple.Create<string, string>(a, b);
                    factionAdjectives.Add(mapping);
                }
            }
            return factionAdjectives;
        }

        private List<Tuple<string, string, bool>> readFactionCK2Equivalent() {
            string conversionInfoPath = ImportantPaths.conversionInfoPath();
            string factionEquivalentsPath = conversionInfoPath + "\\factions\\ck2Titles\\titlesFactionMap.csv";
            List<Tuple<string, string, bool>> factionEquivalents = new List<Tuple<string, string, bool>>();
            using (var factionEquivalentsReader = new StreamReader(@factionEquivalentsPath)) {
                while (!factionEquivalentsReader.EndOfStream) {
                    var line = factionEquivalentsReader.ReadLine();
                    var lineValues = line.Split(',');
                    string faction = lineValues[0];
                    string ck2Title = lineValues[1];
                    bool isNewTitle = (Int32.Parse(lineValues[2]) == 1);
                    Tuple<string, string, bool> mapping = Tuple.Create<string, string, bool>(faction, ck2Title, isNewTitle);
                    factionEquivalents.Add(mapping);
                }
            }
            return factionEquivalents;
        }

        public void updateImportantPaths(ImportantPaths paths) {
            importantPaths = paths;
        }

        public void updateFactionsInfo(AttilaRegionsInfo regions) {
            List<AttilaRegionInfo> regionsInfo = regions.getList();
            foreach (FactionInfo faction in factionPreProcessMappings) {
                faction.setExists(regions.checkIfFactionExists(faction.getID()));
                foreach (AttilaRegionInfo region in regionsInfo) {
                    if (region.getOwningFaction() == faction.getID() && !region.getIsBurned()) {
                        List<String> ck2Regions = region.getCK2Regions();
                        foreach (String ck2Region in ck2Regions) {
                            faction.addCounty(ck2Region);
                        }
                    }
                }
            }
        }

        public void readFamilyTrees(FamilyTrees trees) {
            foreach (FactionInfo faction in factionPreProcessMappings) {
                CK2Character character = new CK2Character();
                FamilyTree tree = trees.getTree(faction.getID());
                if(tree != null)
                    character = tree.getOwner();
                CK2Dynasty dynasty = new CK2Dynasty();
                if (faction.getDynasty() != null) {
                    dynasty = faction.getDynasty();
                    if (tree != null)
                        tree.updateDynasty(dynasty);
                }
                else {
                    if (tree != null)
                        dynasty = tree.getDynasty();
                    faction.setDynasty(dynasty);
                }
                //character.setDynasty(dynasty);
                faction.setOwner(character);
            }
        }

        public void readFactionXMLs() {
            foreach (FactionInfo faction in factionPreProcessMappings) {
                faction.readFactionNumID(importantPaths);
                faction.readFactionColor(importantPaths);
            }
        }

        public List<CK2Dynasty> getDynasties() {
            List<CK2Dynasty> dynasties = new List<CK2Dynasty>();
            foreach (FactionInfo faction in factionPreProcessMappings) {
                dynasties.Add(faction.getDynasty());
            }
            return dynasties;
        }

        public List<CK2Character> getOwners() {
            List<CK2Character> characters = new List<CK2Character>();
            foreach (FactionInfo faction in factionPreProcessMappings) {
                characters.Add(faction.getOwner());
            }
            return characters;
        }

        public List<FactionInfo> getFactions() {
            return factionPreProcessMappings;
        }

        public FactionInfo getFactionByNumID(int id) {
            foreach (FactionInfo faction in factionPreProcessMappings) {
                if (faction.getNumID() == id) return faction;
            }
            return null;
        }

    }
}
