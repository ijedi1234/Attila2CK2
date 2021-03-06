﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Attila2CK2 {
    class CK2Character {

        private static int newIDnum = deriveFirstCharID();

        private int id;
        private int esfID;
        private int treeID;

        private bool isMale;
        private string name;
        private string office;

        private bool isRoot;

        private DateTime birth;
        private DateTime death;

        private string religion;
        private string culture;

        private CK2Dynasty dynasty;
        private CK2Character father;
        private List<CK2Character> children;
        private CK2Character spouse;
        private bool isBastard;

        public CK2Character() {
            this.name = "Place";
            this.id = CK2Character.newIDnum;
            CK2Character.newIDnum++;
            this.birth = new DateTime(740, 4, 2);
            this.death = new DateTime(1, 1, 1);
            this.isMale = true;
            this.culture = "";
        }

        public CK2Character(string name, int esfID, int treeID, bool male, DateTime birth, DateTime death, string religion, string culture) {
            this.name = name;
            this.esfID = esfID;
            this.treeID = treeID;
            this.id = CK2Character.newIDnum;
            CK2Character.newIDnum++;
            this.isMale = male;
            this.religion = religion;
            this.birth = birth;
            this.death = death;
            this.culture = culture;
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

        public void incrementBirthDay(CK2Character character) {
            this.birth = character.getBirth().AddDays(1);
        }

        private static int deriveFirstCharID() {
            string settingsLoc = ImportantPaths.conversionInfoPath() + "\\settings.xml";
            XmlDocument doc = new XmlDocument();
            try {
                doc.Load(settingsLoc);
            }
            catch (Exception) {
                return 1000001;
            }
            XmlNode root = doc.DocumentElement;
            for (XmlNode node = root.FirstChild; node != null; node = node.NextSibling) {
                if (node.Name == "firstCharID") {
                    return Int32.Parse(node.InnerText);
                }
            }
            return 1000001;
        }

        public int getID() { return id; }
        public int getESFID() { return esfID; }
        public int getFamilyTreeID() { return treeID; }
        public string getName() { return name; }

        public bool getIsMale() { return isMale; }

        public void setReligion(string religion) { this.religion = religion; }
        public string getReligion() { return religion; }
        public void setCulture(string culture) { this.culture = culture; }
        public string getCulture() { return culture; }

        public DateTime getBirth() { return birth; }
        public int getBirthMonth() { return birth.Month; }
        public int getBirthDay() { return birth.Day; }
        public int getBirthYear() { return birth.Year; }
        public int getDeathMonth() { return death.Month; }
        public int getDeathDay() { return death.Day; }
        public int getDeathYear() { return death.Year; }
        public bool getAlive() { return death.Year == 1; }

        public void setDynasty(CK2Dynasty dynasty) { this.dynasty = dynasty; }
        public CK2Dynasty getDynasty() { return dynasty; }
        public bool getIsRoot() { return isRoot; }

        public CK2Character getFather() { return father; }
        public CK2Character getSpouse() { return spouse; }
        public List<CK2Character> getChildren() { return children; }
        public void setIsBastard(bool bastard) { this.isBastard = bastard; }
        public bool getIsBastard() { return isBastard; }

    }
}
