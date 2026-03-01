using Avalonia.Controls;

namespace FEFTwiddler.GUI.ChapterData
{
    public partial class GoldAndPoints : UserControl
    {
        private Model.IChapterSave? _chapterSave;
        private bool _loading;

        public GoldAndPoints()
        {
            InitializeComponent();
        }

        public void LoadChapterSave(Model.IChapterSave chapterSave)
        {
            _chapterSave = chapterSave;
            _loading = true;
            PopulateControls();
            _loading = false;
            numGold.ValueChanged += (_, _) => { if (!_loading) _chapterSave!.UserRegion.Gold = (uint)(numGold.Value ?? 0); };
            numDragonVeinPoints.ValueChanged += (_, _) => { if (!_loading) _chapterSave!.MyCastleRegion.DragonVeinPoint = (ushort)((numDragonVeinPoints.Value ?? 0) * 100); };
        }

        private void PopulateControls()
        {
            numGold.Value = _chapterSave!.UserRegion.Gold;
            numDragonVeinPoints.Value = _chapterSave.MyCastleRegion.DragonVeinPoint / 100;
        }

        private void BtnMaxGold_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            numGold.Value = 999999;
        }

        private void Btn99DragonVeinPoints_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            numDragonVeinPoints.Value = 99;
        }
    }
}
