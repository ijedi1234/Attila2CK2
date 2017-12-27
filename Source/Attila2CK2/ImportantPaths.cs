using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attila2CK2 {
    class ImportantPaths {

        private string savegameXMLPath;

        public ImportantPaths(string savegameXMLPath) {
            this.savegameXMLPath = savegameXMLPath;
        }

        //public static string conversionInfoPath() { return "E:\\Attila2CK2\\conversionInfo"; }
        public static string conversionInfoPath() { return "./conversionInfo"; }
        public string getSavegameXMLPath() { return savegameXMLPath; }
        //public static string getOutputPath() { return "E:\\Attila2CK2\\output"; }
        public static string getOutputPath() { return "./output"; }

    }
}
