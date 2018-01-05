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
                    if (faction.getCK2Title() == null) continue;
                    outputNames(writer, faction);
                    outputAdjectives(writer, faction);
                }
            }
        }

        private static void outputNames(StreamWriter writer, FactionInfo faction) {
            //if (faction.getCK2Title() == "e_persia") return;
            if (faction.getIsNewTitle()) {
                string id = faction.getCK2Title();
                string english = faction.getName();
                string line = LocalisationFormatter.format(id, english);
                writer.WriteLine(line);
            }
            else {
                string id = "correction_" + faction.getCK2Title();
                id = id.ToUpper();
                string english = faction.getName();
                string line = LocalisationFormatter.format(id, english);
                writer.WriteLine(line);
            }
        }

        private static void outputAdjectives(StreamWriter writer, FactionInfo faction) {
            //if (faction.getCK2Title() == "e_persia") return;
            if (faction.getIsNewTitle()) {
                string id = faction.getCK2Title() + "_adj";
                string english = faction.getAdjective();
                string line = LocalisationFormatter.format(id, english);
                writer.WriteLine(line);
            }
            else {
                string id = "correction_" + faction.getCK2Title() + "_adj";
                id = id.ToUpper();
                string english = faction.getAdjective();
                string line = LocalisationFormatter.format(id, english);
                writer.WriteLine(line);
            }
        }

        public static void outputDeJure(DeJureTitles titles) {
            string outputFile = ImportantPaths.getOutputPath() + "\\localisation\\newDeJureTitles.csv";
            List<DeJureKingdom> kingdoms = titles.getTitles();
            using (StreamWriter writer = File.CreateText(outputFile)) {
                foreach (DeJureKingdom kingdom in kingdoms) {
                    outputDeJureInfo(writer, true, kingdom.getName(), kingdom.getScreenName(), kingdom.getScreenName());
                    List<DeJureDuchy> duchies = kingdom.getDuchies();
                    foreach (DeJureDuchy duchy in duchies) {
                        outputDeJureInfo(writer, false, duchy.getName(), duchy.getScreenName(), duchy.getScreenName());
                    }
                }
            }
        }

        private static void outputDeJureInfo(StreamWriter writer, bool isKingdom, string title, string name, string adjective) {
            string prefix = "d";
            if (isKingdom) prefix = "k";
            string idName = prefix + "_attila_" + title;
            string idAdj = prefix + "_attila_" + title + "_adj";
            string lineName = LocalisationFormatter.format(idName, name);
            writer.WriteLine(lineName);
            string lineAdjective = LocalisationFormatter.format(idAdj, adjective);
            writer.WriteLine(lineAdjective);
        }

    }
}
