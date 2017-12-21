using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attila2CK2 {
    class CK2Character {

        private static int newIDnum = 157600001;

        private int id;
        private int esfID;
        private int treeID;

        private bool isMale;
        private string name;
        private string office;

        private bool isRoot;

        private int birthMonth;
        private int birthDay;
        private int birthYear;

        private CK2Dynasty dynasty;
        private CK2Character father;
        private List<CK2Character> children;
        private CK2Character spouse;

        public CK2Character() {
            this.name = "Place";
            this.id = CK2Character.newIDnum;
            CK2Character.newIDnum++;
            this.birthMonth = 4;
            this.birthDay = 2;
            this.birthYear = 740;
        }

        public CK2Character(string name, int esfID, int treeID, bool male) {
            this.name = name;
            this.esfID = esfID;
            this.treeID = treeID;
            this.id = CK2Character.newIDnum;
            CK2Character.newIDnum++;
            this.isMale = male;
            this.birthMonth = 4;
            this.birthDay = 2;
            this.birthYear = 740;
        }

        public void setOffice(string office) { 
            this.office = office;
            if (office == "faction_leader")
                this.isRoot = true;
        }

        public void setFather(CK2Character father) {
            this.father = father;
        }

        public void setSpouse(CK2Character spouse) {
            this.spouse = spouse;
        }

        public void setChildren(List<CK2Character> children) {
            this.children = children;
        }

        public int getID() { return id; }
        public int getESFID() { return esfID; }
        public int getFamilyTreeID() { return treeID; }
        public string getName() { return name; }
        public int getBirthMonth() { return birthMonth; }
        public int getBirthDay() { return birthDay; }
        public int getBirthYear() { return birthYear; }
        public void setDynasty(CK2Dynasty dynasty) { this.dynasty = dynasty; }
        public CK2Dynasty getDynasty() { return dynasty; }
        public bool getIsRoot() { return isRoot; }

        public CK2Character getFather() { return father; }
        public CK2Character getSpouse() { return spouse; }
        public List<CK2Character> getChildren() { return children; }

    }
}
