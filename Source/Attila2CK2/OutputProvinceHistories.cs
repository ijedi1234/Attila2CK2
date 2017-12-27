using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Attila2CK2 {
    class OutputProvinceHistories {

        public static void output(AttilaRegionsInfo attilaRegions, CK2RegionsInfo ck2Regions, ReligionsInfo religions) {
            int count = 0;
            List<AttilaRegionInfo> attilaList = attilaRegions.getList();
            Dictionary<string, CK2CountyRegionInfo> ck2CountiesMap = ck2Regions.getCountiesMap();
            foreach(AttilaRegionInfo attilaRegion in attilaList) {
                List<String> attilasCK2regions = attilaRegion.getCK2Regions();
                foreach (String ck2RegionStr in attilasCK2regions) {
                    CK2CountyRegionInfo ck2County = ck2CountiesMap[ck2RegionStr];
                    writeProvinceHistory(attilaRegion, ck2County, religions);
                    count++;
                }
            }
            count += 0;
        }

        private static void writeProvinceHistory(AttilaRegionInfo attilaRegion, CK2CountyRegionInfo ck2County, ReligionsInfo religions) {
            //Remove ifs if going province specific
            if (attilaRegion.getIsBurned()) return;
            FactionInfo faction = attilaRegion.getOwningFaction();
            if (faction.getID().Contains("fact_separatist") || faction.getID().Contains("fact_rebel")) {
                return;
            }
            string filename = ck2County.getFilename();
            string outputPath = ImportantPaths.getOutputPath() + "\\history\\provinces\\" + filename;
            HashSet<String> baronies = ck2County.getBaronies();
            using (StreamWriter writer = File.CreateText(outputPath)) {
                writer.WriteLine("# " + filename.Substring(0, filename.Length - 4));
                writer.WriteLine("");
                writer.WriteLine("# County Title");
                writer.WriteLine("title = " + ck2County.getID());
                writer.WriteLine("");
                writer.WriteLine("# Settlements");
                writer.WriteLine("max_settlements = " + ck2County.getMaxSettlements());
                bool wroteBarony = false;
                foreach (String barony in baronies) {
                    if (wroteBarony == false) {
                        writer.WriteLine(barony + " = castle");
                        wroteBarony = true;
                    }
                    else {
                        writer.WriteLine("#" + barony + " = castle");
                    }
                }
                writer.WriteLine("");
                writer.WriteLine("# Misc");
                string culture = faction.getOwner().getCulture();
                writer.WriteLine("culture = " + culture);
                //writer.WriteLine("culture = " + attilaRegion.getIDStr());
                writer.WriteLine("religion = " + religions.getCK2Religion(attilaRegion.getMostPowerfulReligion()));
                writer.WriteLine("");
                writer.WriteLine("# History");
            }
        }

    }
}
