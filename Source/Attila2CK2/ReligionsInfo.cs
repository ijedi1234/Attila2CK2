using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Attila2CK2 {
    class ReligionsInfo {

        //Screen name, Attila name, CK2 name
        private Dictionary<string, ReligionInfo> religions;

        public ReligionsInfo() {
            religions = new Dictionary<string, ReligionInfo>();
            string converterInfo = ImportantPaths.conversionInfoPath();
            string religionFile = converterInfo + "\\religion\\religions.csv";
            using (var religionsReader = new StreamReader(@religionFile)) {
                while (!religionsReader.EndOfStream) {
                    ReligionInfo religion = new ReligionInfo(religionsReader.ReadLine());
                    religions.Add(religion.getAttilaName(), religion);
                }
            }
        }

        public string getCK2Religion(string attilaReligion) {
            return religions[attilaReligion].getCK2Name();
        }

    }
}
