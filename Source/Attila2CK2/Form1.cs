using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Attila2CK2 {
    public partial class Form1 : Form {

        private ImportantPaths importantPaths;
        private FactionsInfo factionsInfo;
        private RegionMapper regionMap;
        private AttilaRegionsInfo attilaRegionsInfo;
        private CK2RegionsInfo ck2RegionsInfo;
        private ReligionsInfo religionsInfo;

        public Form1() {
            regionMap = new RegionMapper();
            ck2RegionsInfo = new CK2RegionsInfo(regionMap);
            religionsInfo = new ReligionsInfo();
            factionsInfo = new FactionsInfo();
            InitializeComponent();
            string savegameXMLPath = "D:\\Program Files (x86)\\TotalWarEditors\\esfxml\\output\\compressed_data";
            importantPaths = new ImportantPaths(savegameXMLPath);
            factionsInfo.updateImportantPaths(importantPaths);
            factionsInfo.readFactionXMLs();
            attilaRegionsInfo = new AttilaRegionsInfo(importantPaths, regionMap, factionsInfo);
            factionsInfo.updateFactionsInfo(attilaRegionsInfo);
            factionsInfo.deriveFactionTree();
            DirectoryHierarchyCreator.createOutputDirectories();

            MoveFlags.move(factionsInfo);

            OutputCommonLandedTitles.output(factionsInfo);
            OutputCommonDynasties.output(factionsInfo);
            OutputCommonCultures.outputProvinceSpecific(attilaRegionsInfo);

            OutputCharacterHistories.output(factionsInfo);
            OutputProvinceHistories.output(attilaRegionsInfo, ck2RegionsInfo, religionsInfo);
            OutputTitleHistories.outputCountyHistory(factionsInfo);
            OutputTitleHistories.outputFactionTitleHistory(factionsInfo);

            OutputTitleLocalisation.output(factionsInfo);
        }
    }
}
