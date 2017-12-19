using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Attila2CK2 {
    class MoveFlags {

        public static void move(FactionsInfo factions) {
            string searchDir = ImportantPaths.conversionInfoPath() + "\\factions\\flags";
            string destDir = ImportantPaths.getOutputPath() + "\\gfx\\flags";
            foreach (FactionInfo faction in factions.getFactions()) {
                string ck2Title = faction.getCK2Title();
                if (ck2Title == null) continue;
                string target = ck2Title.Substring(2) + ".tga";
                target = searchDir + "\\" + target;
                bool flagExists = File.Exists(target);
                if (!flagExists) continue;
                string dest = destDir + "\\" + ck2Title + ".tga";
                File.Copy(target, dest, true);
            }
        }

    }
}
