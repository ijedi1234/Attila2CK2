using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Attila2CK2 {
    class OutputTitleHistories {

        public static void outputCountyHistory(FactionsInfo factions) {
            string outputDir = ImportantPaths.getOutputPath() + "\\history\\titles";
            string countyAcquireDate = "767.1.1";
            foreach (FactionInfo faction in factions.getFactions()) {
                if (!faction.getExists() || faction.getCK2Title() == null) continue;
                HashSet<String> counties = faction.getOwnedCounties();
                foreach (String countyStr in counties) {
                    using (StreamWriter writer = File.CreateText(outputDir + "\\" + countyStr + ".txt")) {
                        writer.WriteLine(countyAcquireDate + "={");
                        writer.WriteLine("\tholder=" + faction.getOwner().getID());
                        //if (faction.getOwner().getName() == "Giesmus") {
                        //    int h = 0;
                        //}
                        writer.WriteLine("}");
                    }
                }
            }
        }

        public static void outputFactionTitleHistory(FactionsInfo factions) {
            string outputDir = ImportantPaths.getOutputPath() + "\\history\\titles";
            string titleAcquireDate = "767.1.1";
            foreach (FactionInfo faction in factions.getFactions()) {
                if (!faction.getExists() || faction.getCK2Title() == null) continue;
                string ck2Title = faction.getCK2Title();
                int ownerCharID = faction.getOwner().getID();
                using (StreamWriter writer = File.CreateText(outputDir + "\\" + ck2Title + ".txt")) {
                    writer.WriteLine(titleAcquireDate + "={");
                    writer.WriteLine("\tholder=" + ownerCharID);
                    if (faction.getCK2Title().StartsWith("e_")) {
                        writer.WriteLine("\tlaw=imperial_administration");
                        writer.WriteLine("\tlaw=succ_primogeniture");
                    }
                    if (!faction.getIsNewTitle()) {
                        string idName = ("correction_" + faction.getCK2Title()).ToUpper();
                        string idAdj = ("correction_" + faction.getCK2Title() + "_adj").ToUpper();
                        writer.WriteLine("name=" + idName);
                        writer.WriteLine("adjective=" + idAdj);
                    }
                    writer.WriteLine("}");
                }
            }
        }

        public static void outputv1History(CK2RegionsInfo ck2Regions) {
            Dictionary<string, CK2CountyRegionInfo> counties = ck2Regions.getCountiesMap();
            string outputDir = ImportantPaths.getOutputPath() + "\\history\\titles";
            foreach (var countyPair in counties) {
                string countyStr = countyPair.Key;
                int luckyGuy = 157600001;
                using (StreamWriter writer = File.CreateText(outputDir + "\\" + countyStr + ".txt")) {
                    writer.WriteLine("767.1.1={");
                    writer.WriteLine("\tholder=" + luckyGuy);
                    writer.WriteLine("}");
                }
            }
        }

    }
}
