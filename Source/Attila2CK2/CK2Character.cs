using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attila2CK2 {
    class CK2Character {

        private static int newIDnum = 157600001;

        private int id;
        private string name;
        private CK2Dynasty dynasty;
        private bool alive;

        private int birthMonth;
        private int birthDay;
        private int birthYear;

        private CK2Character father;
        private List<CK2Character> children;

        public CK2Character() {
            this.name = "Place";
            this.id = CK2Character.newIDnum;
            CK2Character.newIDnum++;
            this.birthMonth = 4;
            this.birthDay = 2;
            this.birthYear = 740;
        }

        public CK2Character(string name) {
            this.name = name;
            this.id = CK2Character.newIDnum;
            CK2Character.newIDnum++;
            this.birthMonth = 4;
            this.birthDay = 2;
            this.birthYear = 740;
        }

        public int getID() { return id; }
        public string getName() { return name; }
        public int getBirthMonth() { return birthMonth; }
        public int getBirthDay() { return birthDay; }
        public int getBirthYear() { return birthYear; }
        public void setDynasty(CK2Dynasty dynasty) { this.dynasty = dynasty; }
        public CK2Dynasty getDynasty() { return dynasty; }

    }
}
