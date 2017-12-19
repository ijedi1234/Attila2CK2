using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Attila2CK2 {
    class RegionMapper {

        private Dictionary<string, string[]> regionsMap;
        private List<string> alteredRegions;

        public RegionMapper() {
            regionsMap = new Dictionary<string, string[]>();
            alteredRegions = new List<string>();
            string converterInfo = ImportantPaths.conversionInfoPath();
            string regionMapFile = converterInfo + "\\region\\regionsMap.csv";
            using (var regionsMapReader = new StreamReader(regionMapFile)) {
                while (!regionsMapReader.EndOfStream) {
                    string[] kvPair = regionsMapReader.ReadLine().Split(',');
                    string key = kvPair[0];
                    string[] values = kvPair[1].Split(';');
                    regionsMap.Add(key, values);
                    alteredRegions.AddRange(values);
                }
            }
            alteredRegions.Sort();
            /*for (int i = 0; i < alteredRegions.Count - 1; i++) {
                if (alteredRegions[i] == alteredRegions[i + 1]) {
                    bool trouble = true;
                }
            }*/
        }

        public string[] getCK2Regions(string key) {
            try {
                return regionsMap[key];
            }
            catch (Exception) {
                return null;
            }
        }

        public bool shallBeAltered(string ck2region) {
            int regionIndex = alteredRegions.BinarySearch(ck2region);
            return regionIndex >= 0;
        }

    }
}
