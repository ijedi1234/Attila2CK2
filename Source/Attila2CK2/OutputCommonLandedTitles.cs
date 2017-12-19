using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace Attila2CK2 {
    class OutputCommonLandedTitles {

        public static void output(FactionsInfo factions) {
            string outputPath = ImportantPaths.getOutputPath() + "\\common\\landed_titles\\newTitles.txt";
            HashSet<string> dupChecker = new HashSet<string>();
            using (StreamWriter writer = File.CreateText(outputPath)) {
                foreach(FactionInfo faction in factions.getFactions()) {
                    if (!faction.getExists()) continue;
                    writeTitleInfo(writer, faction, dupChecker);
                }
                foreach (FactionInfo faction in factions.getFactions()) {
                    if (faction.getExists()) continue;
                    writeTitleInfo(writer, faction, dupChecker);
                }
            }
        }

        private static void writeTitleInfo(StreamWriter writer, FactionInfo faction, HashSet<string> dupChecker) {
            string ck2Title = faction.getCK2Title();

            int size1 = dupChecker.Count;
            dupChecker.Add(ck2Title);
            int size2 = dupChecker.Count;
            if (size1 == size2) return;

            if (ck2Title != null && faction.getIsNewTitle()) {
                Color color = faction.getColor();
                writer.WriteLine(ck2Title + " = {");
                writer.WriteLine("\tcolor={ " + color.R + " " + color.G + " " + color.B + " }");
                if (faction.getCK2Title() == "e_wre")
                    writer.WriteLine("\tshort_name=yes");
                writer.WriteLine("}");
            }
        }

    }
}
