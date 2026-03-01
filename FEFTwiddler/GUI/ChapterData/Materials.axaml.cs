using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace FEFTwiddler.GUI.ChapterData
{
    public partial class Materials : UserControl
    {
        private Model.IChapterSave? _chapterSave;
        private bool _loading;

        public Materials()
        {
            InitializeComponent();
        }

        public void LoadChapterSave(Model.IChapterSave chapterSave)
        {
            _chapterSave = chapterSave;
            _loading = true;
            PopulateControls();
            _loading = false;
            BindEvents();
        }

        private static Bitmap? LoadMaterialIcon(string name)
        {
            try { return new Bitmap(AssetLoader.Open(new System.Uri($"avares://FEFTwiddler/Resources/Images/MaterialIcons/Material_{name}.png"))); }
            catch { return null; }
        }

        private void PopulateControls()
        {
            imgAmber.Source = LoadMaterialIcon("amber");
            imgBeans.Source = LoadMaterialIcon("beans");
            imgBerries.Source = LoadMaterialIcon("berries");
            imgCabbage.Source = LoadMaterialIcon("cabbage");
            imgCoral.Source = LoadMaterialIcon("coral");
            imgCrystal.Source = LoadMaterialIcon("crystal");
            imgDaikon.Source = LoadMaterialIcon("daikon");
            imgEmerald.Source = LoadMaterialIcon("emerald");
            imgFish.Source = LoadMaterialIcon("fish");
            imgJade.Source = LoadMaterialIcon("jade");
            imgLapis.Source = LoadMaterialIcon("lapis");
            imgMeat.Source = LoadMaterialIcon("meat");
            imgMilk.Source = LoadMaterialIcon("milk");
            imgOnyx.Source = LoadMaterialIcon("onyx");
            imgPeaches.Source = LoadMaterialIcon("peaches");
            imgPearl.Source = LoadMaterialIcon("pearl");
            imgQuartz.Source = LoadMaterialIcon("quartz");
            imgRice.Source = LoadMaterialIcon("rice");
            imgRuby.Source = LoadMaterialIcon("ruby");
            imgSapphire.Source = LoadMaterialIcon("sapphire");
            imgTopaz.Source = LoadMaterialIcon("topaz");
            imgWheat.Source = LoadMaterialIcon("wheat");

            var r = _chapterSave!.MyCastleRegion;
            numAmber.Value = r.MaterialQuantity_Amber;
            numBeans.Value = r.MaterialQuantity_Beans;
            numBerries.Value = r.MaterialQuantity_Berries;
            numCabbage.Value = r.MaterialQuantity_Cabbage;
            numCoral.Value = r.MaterialQuantity_Coral;
            numCrystal.Value = r.MaterialQuantity_Crystal;
            numDaikon.Value = r.MaterialQuantity_Daikon;
            numEmerald.Value = r.MaterialQuantity_Emerald;
            numFish.Value = r.MaterialQuantity_Fish;
            numJade.Value = r.MaterialQuantity_Jade;
            numLapis.Value = r.MaterialQuantity_Lapis;
            numMeat.Value = r.MaterialQuantity_Meat;
            numMilk.Value = r.MaterialQuantity_Milk;
            numOnyx.Value = r.MaterialQuantity_Onyx;
            numPeaches.Value = r.MaterialQuantity_Peaches;
            numPearl.Value = r.MaterialQuantity_Pearl;
            numQuartz.Value = r.MaterialQuantity_Quartz;
            numRice.Value = r.MaterialQuantity_Rice;
            numRuby.Value = r.MaterialQuantity_Ruby;
            numSapphire.Value = r.MaterialQuantity_Sapphire;
            numTopaz.Value = r.MaterialQuantity_Topaz;
            numWheat.Value = r.MaterialQuantity_Wheat;
        }

        private void BindEvents()
        {
            var r = _chapterSave!.MyCastleRegion;
            numAmber.ValueChanged += (_, _) => { if (!_loading) { r.MaterialDiscovered_Amber = true; r.MaterialQuantity_Amber = (byte)(numAmber.Value ?? 0); } };
            numBeans.ValueChanged += (_, _) => { if (!_loading) { r.MaterialDiscovered_Beans = true; r.MaterialQuantity_Beans = (byte)(numBeans.Value ?? 0); } };
            numBerries.ValueChanged += (_, _) => { if (!_loading) { r.MaterialDiscovered_Berries = true; r.MaterialQuantity_Berries = (byte)(numBerries.Value ?? 0); } };
            numCabbage.ValueChanged += (_, _) => { if (!_loading) { r.MaterialDiscovered_Cabbage = true; r.MaterialQuantity_Cabbage = (byte)(numCabbage.Value ?? 0); } };
            numCoral.ValueChanged += (_, _) => { if (!_loading) { r.MaterialDiscovered_Coral = true; r.MaterialQuantity_Coral = (byte)(numCoral.Value ?? 0); } };
            numCrystal.ValueChanged += (_, _) => { if (!_loading) { r.MaterialDiscovered_Crystal = true; r.MaterialQuantity_Crystal = (byte)(numCrystal.Value ?? 0); } };
            numDaikon.ValueChanged += (_, _) => { if (!_loading) { r.MaterialDiscovered_Daikon = true; r.MaterialQuantity_Daikon = (byte)(numDaikon.Value ?? 0); } };
            numEmerald.ValueChanged += (_, _) => { if (!_loading) { r.MaterialDiscovered_Emerald = true; r.MaterialQuantity_Emerald = (byte)(numEmerald.Value ?? 0); } };
            numFish.ValueChanged += (_, _) => { if (!_loading) { r.MaterialDiscovered_Fish = true; r.MaterialQuantity_Fish = (byte)(numFish.Value ?? 0); } };
            numJade.ValueChanged += (_, _) => { if (!_loading) { r.MaterialDiscovered_Jade = true; r.MaterialQuantity_Jade = (byte)(numJade.Value ?? 0); } };
            numLapis.ValueChanged += (_, _) => { if (!_loading) { r.MaterialDiscovered_Lapis = true; r.MaterialQuantity_Lapis = (byte)(numLapis.Value ?? 0); } };
            numMeat.ValueChanged += (_, _) => { if (!_loading) { r.MaterialDiscovered_Meat = true; r.MaterialQuantity_Meat = (byte)(numMeat.Value ?? 0); } };
            numMilk.ValueChanged += (_, _) => { if (!_loading) { r.MaterialDiscovered_Milk = true; r.MaterialQuantity_Milk = (byte)(numMilk.Value ?? 0); } };
            numOnyx.ValueChanged += (_, _) => { if (!_loading) { r.MaterialDiscovered_Onyx = true; r.MaterialQuantity_Onyx = (byte)(numOnyx.Value ?? 0); } };
            numPeaches.ValueChanged += (_, _) => { if (!_loading) { r.MaterialDiscovered_Peaches = true; r.MaterialQuantity_Peaches = (byte)(numPeaches.Value ?? 0); } };
            numPearl.ValueChanged += (_, _) => { if (!_loading) { r.MaterialDiscovered_Pearl = true; r.MaterialQuantity_Pearl = (byte)(numPearl.Value ?? 0); } };
            numQuartz.ValueChanged += (_, _) => { if (!_loading) { r.MaterialDiscovered_Quartz = true; r.MaterialQuantity_Quartz = (byte)(numQuartz.Value ?? 0); } };
            numRice.ValueChanged += (_, _) => { if (!_loading) { r.MaterialDiscovered_Rice = true; r.MaterialQuantity_Rice = (byte)(numRice.Value ?? 0); } };
            numRuby.ValueChanged += (_, _) => { if (!_loading) { r.MaterialDiscovered_Ruby = true; r.MaterialQuantity_Ruby = (byte)(numRuby.Value ?? 0); } };
            numSapphire.ValueChanged += (_, _) => { if (!_loading) { r.MaterialDiscovered_Sapphire = true; r.MaterialQuantity_Sapphire = (byte)(numSapphire.Value ?? 0); } };
            numTopaz.ValueChanged += (_, _) => { if (!_loading) { r.MaterialDiscovered_Topaz = true; r.MaterialQuantity_Topaz = (byte)(numTopaz.Value ?? 0); } };
            numWheat.ValueChanged += (_, _) => { if (!_loading) { r.MaterialDiscovered_Wheat = true; r.MaterialQuantity_Wheat = (byte)(numWheat.Value ?? 0); } };
        }

        private void BtnMaxMaterials_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            _loading = true;
            numAmber.Value = numBeans.Value = numBerries.Value = numCabbage.Value = numCoral.Value =
            numCrystal.Value = numDaikon.Value = numEmerald.Value = numFish.Value = numJade.Value =
            numLapis.Value = numMeat.Value = numMilk.Value = numOnyx.Value = numPeaches.Value =
            numPearl.Value = numQuartz.Value = numRice.Value = numRuby.Value = numSapphire.Value =
            numTopaz.Value = numWheat.Value = 99;
            _loading = false;
            // Apply to model
            var r = _chapterSave!.MyCastleRegion;
            r.MaterialDiscovered_Amber = r.MaterialDiscovered_Beans = r.MaterialDiscovered_Berries =
            r.MaterialDiscovered_Cabbage = r.MaterialDiscovered_Coral = r.MaterialDiscovered_Crystal =
            r.MaterialDiscovered_Daikon = r.MaterialDiscovered_Emerald = r.MaterialDiscovered_Fish =
            r.MaterialDiscovered_Jade = r.MaterialDiscovered_Lapis = r.MaterialDiscovered_Meat =
            r.MaterialDiscovered_Milk = r.MaterialDiscovered_Onyx = r.MaterialDiscovered_Peaches =
            r.MaterialDiscovered_Pearl = r.MaterialDiscovered_Quartz = r.MaterialDiscovered_Rice =
            r.MaterialDiscovered_Ruby = r.MaterialDiscovered_Sapphire = r.MaterialDiscovered_Topaz =
            r.MaterialDiscovered_Wheat = true;
            r.MaterialQuantity_Amber = r.MaterialQuantity_Beans = r.MaterialQuantity_Berries =
            r.MaterialQuantity_Cabbage = r.MaterialQuantity_Coral = r.MaterialQuantity_Crystal =
            r.MaterialQuantity_Daikon = r.MaterialQuantity_Emerald = r.MaterialQuantity_Fish =
            r.MaterialQuantity_Jade = r.MaterialQuantity_Lapis = r.MaterialQuantity_Meat =
            r.MaterialQuantity_Milk = r.MaterialQuantity_Onyx = r.MaterialQuantity_Peaches =
            r.MaterialQuantity_Pearl = r.MaterialQuantity_Quartz = r.MaterialQuantity_Rice =
            r.MaterialQuantity_Ruby = r.MaterialQuantity_Sapphire = r.MaterialQuantity_Topaz =
            r.MaterialQuantity_Wheat = 99;
        }
    }
}
