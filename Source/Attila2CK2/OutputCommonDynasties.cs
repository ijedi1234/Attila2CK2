using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Attila2CK2 {
    class OutputCommonDynasties {

        public static void output(FamilyTrees trees) {
            List<CK2Dynasty> dynasties = trees.getDynasties();
            string filename = ImportantPaths.getOutputPath() + "\\common\\dynasties\\attila_dynasties.txt";
            using (StreamWriter writer = File.CreateText(filename)) {
                foreach (CK2Dynasty dynasty in dynasties) {
                    int id = dynasty.getID();
                    string name = dynasty.getName();
                    writeDynasty(writer, id, name);
                }
            }
        }

        private static void writeDynasty(StreamWriter writer, int id, string name) {
            writer.WriteLine(id + "=");
            writer.WriteLine("{");
            writer.WriteLine("\tname=\"" + name + "\"");
            writer.WriteLine("\tculture=\"english\"");
            writer.WriteLine("}");
        }

    }
}
