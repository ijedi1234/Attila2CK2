using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Attila2CK2 {
    class OutputTitleLocalisation {

        public static void output(FactionsInfo factionsInfo) {
            string outputFile = ImportantPaths.getOutputPath() + "\\localisation\\newTitles.csv";
            using (StreamWriter writer = File.CreateText(outputFile)) {
                writer.WriteLine(LocalisationFormatter.header());
                foreach (FactionInfo faction in factionsInfo.getFactions()) {
                    outputNames(writer, faction);
                    outputAdjectives(writer, faction);
                }
            }
        }

        private static void outputNames(StreamWriter writer, FactionInfo faction) {
            if (faction.getCK2Title() == null) return;
            //if (faction.getCK2Title() == "e_persia") return;
            if (!faction.getIsNewTitle()) return;
            string id = faction.getCK2Title();
            string english = faction.getName();
            string line = LocalisationFormatter.format(id, english);
            writer.WriteLine(line);
        }

        private static void outputAdjectives(StreamWriter writer, FactionInfo faction) {
            if (faction.getCK2Title() == null) return;
            //if (faction.getCK2Title() == "e_persia") return;
            if (!faction.getIsNewTitle()) return;
            string id = faction.getCK2Title() + "_adj";
            string english = faction.getAdjective();
            string line = LocalisationFormatter.format(id, english);
            writer.WriteLine(line);
        }

    }
}
