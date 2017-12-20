using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attila2CK2 {
    class CK2Dynasty {

        private static int newIDnum = 137600001;

        private int id;
        private string name;

        public CK2Dynasty() {
            this.name = "Holder";
            this.id = CK2Dynasty.newIDnum;
            CK2Dynasty.newIDnum++;
        }

        public CK2Dynasty(string name) {
            this.name = name;
            this.id = CK2Dynasty.newIDnum;
            CK2Dynasty.newIDnum++;
        }

        public int getID() { return id; }
        public string getName() { return name; }

    }
}
