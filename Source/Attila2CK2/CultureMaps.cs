using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Attila2CK2 {
    class CultureMaps {

        private Dictionary<String, List<String>> cultureGroupMaps;
        private Dictionary<String, List<String>> cultureMaps;

        public CultureMaps() {
            readCultureGroupMaps();
            readCultureMaps();
        }

        public void readCultureGroupMaps() {
            cultureGroupMaps = new Dictionary<string, List<string>>();
            string converterInfo = ImportantPaths.conversionInfoPath();
            string CGMFile = converterInfo + "\\cultures\\cultureGroupNameMap.csv";
            using (var CGMReader = new StreamReader(CGMFile)) {
                while (!CGMReader.EndOfStream) {
                    string[] items = CGMReader.ReadLine().Split(',');
                    string cultureGroupTag = items[0];
                    List<String> localisation = new List<string>();
                    for (int i = 1; i < items.Length; i++) {
                        localisation.Add(items[i]);
                    }
                    cultureGroupMaps.Add(cultureGroupTag, localisation);
                }
            }
        }

        public void readCultureMaps() {
            cultureMaps = new Dictionary<string, List<string>>();
            string converterInfo = ImportantPaths.conversionInfoPath();
            string CMFile = converterInfo + "\\cultures\\cultureNameMap.csv";
            using (var CGReader = new StreamReader(CMFile)) {
                while (!CGReader.EndOfStream) {
                    string[] items = CGReader.ReadLine().Split(',');
                    string cultureTag = items[0];
                    List<String> localisation = new List<string>();
                    for (int i = 1; i < items.Length; i++) {
                        localisation.Add(items[i]);
                    }
                    cultureGroupMaps.Add(cultureTag, localisation);
                }
            }
        }

        public Dictionary<String, List<String>> getCultureGroupMaps() { return cultureGroupMaps; }
        public Dictionary<String, List<String>> getCultureMaps() { return cultureMaps; }

    }
}
