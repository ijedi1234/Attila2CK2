using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Attila2CK2 {
    class OutputCharacterHistories {

        public static void output(FactionsInfo factionsObj) {
            List<CK2Character> owners = factionsObj.getOwners();
            List<FactionInfo> factions = factionsObj.getFactions();
            Dictionary<string, HashSet<CK2Character>> charactersToOutput = new Dictionary<string, HashSet<CK2Character>>();
            foreach (FactionInfo faction in factions) {
                if (!faction.getExists()) continue;
                CK2Character owner = faction.getOwner();
                HashSet<int> writtenCharacters = new HashSet<int>();
                selectCharacter(writtenCharacters, owner, charactersToOutput);
            }
            foreach (var pair in charactersToOutput) {
                string filename = ImportantPaths.getOutputPath() + "\\history\\characters\\" + pair.Key + ".txt";
                HashSet<CK2Character> characters = pair.Value;
                using (StreamWriter writer = File.CreateText(filename)) {
                    foreach (CK2Character character in characters) {
                        writeCharacter(writer, character);
                    }
                }
            }
        }

        private static void selectCharacter(HashSet<int> writtenCharacters, CK2Character character, Dictionary<string, HashSet<CK2Character>> charactersToOutput) {
            if (character == null) return;
            bool bWritten = !(writtenCharacters.Add(character.getID()));
            if (bWritten)
                return;
            appendCharToCTO(character, charactersToOutput);
            CK2Character father = character.getFather();
            selectCharacter(writtenCharacters, father, charactersToOutput);
            List<CK2Character> children = character.getChildren();
            if (children != null)
                foreach (CK2Character child in children) {
                    selectCharacter(writtenCharacters, child, charactersToOutput);
                }
            CK2Character spouse = character.getSpouse();
            selectCharacter(writtenCharacters, spouse, charactersToOutput);
        }

        private static void appendCharToCTO(CK2Character character, Dictionary<string, HashSet<CK2Character>> charactersToOutput) {
            try {
                HashSet<CK2Character> charList = charactersToOutput[character.getCulture()];
                charList.Add(character);
            }
            catch (Exception) {
                HashSet<CK2Character> charList = new HashSet<CK2Character>();
                charList.Add(character);
                charactersToOutput.Add(character.getCulture(), charList);
            }
        }

        private static void selectCharacter(HashSet<int> writtenCharacters, StreamWriter writer, CK2Character character) {
            if (character == null) return;
            bool bWritten = !(writtenCharacters.Add(character.getID()));
            if (bWritten) 
                return;
            writeCharacter(writer, character);
            CK2Character father = character.getFather();
            selectCharacter(writtenCharacters, writer, father);
            List<CK2Character> children = character.getChildren();
            if(children != null)
                foreach (CK2Character child in children) {
                    selectCharacter(writtenCharacters, writer, child);
                }
            CK2Character spouse = character.getSpouse();
            selectCharacter(writtenCharacters, writer, spouse);
        }

        private static void writeCharacter(StreamWriter writer, CK2Character character) {
            CK2Dynasty dynasty = character.getDynasty();
            int dynastyID = 0;
            if(dynasty != null)
                dynastyID = dynasty.getID();
            string name = character.getName();
            int charID = character.getID();
            int dob = character.getBirthDay();
            int mob = character.getBirthMonth();
            int yob = character.getBirthYear();
            writer.WriteLine(charID + " = {");
            writer.WriteLine("\tname=\"" + name + "\"");
            if(dynastyID != 0)
                writer.WriteLine("\tdynasty=" + dynastyID);
            else
                writer.WriteLine("\tdynasty=" + "NONE");
            writer.WriteLine("\treligion=\"" + character.getReligion() +"\"");
            writer.WriteLine("\tculture=\"" + character.getCulture() + "\"");

            if (character.getIsMale() == false) {
                writer.WriteLine("\tfemale=yes");
            }

            if (character.getIsBastard()) {
                writer.WriteLine("\tadd_trait=\"bastard\"");
            }

            if (character.getFather() != null) {
                writer.WriteLine("\tfather=" + character.getFather().getID());
            }

            writer.WriteLine("\t" + yob + "." + mob + "." + dob + "={");
            writer.WriteLine("\t\tbirth=yes");
            writer.WriteLine("\t}");

            if (character.getSpouse() != null) {
                DateTime maxBirthDT = maxBirth(character.getBirth(), character.getSpouse().getBirth());
                int yom = maxBirthDT.Year;
                int mom = maxBirthDT.Month + 1;
                int dom = maxBirthDT.Day + 1;
                writer.WriteLine("\t" + yom + "." + mom + "." + dom + "={");
                writer.WriteLine("\t\tadd_spouse=" + character.getSpouse().getID());
                writer.WriteLine("\t}");
            }

            if (character.getAlive() == false) {
                int dod = character.getDeathDay();
                int mod = character.getDeathMonth();
                int yod = character.getDeathYear();
                writer.WriteLine("\t" + yod + "." + mod + "." + dod + "={");
                writer.WriteLine("\t\tdeath=yes");
                writer.WriteLine("\t}");
            }

            writer.WriteLine("}");
        }

        private static DateTime maxBirth(DateTime d1, DateTime d2) {
            if (d1.Year == d2.Year) {
                if (d1.Month >= d2.Month) {
                    return d1;
                }
                else {
                    return d2;
                }
            }
            else if (d1.Year > d2.Year) {
                return d1;
            }
            else {
                return d2;
            }
        }

    }
}
