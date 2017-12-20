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

        public FamilyTree(List<CK2Character> factionCharacters, List<CK2Character> allCharacters) {
            this.localCharacters = factionCharacters;
            this.root = factionCharacters[0];
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

        public CK2Character getOwner() { return root; }
        public CK2Dynasty getDynasty() { return dynasty; }

    }
}
