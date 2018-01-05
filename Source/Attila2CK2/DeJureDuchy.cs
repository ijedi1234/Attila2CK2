using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attila2CK2 {
    class DeJureDuchy {

        private string name;
        private string screenName;
        private AttilaRegionInfo region;

        public DeJureDuchy(string name, AttilaRegionInfo region) {
            this.name = name;
            this.region = region;
            this.screenName = DeJureTitles.deriveScreenName(name);
        }

        public string getName() { return name; }
        public string getScreenName() { return screenName; }
        public AttilaRegionInfo getRegion() { return region; }

    }
}
