using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Attila2CK2 {
    class MoveCultures {

        public static void move() {
            string searchDir = ImportantPaths.conversionInfoPath() + "\\cultures\\cultureFiles";
            string destDir = ImportantPaths.getOutputPath() + "\\common\\cultures";
            string[] searchFiles = Directory.GetFiles(searchDir, "*.txt");
            foreach (string searchFile in searchFiles) {
                string absoluteFilename = Path.GetFileName(searchFile);
                string destFile = destDir + "\\" + absoluteFilename;
                File.Copy(searchFile, destFile, true);
            }
        }

    }
}
