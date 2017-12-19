using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Attila2CK2 {
    class DirectoryHierarchyCreator {

        public static void createOutputDirectories() {
            string outputDir = ImportantPaths.getOutputPath();
            string historyDirectory = outputDir + "\\history";
            string historyProvincesDirectory = historyDirectory + "\\provinces";
            string historyTitlesDirectory = historyDirectory + "\\titles";
            string historyCharactersDirectory = historyDirectory + "\\characters";
            if (!Directory.Exists(historyDirectory))
                Directory.CreateDirectory(historyDirectory);
            if (!Directory.Exists(historyProvincesDirectory))
                Directory.CreateDirectory(historyProvincesDirectory);
            if (!Directory.Exists(historyTitlesDirectory))
                Directory.CreateDirectory(historyTitlesDirectory);
            if (!Directory.Exists(historyCharactersDirectory))
                Directory.CreateDirectory(historyCharactersDirectory);
            string commonDirectory = outputDir + "\\common";
            string commonCulturesDirectory = commonDirectory + "\\cultures";
            string commonDynastiesDirectory = commonDirectory + "\\dynasties";
            string commonLandedTitlesDirectory = commonDirectory + "\\landed_titles";
            if (!Directory.Exists(commonDirectory))
                Directory.CreateDirectory(commonDirectory);
            if (!Directory.Exists(commonCulturesDirectory))
                Directory.CreateDirectory(commonCulturesDirectory);
            if (!Directory.Exists(commonDynastiesDirectory))
                Directory.CreateDirectory(commonDynastiesDirectory);
            if (!Directory.Exists(commonLandedTitlesDirectory))
                Directory.CreateDirectory(commonLandedTitlesDirectory);
            string localisationDirectory = outputDir + "\\localisation";
            if (!Directory.Exists(localisationDirectory))
                Directory.CreateDirectory(localisationDirectory);
            string gfxDirectory = outputDir + "\\gfx";
            string gfxFlagsDirectory = gfxDirectory + "\\flags";
            if (!Directory.Exists(gfxDirectory))
                Directory.CreateDirectory(gfxDirectory);
            if (!Directory.Exists(gfxFlagsDirectory))
                Directory.CreateDirectory(gfxFlagsDirectory);
        }

    }
}
