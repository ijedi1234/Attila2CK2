using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Attila2CK2 {
    class CK2CountyRegionInfo {

        private bool shallBeAltered;
        private string provinceHistoryFilename;
        private string regionID;
        private int maxSettlements;
        private HashSet<String> baronies;
        private string terrain;

        public CK2CountyRegionInfo(RegionMapper map, string provinceHistoryPath) {
            shallBeAltered = false; terrain = null;
            provinceHistoryFilename = System.IO.Path.GetFileName(provinceHistoryPath);
            baronies = new HashSet<String>();
            using (var provinceHistoryReader = new StreamReader(provinceHistoryPath)) {
                while (!provinceHistoryReader.EndOfStream) {
                    string line = provinceHistoryReader.ReadLine().Trim();
                    if (line.StartsWith("title")) {
                        regionID = line.Split('=')[1].Trim();
                        if (map.shallBeAltered(regionID)) {
                            shallBeAltered = true;
                        }
                        else {
                            shallBeAltered = false;
                        }
                    }
                    else if (line.StartsWith("max_settlements")) {
                        maxSettlements = Int32.Parse(line.Split('=')[1].Trim());
                    }
                    else if (line.StartsWith("b_")) {
                        int eqSignLoc = line.IndexOf('=');
                        string barony;
                        if (eqSignLoc < 0)
                            barony = line.Substring(0).Trim();
                        else
                            barony = line.Substring(0, eqSignLoc).Trim();
                        baronies.Add(barony);
                    }
                    else if (line.StartsWith("#b_")) {
                        int eqSignLoc = line.IndexOf('=');
                        string barony;
                        if(eqSignLoc < 0)
                            barony = line.Substring(1).Trim();
                        else
                            barony = line.Substring(1, eqSignLoc - 1).Trim();
                        baronies.Add(barony);
                    }
                    else if (line.StartsWith("terrain")) {
                        terrain = line.Split('=')[1].Trim();
                    }
                }
            }
        }

        public bool countyShallBeAltered() { return shallBeAltered; }
        public string getID() { return regionID; }
        public string getFilename() { return provinceHistoryFilename; }
        public int getMaxSettlements() { return maxSettlements; }
        public HashSet<String> getBaronies() { return baronies; }

    }
}
