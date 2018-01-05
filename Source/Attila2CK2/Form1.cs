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
        private CK2CountyRegionsInfo ck2RegionsInfo;
        private ReligionsInfo religionsInfo;
        private CultureMaps cultureMaps;
        private DeJureTitles deJureTitles;

        public Form1() {
            cultureMaps = new CultureMaps();
            regionMap = new RegionMapper();
            ck2RegionsInfo = new CK2CountyRegionsInfo(regionMap);
            religionsInfo = new ReligionsInfo();
            factionsInfo = new FactionsInfo();

            InitializeComponent();

            psr = new ProjectSettingsReader();
            string savegameXMLPath = psr.getSavegameXMLLocation();

            importantPaths = new ImportantPaths(savegameXMLPath);


            //Init dating system
            DateConverter dtConverter = new DateConverter(importantPaths);

            //Generate characters located in faction array (these characters are alive)
            CharInfoCreator charInfoCreator = new CharInfoCreator(importantPaths, dtConverter, religionsInfo);

            //Build family tree and generate dead characters
            FamilyTrees famTrees = new FamilyTrees(importantPaths, charInfoCreator, dtConverter);

            //Update faction-specific information with save game data
            factionsInfo.updateImportantPaths(importantPaths);
            factionsInfo.readFactionXMLs();

            //Init region information
            attilaRegionsInfo = new AttilaRegionsInfo(importantPaths, regionMap, factionsInfo);

            //Generate de jure titles
            deJureTitles = new DeJureTitles(attilaRegionsInfo);

            //Update other information regarding factions and regions
            factionsInfo.updateFactionsInfo(attilaRegionsInfo);

            //Process family trees with regards to faction settings. (Dynasty update occurs here)
            factionsInfo.readFamilyTrees(famTrees);


            DirectoryHierarchyCreator.createOutputDirectories();

            MoveFlags.move(factionsInfo);
            MoveCultures.move();

            OutputCommonLandedTitles.output(factionsInfo);
            OutputCommonLandedTitles.outputDeJure(deJureTitles);
            OutputCommonDynasties.output(famTrees);
            //OutputCommonCultures.outputProvinceSpecific(attilaRegionsInfo);

            OutputCharacterHistories.output(factionsInfo);
            OutputProvinceHistories.output(attilaRegionsInfo, ck2RegionsInfo, religionsInfo);
            OutputTitleHistories.outputCountyHistory(factionsInfo);
            OutputTitleHistories.outputFactionTitleHistory(factionsInfo);

            OutputTitleLocalisation.output(factionsInfo);
            OutputTitleLocalisation.outputDeJure(deJureTitles);
            OutputCultureLocalisation.outputCultureGroups(cultureMaps);
            OutputCultureLocalisation.outputCultures(cultureMaps);
        }
    }
}
