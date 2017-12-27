using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Attila2CK2 {
    class AttilaRegionsInfo {

        private ImportantPaths importantPaths;
        private List<AttilaRegionInfo> attilaRegionMappings;

        public AttilaRegionsInfo(ImportantPaths importantPaths, RegionMapper map, FactionsInfo factions) {
            this.importantPaths = importantPaths;
            getRegionXMLInfo(map, factions);
        }

        private void getRegionXMLInfo(RegionMapper map, FactionsInfo factions) {
            attilaRegionMappings = new List<AttilaRegionInfo>();
            string regionPath = importantPaths.getSavegameXMLPath() + "\\region";
            string[] regionXMLs = Directory.GetFiles(regionPath);
            foreach(string regionXML in regionXMLs) {
                AttilaRegionInfo regionInfo = new AttilaRegionInfo(importantPaths, regionXML, map, factions);
                attilaRegionMappings.Add(regionInfo);
            }
        }

        public bool checkIfFactionExists(string factionID) {
            foreach(AttilaRegionInfo region in attilaRegionMappings) {
                if (!region.getIsBurned() && region.getOwningFaction().getID() == factionID) {
                    return true;
                }
            }
            return false;
        }

        public List<AttilaRegionInfo> getList() { return attilaRegionMappings; }

    }
}
