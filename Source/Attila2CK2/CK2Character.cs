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

        private int birthMonth;
        private int birthDay;
        private int birthYear;

        public CK2Character(string name, CK2Dynasty dynasty) {
            this.name = name;
            this.dynasty = dynasty;
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
        public CK2Dynasty getDynasty() { return dynasty; }

    }
}
