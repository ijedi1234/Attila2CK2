using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Attila2CK2 {
    class OutputCultureLocalisation {

        public static void outputCultureGroups(CultureMaps maps) {
            string outputFile = ImportantPaths.getOutputPath() + "\\localisation\\newGroupCultures.csv";
            using (StreamWriter writer = File.CreateText(outputFile)) {
                writer.WriteLine(LocalisationFormatter.header());
                foreach (var pair in maps.getCultureGroupMaps()) {
                    string id = pair.Key;
                    string english = "";
                    if (pair.Value.Count > 0) english = pair.Value[0];
                    string line = LocalisationFormatter.format(id, english);
                    writer.WriteLine(line);
                }
            }
        }

        public static void outputCultures(CultureMaps maps) {
            string outputFile = ImportantPaths.getOutputPath() + "\\localisation\\newCultures.csv";
            using (StreamWriter writer = File.CreateText(outputFile)) {
                writer.WriteLine(LocalisationFormatter.header());
                foreach (var pair in maps.getCultureMaps()) {
                    string id = pair.Key;
                    string english = "";
                    if (pair.Value.Count > 0) english = pair.Value[0];
                    string line = LocalisationFormatter.format(id, english);
                    writer.WriteLine(line);
                }
            }
        }

    }
}
