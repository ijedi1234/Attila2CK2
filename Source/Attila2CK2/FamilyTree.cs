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

        public FamilyTree(CharInfoCreator charInfoCreator, List<CK2Character> factionCharacters, Dictionary<int, CK2Character> familyID2Characters, List<List<string>> esfFamilyTreeStructure) {
            this.localCharacters = factionCharacters;
            deriveJobs(charInfoCreator);
            discoverTree(this.root, familyID2Characters, esfFamilyTreeStructure);
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

        private void deriveJobs(CharInfoCreator charInfoCreator) {
            foreach (CK2Character character in localCharacters) {
                string office = charInfoCreator.getJob(character.getESFID());
                if (office != null) {
                    character.setOffice(office);
                    if (office == "faction_leader") {
                        root = character;
                    }
                }
            }
        }

        private void discoverTree(CK2Character character, Dictionary<int, CK2Character> familyID2Characters, List<List<string>> esfFamilyTreeStructure) {
            int charsFamilyID = character.getFamilyTreeID();
            List<string> familyTreeInfo = esfFamilyTreeStructure[charsFamilyID - 1];
            int fatherID = Int32.Parse(familyTreeInfo[4]);
            int spouseID = Int32.Parse(familyTreeInfo[5]);
            //This is apparently spouse-related (?)
            int numSpouses = Int32.Parse(familyTreeInfo[6]);
            if (spouseID == 0 && numSpouses > 0) spouseID = Int32.Parse(familyTreeInfo[7]);
            int numChildrenPos = numSpouses + 7;
            int numChildren = Int32.Parse(familyTreeInfo[numChildrenPos]);
            List<int> childrenIDs = new List<int>(numChildren);
            for (int i = 0; i < numChildren; i++) {
                int childID = Int32.Parse(familyTreeInfo[numChildrenPos + 1 + i]);
                childrenIDs.Add(childID);
            }
            if (fatherID != 0) character.setFather(familyID2Characters[fatherID]);
            if (spouseID != 0) character.setSpouse(familyID2Characters[spouseID]);
            if (numChildren != 0) {
                List<CK2Character> children = new List<CK2Character>(numChildren);
                foreach (int childID in childrenIDs) {
                    children.Add(familyID2Characters[childID]);
                }
                if(children.Count == 0)
                    character.setChildren(null);
                else
                    character.setChildren(children);
            }
            //Skip the spouse. She is handled in the next run.
            {
                CK2Character father = character.getFather();
                if(father != null) {
                    bool hasChar = (father.getChildren() != null);
                    if (!hasChar) discoverTree(father, familyID2Characters, esfFamilyTreeStructure);
                }

                List<CK2Character> children = character.getChildren();
                if (children != null) {
                    foreach (CK2Character child in children) {
                        bool hasChar = (child.getFather() != null);
                        if (!hasChar) discoverTree(child, familyID2Characters, esfFamilyTreeStructure);
                    }
                }
            }
        }

        public CK2Character getOwner() { return root; }
        public CK2Dynasty getDynasty() { return dynasty; }

    }
}
