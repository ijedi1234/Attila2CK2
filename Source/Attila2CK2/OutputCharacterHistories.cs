using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Attila2CK2 {
    class OutputCharacterHistories {

        public static void output(FactionsInfo factions) {
            List<CK2Character> owners = factions.getOwners();
            string filename = ImportantPaths.getOutputPath() + "\\history\\characters\\english.txt";
            using (StreamWriter writer = File.CreateText(filename)) {
                foreach (CK2Character owner in owners) {
                    CK2Dynasty dynasty = owner.getDynasty();
                    writeCharacter(writer, dynasty, owner);
                }
            }
        }

        private static void writeCharacter(StreamWriter writer, CK2Dynasty dynasty, CK2Character character) {
            int dynastyID = dynasty.getID();
            string name = character.getName();
            int charID = character.getID();
            int dob = character.getBirthDay();
            int mob = character.getBirthMonth();
            int yob = character.getBirthYear();
            writer.WriteLine(charID + " = {");
            writer.WriteLine("\tname=\"" + name + "\"");
            writer.WriteLine("\tdynasty=" + dynastyID);
            writer.WriteLine("\treligion=\"catholic\"");
            writer.WriteLine("\tculture=\"english\"");
            writer.WriteLine("\t" + yob + "." + mob + "." + dob + "={");
            writer.WriteLine("\t\tbirth=yes");
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }

    }
}
