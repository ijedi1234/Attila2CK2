using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attila2CK2 {
    class DeJureTitles {

        private List<DeJureKingdom> titleHierarchy;

        public DeJureTitles(AttilaRegionsInfo attilaRegions) {
            List<AttilaRegionInfo> regionList = attilaRegions.getList();
            createTitleHierarchy(regionList);
        }

        private void createTitleHierarchy(List<AttilaRegionInfo> regionList) {
            titleHierarchy = new List<DeJureKingdom>();

            string curRegionName = regionList[0].getIDStr().Substring("att_reg_".Length);
            string nextRegionName = regionList[1].getIDStr().Substring("att_reg_".Length);

            int prevDiffPos = -1;
            int diffPos = diffLetterPosition(curRegionName, nextRegionName);
            int prevUnderPos = -1;
            int underPos = lastUnderscoreBeforeDiff(curRegionName, diffPos);

            string kingdomPart = curRegionName.Substring(0, underPos);
            string duchyPart = curRegionName.Substring(underPos + 1);
            DeJureKingdom kingdom = new DeJureKingdom(kingdomPart);
            titleHierarchy.Add(kingdom);
            DeJureDuchy duchy = new DeJureDuchy(duchyPart, regionList[0]);
            kingdom.addDuchy(duchy);
            bool makeNewKingdom = false;
            for (int i = 1; i < regionList.Count; i++) {

                curRegionName = regionList[i].getIDStr().Substring("att_reg_".Length);
                nextRegionName = "";
                if (i != regionList.Count - 1) {
                    nextRegionName = regionList[i + 1].getIDStr().Substring("att_reg_".Length);
                }

                prevDiffPos = diffPos;
                diffPos = diffLetterPosition(curRegionName, nextRegionName);
                prevUnderPos = underPos;
                underPos = lastUnderscoreBeforeDiff(curRegionName, diffPos);

                if (underPos == prevUnderPos || makeNewKingdom) {
                    duchyPart = curRegionName.Substring(underPos + 1);
                    duchy = new DeJureDuchy(duchyPart, regionList[i]);
                    if (makeNewKingdom) {
                        kingdomPart = curRegionName.Substring(0, underPos);
                        kingdom = new DeJureKingdom(kingdomPart);
                        titleHierarchy.Add(kingdom);
                        makeNewKingdom = false;
                    }
                    kingdom.addDuchy(duchy);
                }
                else {
                    duchyPart = curRegionName.Substring(prevUnderPos + 1);
                    duchy = new DeJureDuchy(duchyPart, regionList[i]);
                    kingdom.addDuchy(duchy);
                    makeNewKingdom = true;
                }
            }
        }

        private int diffLetterPosition(string s1, string s2) {
            int pos = 0;
            int size = Math.Min(s1.Length, s2.Length);
            for (int i = 0; i < size; i++) {
                if (s1[i] != s2[i]) return pos;
                pos++;
            }
            return pos;
        }

        private int lastUnderscoreBeforeDiff(string str, int diffPos) {
            int pos = diffPos;
            for (int i = diffPos; i >= 0; i--) {
                if (str[i] == '_') return pos;
                pos--;
            }
            return pos;
        }

        public static string deriveScreenName(string name) {
            string screenName = "";
            bool encounteredUnderscore = true;
            for (int i = 0; i < name.Length; i++) {
                string item = name[i].ToString();
                if (encounteredUnderscore) {
                    item = item.ToUpper();
                    encounteredUnderscore = false;
                }
                if (item == "_") {
                    item = " ";
                    encounteredUnderscore = true;
                }
                screenName += item;
            }
            return screenName;
        }

        public List<DeJureKingdom> getTitles() { return titleHierarchy; }

    }
}
