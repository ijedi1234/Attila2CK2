using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Attila2CK2 {
    class OutputCommonCultures {
        
        public static void outputProvinceSpecific(AttilaRegionsInfo attilaRegions) {
            string outputPath = ImportantPaths.getOutputPath() + "\\common\\cultures\\specialProvCultures.txt";
            List<AttilaRegionInfo> regions = attilaRegions.getList();
            using (StreamWriter writer = File.CreateText(outputPath)) {
                writer.WriteLine("iberian = {");
                writer.WriteLine("\tgraphical_cultures = { occitangfx }");
                writer.WriteLine("\t");
                Random rand = new Random();
                foreach (AttilaRegionInfo region in regions) {
                    writer.WriteLine("\t" + region.getIDStr() + " = {");
                    writer.WriteLine("\t\tgraphical_cultures = { southerngfx }");
                    writer.WriteLine("\t\tsecondary_event_pictures = bedouin_arabic");
                    writer.WriteLine("\t\t");
                    double r = (rand.Next(0, 100))/100.0;
                    double g = (rand.Next(0, 100)) / 100.0;
                    double b = (rand.Next(0, 100)) / 100.0;
                    writer.WriteLine("\t\tcolor = { " + r + " " + g + " " + b + " }");
                    writer.WriteLine("\t\t");
                    writer.WriteLine("\t\tmale_names = { Placeholder }");
                    writer.WriteLine("\t\tfemale_names = { Placeholder }");
                    writer.WriteLine("\t\t");
                    writer.WriteLine("\t\tmodifier = default_culture_modifier");
                    writer.WriteLine("\t}");
                }
                writer.WriteLine("}");
            }
        }
        
    }
}
