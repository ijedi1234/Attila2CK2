using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Attila2CK2 {
    class CK2CountyRegionsInfo {

        private Dictionary<string, CK2CountyRegionInfo> counties;

        public CK2CountyRegionsInfo(RegionMapper map) {
            counties = new Dictionary<string, CK2CountyRegionInfo>();
            string provinceHistoryPath = ImportantPaths.conversionInfoPath() + "\\region\\provinceHistory";
            string[] histories = Directory.GetFiles(provinceHistoryPath);
            foreach (string historyPath in histories) {
                CK2CountyRegionInfo county = new CK2CountyRegionInfo(map, historyPath);
                if(county.countyShallBeAltered())
                    counties.Add(county.getID(), county);
            }
        }

        public Dictionary<string, CK2CountyRegionInfo> getCountiesMap() { return counties; }

    }
}
