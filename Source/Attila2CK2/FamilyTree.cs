using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attila2CK2 {
    class FamilyTree {

        List<CK2Character> localCharacters;
        private CK2Character root;
        private CK2Dynasty dynasty;

        public FamilyTree(CharInfoCreator charInfoCreator, List<CK2Character> factionCharacters, List<CK2Character> allCharacters) {
            this.localCharacters = factionCharacters;
            deriveJobs(charInfoCreator, factionCharacters);
            dynasty = new CK2Dynasty("HolderTree");
            foreach (CK2Character character in factionCharacters) {
                character.setDynasty(dynasty);
            }
        }

        public void replaceDynasty(CK2Dynasty newDynasty) {
            foreach (CK2Character character in localCharacters) {
                character.setDynasty(newDynasty);
            }
        }

        private void deriveJobs(CharInfoCreator charInfoCreator, List<CK2Character> factionCharacters) {
            foreach(CK2Character character in factionCharacters) {
                string office = charInfoCreator.getJob(character.getESFID());
                if (office != null) {
                    character.setOffice(office);
                    if (office == "faction_leader") {
                        root = character;
                    }
                }
            }
        }

        public CK2Character getOwner() { return root; }
        public CK2Dynasty getDynasty() { return dynasty; }

    }
}
