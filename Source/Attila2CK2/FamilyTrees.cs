using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Attila2CK2 {
    class FamilyTrees {

        private List<CK2Character> allCharacters;
        private Dictionary<String, FamilyTree> trees;
        private List<List<string>> esfFamilyTreeStructure;

        public FamilyTrees(ImportantPaths paths, CharInfoCreator charInfoCreator) {
            allCharacters = new List<CK2Character>();
            findESFFamilyTreeStructure(charInfoCreator, paths);
            trees = new Dictionary<string, FamilyTree>();
            Dictionary<String, List<CK2Character>> charInfo = reformatCharInfo(charInfoCreator.getCharInfo());
            createTrees(charInfoCreator, charInfo);
        }

        private void findESFFamilyTreeStructure(CharInfoCreator charInfoCreator, ImportantPaths paths) {
            string worldPath = paths.getSavegameXMLPath() + "\\campaign_env\\world-0000.xml";
            XmlDocument doc = new XmlDocument();
            try {
                doc.Load(worldPath);
            }
            catch (Exception) { return; }
            XmlNode root = doc.DocumentElement;
            for (XmlNode node = root.FirstChild; node != null; node = node.NextSibling) {
                if (node.Attributes.Count == 0) {
                    continue;
                }
                XmlAttribute attr = node.Attributes[0];
                if (attr.Name == "type" && attr.InnerText == "FAMILY_TREE") {
                    extractESFFamilyTreeStructure(node, charInfoCreator);
                }
            }
        }

        private void extractESFFamilyTreeStructure(XmlNode root, CharInfoCreator charInfoCreator) {
            int treeSize = Int32.Parse(root.FirstChild.InnerText);
            esfFamilyTreeStructure = new List<List<string>>(treeSize);
            for (XmlNode node = root.FirstChild.NextSibling; node != null; node = node.NextSibling) {
                if (node.Name != "rec")
                    continue;
                List<string> treeContents = new List<string>();
                for (XmlNode treeContentItem = node.FirstChild; treeContentItem != null; treeContentItem = treeContentItem.NextSibling) {
                    if (treeContentItem.Name == "u") {
                        treeContents.Add(treeContentItem.InnerText);
                    }
                    else if (treeContentItem.Name == "no" || treeContentItem.Name == "yes") {
                        treeContents.Add(treeContentItem.Name);
                    }
                    else if (treeContentItem.Attributes.Count > 0 && treeContentItem.Attributes[0].InnerText == "CHARACTER_DETAILS") {
                        int detailCount = 0;
                        bool i1AryFound = false;
                        int countedUAfterPol = 0;
                        bool male = false;
                        int treeID = Int32.Parse(treeContents[0]);
                        string name = "";
                        for (XmlNode detailNode = treeContentItem.FirstChild; detailNode != null; detailNode = detailNode.NextSibling) {
                            if (detailCount == 11) {
                                male = (detailNode.Name == "yes");
                            }
                            if (detailNode.Name == "i1_ary") {
                                i1AryFound = true;
                            }
                            if (detailNode.Name == "u" && i1AryFound) {
                                countedUAfterPol++;
                                if (countedUAfterPol == 5) {
                                    treeID = Int32.Parse(detailNode.InnerText);
                                }
                            }
                            if (detailNode.Name != "rec") {
                                detailCount++;
                            }
                            if (detailNode.Attributes.Count == 0) {
                                continue;
                            }
                            XmlAttribute detailAttr = detailNode.Attributes[0];
                            if (detailAttr.Name == "type" && detailAttr.InnerText == "CHARACTER_NAME") {
                                name = charInfoCreator.extractName(detailNode);
                            }
                        }
                        CK2Character character = new CK2Character(name, 0, treeID, male);
                        allCharacters.Add(character);
                    }
                }
                esfFamilyTreeStructure.Add(treeContents);
            }
        }

        private Dictionary<String, List<CK2Character>> reformatCharInfo(List<Tuple<String, CK2Character>> charInfoBefore) {
            Dictionary<String, List<CK2Character>> charInfoAfter = new Dictionary<String, List<CK2Character>>();
            foreach (Tuple<String, CK2Character> tuple in charInfoBefore) {
                try {
                    List<CK2Character> charList = charInfoAfter[tuple.Item1];
                    charList.Add(tuple.Item2);
                }
                catch (Exception) {
                    List<CK2Character> charList = new List<CK2Character>();
                    charList.Add(tuple.Item2);
                    charInfoAfter.Add(tuple.Item1, charList);
                }
                allCharacters.Add(tuple.Item2);
            }
            return charInfoAfter;
        }

        private void createTrees(CharInfoCreator charInfoCreator, Dictionary<String, List<CK2Character>> charInfo) {
            Dictionary<int, CK2Character> fam2Char = new Dictionary<int, CK2Character>();
            foreach (CK2Character character in allCharacters) {
                fam2Char.Add(character.getFamilyTreeID(), character);
            }
            foreach (var pair in charInfo) {
                FamilyTree tree = new FamilyTree(charInfoCreator, pair.Value, fam2Char, esfFamilyTreeStructure);
                trees.Add(pair.Key, tree);
            }
        }

        public FamilyTree getTree(string factionID) {
            try {
                return trees[factionID];
            }
            catch (Exception) {
                return null;
            }
        }

    }
}
