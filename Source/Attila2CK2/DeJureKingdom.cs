using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attila2CK2 {
    class DeJureKingdom {

        private string name;
        private string screenName;
        private List<DeJureDuchy> duchies;

        public DeJureKingdom(string name) {
            this.name = name;
            duchies = new List<DeJureDuchy>();
            this.screenName = DeJureTitles.deriveScreenName(name);
        }

        public void addDuchy(DeJureDuchy duchy) {
            duchies.Add(duchy);
        }

        public string getName() { return name; }
        public string getScreenName() { return screenName; }
        public List<DeJureDuchy> getDuchies() { return duchies; }

    }
}
