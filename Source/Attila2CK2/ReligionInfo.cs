using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attila2CK2 {
    class ReligionInfo {

        private string screenName;
        private string attilaName;
        private string ck2Name;

        public ReligionInfo(string line) {
            var lineValues = line.Split(',');
            screenName = lineValues[0];
            attilaName = lineValues[1];
            ck2Name = lineValues[2];
        }

        public string getAttilaName() { return attilaName; }
        public string getCK2Name() { return ck2Name; }

    }
}
