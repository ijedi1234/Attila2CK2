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
        private ProjectSettingsReader psr;
        private FactionsInfo factionsInfo;
        private RegionMapper regionMap;
        private AttilaRegionsInfo attilaRegionsInfo;
        private CK2RegionsInfo ck2RegionsInfo;
        private ReligionsInfo religionsInfo;
        private CultureMaps cultureMaps;

        public Form1() {
            cultureMaps = new CultureMaps();
            regionMap = new RegionMapper();
            ck2RegionsInfo = new CK2RegionsInfo(regionMap);
            religionsInfo = new ReligionsInfo();
            factionsInfo = new FactionsInfo();
            InitializeComponent();

            psr = new ProjectSettingsReader();
            string savegameXMLPath = psr.getSavegameXMLLocation();

            importantPaths = new ImportantPaths(savegameXMLPath);
            DateConverter dtConverter = new DateConverter(importantPaths);
            CharInfoCreator charInfoCreator = new CharInfoCreator(importantPaths, dtConverter, religionsInfo);
            FamilyTrees famTrees = new FamilyTrees(importantPaths, charInfoCreator, dtConverter);
            factionsInfo.updateImportantPaths(importantPaths);
            factionsInfo.readFactionXMLs();
            attilaRegionsInfo = new AttilaRegionsInfo(importantPaths, regionMap, factionsInfo);
            factionsInfo.updateFactionsInfo(attilaRegionsInfo);
            factionsInfo.readFamilyTrees(famTrees);

            DirectoryHierarchyCreator.createOutputDirectories();

            MoveFlags.move(factionsInfo);
            MoveCultures.move();

            OutputCommonLandedTitles.output(factionsInfo);
            OutputCommonDynasties.output(famTrees);
            //OutputCommonCultures.outputProvinceSpecific(attilaRegionsInfo);

            OutputCharacterHistories.output(factionsInfo);
            OutputProvinceHistories.output(attilaRegionsInfo, ck2RegionsInfo, religionsInfo);
            OutputTitleHistories.outputCountyHistory(factionsInfo);
            OutputTitleHistories.outputFactionTitleHistory(factionsInfo);

            OutputTitleLocalisation.output(factionsInfo);
            OutputCultureLocalisation.outputCultureGroups(cultureMaps);
            OutputCultureLocalisation.outputCultures(cultureMaps);
        }
    }
}
