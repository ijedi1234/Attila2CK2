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
        private string religion;
        private string culture;
        Dictionary<int, CK2Character> fam2Char;
        HashSet<CK2Dynasty> associatedDynasties;

        public FamilyTree(CharInfoCreator charInfoCreator, List<CK2Character> factionCharacters, Dictionary<int, CK2Character> familyID2Characters, List<List<string>> esfFamilyTreeStructure) {
            this.associatedDynasties = new HashSet<CK2Dynasty>();
            this.localCharacters = factionCharacters;
            this.fam2Char = familyID2Characters;
            deriveJobs(charInfoCreator);
            if (this.root.getCulture() == "") this.root.setCulture("english");
            religion = this.root.getReligion();
            culture = this.root.getCulture();
            discoverTree(this.root, familyID2Characters, esfFamilyTreeStructure);
            //dynasty = new CK2Dynasty("HolderTree");
            dynasty = this.deriveDynasty(root);
            updateDynasty(dynasty, root, true);
            updateReligion(root, true);
            updateCulture(root, true);
        }

        public void updateDynasty(CK2Dynasty newDynasty, CK2Character character, bool canOverwrite) {
            associatedDynasties.Add(newDynasty);
            HashSet<int> processed = new HashSet<int>();
            updateDynastyForCharacter(character, newDynasty, canOverwrite, processed);
        }

        private void updateDynastyForCharacter(CK2Character character, CK2Dynasty newDynasty, bool canOverwrite, HashSet<int> processed) {
            bool hasChar = !(processed.Add(character.getFamilyTreeID()));
            if (hasChar) return;
            if(character.getDynasty() == null || canOverwrite)
                character.setDynasty(newDynasty);
            CK2Character father = character.getFather();
            if (father != null) {
                //CK2Dynasty fatherDynasty = father.getDynasty();
                //if (fatherDynasty == null || fatherDynasty.getID() != newDynasty.getID())
                updateDynastyForCharacter(father, newDynasty, canOverwrite, processed);
            }
            List<CK2Character> children = character.getChildren();
            if(children != null)
                foreach (CK2Character child in children) {
                    //CK2Dynasty childDynasty = child.getDynasty();
                    //if (childDynasty == null || childDynasty.getID() != newDynasty.getID())
                    updateDynastyForCharacter(child, newDynasty, canOverwrite, processed);
                }
        }

        public void updateReligion(CK2Character character, bool canOverwrite) {
            HashSet<int> processed = new HashSet<int>();
            updateReligionForCharacter(character, canOverwrite, processed);
        }

        public void updateReligionForCharacter(CK2Character character, bool canOverwrite, HashSet<int> processed) {
            bool hasChar = !(processed.Add(character.getFamilyTreeID()));
            if (hasChar) return;
            if(character.getReligion() == null || canOverwrite)
                character.setReligion(this.religion);
            CK2Character father = character.getFather();
            if (father != null) {
                updateReligionForCharacter(father, canOverwrite, processed);
            }
            List<CK2Character> children = character.getChildren();
            if (children != null)
                foreach (CK2Character child in children) {
                    updateReligionForCharacter(child, canOverwrite, processed);
                }
        }

        public void updateCulture(CK2Character character, bool canOverwrite) {
            HashSet<int> processed = new HashSet<int>();
            updateCultureForCharacter(character, canOverwrite, processed);
        }

        public void updateCultureForCharacter(CK2Character character, bool canOverwrite, HashSet<int> processed) {
            bool hasChar = !(processed.Add(character.getFamilyTreeID()));
            if (hasChar) return;
            if (character.getCulture() == null || character.getCulture() == "" || canOverwrite)
                character.setCulture(this.culture);
            CK2Character father = character.getFather();
            if (father != null) {
                updateCultureForCharacter(father, canOverwrite, processed);
            }
            List<CK2Character> children = character.getChildren();
            if (children != null)
                foreach (CK2Character child in children) {
                    updateCultureForCharacter(child, canOverwrite, processed);
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

        private CK2Dynasty deriveDynasty(CK2Character character) {
            CK2Character curChar = character;
            while (curChar.getFather() != null) {
                curChar = curChar.getFather();
            }
            CK2Dynasty dynasty = new CK2Dynasty(curChar.getName());
            return dynasty;
        }

        private void discoverTree(CK2Character character, Dictionary<int, CK2Character> familyID2Characters, List<List<string>> esfFamilyTreeStructure) {
            int charsFamilyID = character.getFamilyTreeID();
            List<string> familyTreeInfo = esfFamilyTreeStructure[charsFamilyID - 1];
            int fatherID = Int32.Parse(familyTreeInfo[4]);
            int spouseID = Int32.Parse(familyTreeInfo[5]);
            //This is apparently spouse-related (?)
            int numSpousesPos = 6;
            int numSpouses = Int32.Parse(familyTreeInfo[numSpousesPos]);
            if (spouseID == 0 && numSpouses > 0) spouseID = Int32.Parse(familyTreeInfo[numSpousesPos + 1]);
            int numChildrenPos = numSpouses + numSpousesPos + 1;
            int numChildren = Int32.Parse(familyTreeInfo[numChildrenPos]);
            int numAdoptedPos = numChildren + numChildrenPos + 1;
            int numAdopted = Int32.Parse(familyTreeInfo[numAdoptedPos]);
            List<int> childrenIDs = new List<int>(numChildren);
            for (int i = 0; i < numChildren; i++) {
                int childID = Int32.Parse(familyTreeInfo[numChildrenPos + i + 1]);
                childrenIDs.Add(childID);
            }
            for (int i = 0; i < numAdopted; i++) {
                int childID = Int32.Parse(familyTreeInfo[numAdoptedPos + i + 1]);
                childrenIDs.Add(childID);
            }
            if (fatherID != 0) character.setFather(familyID2Characters[fatherID]);
            if (spouseID != 0) character.setSpouse(familyID2Characters[spouseID]);
            if ((numChildren + numAdopted) != 0) {
                List<CK2Character> children = new List<CK2Character>(numChildren + numAdopted);
                foreach (int childID in childrenIDs) {
                    children.Add(familyID2Characters[childID]);
                }
                if(children.Count == 0)
                    character.setChildren(null);
                else
                    character.setChildren(children);
            }
            int booleanPrefacePos = numAdopted + numAdoptedPos + 1;
            int booleanListPos = booleanPrefacePos + 2;
            bool isBastard = (familyTreeInfo[booleanListPos + 2] == "yes");
            character.setIsBastard(isBastard);

            //Skip the female->male spouse. She is handled in the next run.
            {
                CK2Character father = character.getFather();
                if(father != null) {
                    bool hasChar = (father.getChildren() != null);
                    if (!hasChar) discoverTree(father, familyID2Characters, esfFamilyTreeStructure);
                    if (character.getBirth().CompareTo(father.getBirth()) <= 0) {
                        character.incrementBirthDay(father);
                    }
                }

                List<CK2Character> children = character.getChildren();
                if (children != null) {
                    foreach (CK2Character child in children) {
                        bool hasChar = (child.getFather() != null);
                        if (!hasChar) discoverTree(child, familyID2Characters, esfFamilyTreeStructure);
                    }
                }

                //CK2Character spouse = character.getSpouse();
                //if (character.getIsMale() && spouse != null) {
                //    discoverTree(spouse, familyID2Characters, esfFamilyTreeStructure);
                //}
            }
        }

        public void updateSpouses(CK2Character character, HashSet<int> processed, List<List<string>> esfFamilyTreeStructure) {
            bool hasChar = !(processed.Add(character.getID()));
            if(hasChar) return;
            CK2Character spouse = character.getSpouse();
            if (character.getSpouse() != null) {
                CK2Dynasty spouseDynasty = character.getSpouse().getDynasty();
                if (spouseDynasty == null) {
                    discoverTree(spouse, fam2Char, esfFamilyTreeStructure);
                    spouseDynasty = this.deriveDynasty(spouse);
                    associatedDynasties.Add(spouseDynasty);
                    updateDynasty(spouseDynasty, spouse, false);
                    updateReligion(spouse, false);
                    updateCulture(spouse, false);
                }
            }
            CK2Character father = character.getFather();
            if (father != null) {
                updateSpouses(father, processed, esfFamilyTreeStructure);
            }
            List<CK2Character> children = character.getChildren();
            if (children != null)
                foreach (CK2Character child in children) {
                    updateSpouses(child, processed, esfFamilyTreeStructure);
                }
        }



        public CK2Character getOwner() { return root; }
        public CK2Dynasty getDynasty() { return dynasty; }
        public HashSet<CK2Dynasty> getAssociatedDynasties() { return associatedDynasties; }

    }
}
