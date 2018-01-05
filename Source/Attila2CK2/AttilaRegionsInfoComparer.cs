using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attila2CK2 {
    class AttilaRegionsInfoComparer : IComparer<AttilaRegionInfo> {

        public int Compare(AttilaRegionInfo r1, AttilaRegionInfo r2) {
            string name1 = r1.getIDStr(); string name2 = r2.getIDStr();
            return name1.CompareTo(name2);
        }

    }
}
