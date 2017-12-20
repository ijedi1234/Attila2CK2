using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attila2CK2 {
    class FamilyTrees {

        private List<CK2Character> allCharacters;
        private Dictionary<String, FamilyTree> trees;

        public FamilyTrees(CharInfoCreator charInfoCreator) {
            allCharacters = new List<CK2Character>();
            trees = new Dictionary<string, FamilyTree>();
            Dictionary<String, List<CK2Character>> charInfo = reformatCharInfo(charInfoCreator.getCharInfo());
            createTrees(charInfo);
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

        private void createTrees(Dictionary<String, List<CK2Character>> charInfo) {
            foreach (var pair in charInfo) {
                FamilyTree tree = new FamilyTree(pair.Value, allCharacters);
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
