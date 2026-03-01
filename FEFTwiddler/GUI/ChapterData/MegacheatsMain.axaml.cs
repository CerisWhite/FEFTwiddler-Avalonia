using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.VisualTree;

namespace FEFTwiddler.GUI.ChapterData
{
    public partial class MegacheatsMain : UserControl
    {
        private Model.IChapterSave? _chapterSave;

        public MegacheatsMain()
        {
            InitializeComponent();
        }

        public void LoadChapterSave(Model.IChapterSave chapterSave)
        {
            _chapterSave = chapterSave;
            skills1.LoadChapterSave(_chapterSave);
        }

        private async void BtnAllCharMaxStatue_Click(object? sender, RoutedEventArgs e)
        {
            foreach (var unit in _chapterSave!.UnitRegion.Units) unit.MaximizeStatues();
            await MsgBox.ShowInfo(TopLevel.GetTopLevel(this) as Window ?? throw new InvalidOperationException(), "Done!");
        }

        private async void BtnGiveEternalSeals_Click(object? sender, RoutedEventArgs e)
        {
            foreach (var unit in _chapterSave!.UnitRegion.Units) unit.EternalSealsUsed = 16;
            await MsgBox.ShowInfo(TopLevel.GetTopLevel(this) as Window ?? throw new InvalidOperationException(), "Done!");
        }

        private async void BtnMaxWeaponExp_Click(object? sender, RoutedEventArgs e)
        {
            foreach (var unit in _chapterSave!.UnitRegion.Units) unit.SRankAllWeapons();
            await MsgBox.ShowInfo(TopLevel.GetTopLevel(this) as Window ?? throw new InvalidOperationException(), "Done!");
        }

        private async void BtnMaxBoots_Click(object? sender, RoutedEventArgs e)
        {
            foreach (var unit in _chapterSave!.UnitRegion.Units) unit.Boots = Model.Unit.MaxBoots;
            await MsgBox.ShowInfo(TopLevel.GetTopLevel(this) as Window ?? throw new InvalidOperationException(), "Done!");
        }

        private async void Btn1Boots_Click(object? sender, RoutedEventArgs e)
        {
            foreach (var unit in _chapterSave!.UnitRegion.Units) unit.Boots = 1;
            await MsgBox.ShowInfo(TopLevel.GetTopLevel(this) as Window ?? throw new InvalidOperationException(), "Done!");
        }

        private async void Btn0Boots_Click(object? sender, RoutedEventArgs e)
        {
            foreach (var unit in _chapterSave!.UnitRegion.Units) unit.Boots = 0;
            await MsgBox.ShowInfo(TopLevel.GetTopLevel(this) as Window ?? throw new InvalidOperationException(), "Done!");
        }

        private async void BtnMaxStats_Click(object? sender, RoutedEventArgs e)
        {
            foreach (var unit in _chapterSave!.UnitRegion.Units) unit.GainedStats = Utils.StatUtil.CalculateStatCaps(unit);
            await MsgBox.ShowInfo(TopLevel.GetTopLevel(this) as Window ?? throw new InvalidOperationException(), "Done!");
        }

        private async void BtnGiveDragonBlood_Click(object? sender, RoutedEventArgs e)
        {
            foreach (var unit in _chapterSave!.UnitRegion.Units) unit.Trait_CanUseDragonVein = true;
            await MsgBox.ShowInfo(TopLevel.GetTopLevel(this) as Window ?? throw new InvalidOperationException(), "Done!");
        }

        private async void BtnHearSealDlc_Click(object? sender, RoutedEventArgs e)
        {
            foreach (var unit in _chapterSave!.UnitRegion.Units) unit.DLCClassFlags = new byte[] { 0xFF, 0xFF, 0xFF };
            await MsgBox.ShowInfo(TopLevel.GetTopLevel(this) as Window ?? throw new InvalidOperationException(), "Done!");
        }

        private async void BtnResetSupports_Click(object? sender, RoutedEventArgs e)
        {
            var win = TopLevel.GetTopLevel(this) as Window ?? throw new InvalidOperationException();
            var result = await MsgBox.ShowYesNo(win,
                "This will reset all supports and the A+ rank choice." + Environment.NewLine +
                "Child units will not cease to exist, which could cause issues if you re-establish S supports (this is untested)." + Environment.NewLine +
                "Proceed?",
                "Reset supports?");
            if (!result) return;

            foreach (var unit in _chapterSave!.UnitRegion.Units)
            {
                unit.RawSupports = new byte[unit.RawNumberOfSupports];
                var characterData = Data.Database.Characters.GetByID(unit.CharacterID);
                if (characterData != null && characterData.IsChild)
                {
                    unit.FatherSupport = 0;
                    unit.MotherSupport = 0;
                    unit.SiblingSupport = 0;
                }
                unit.APlusSupportCharacter = Enums.Character.None;
            }
            await MsgBox.ShowInfo(win, "Done!");
        }

        private async void BtnStatBonuses_Click(object? sender, RoutedEventArgs e)
        {
            foreach (var unit in _chapterSave!.UnitRegion.Units)
            {
                unit.TonicBonusStats = new Model.Stat { HP = 5, Str = 2, Mag = 2, Skl = 2, Spd = 2, Lck = 4, Def = 2, Res = 2 };
                unit.StatusBonusStats = new Model.Stat { HP = 0, Str = 4, Mag = 4, Skl = 4, Spd = 4, Lck = 4, Def = 4, Res = 4 };
                unit.MealBonusStats = new Model.Stat { HP = 0, Str = 2, Mag = 2, Skl = 2, Spd = 2, Lck = 2, Def = 2, Res = 0 };
            }
            await MsgBox.ShowInfo(TopLevel.GetTopLevel(this) as Window ?? throw new InvalidOperationException(), "Done!");
        }

        private async void BtnMysteryCheat_Click(object? sender, RoutedEventArgs e)
        {
            Model.Cheats.MysteryCheat(_chapterSave!);
            // Reload unit viewer via MainWindow
            if (this.FindAncestorOfType<MainWindow>() is MainWindow mw) mw.LoadUnitViewer();
            await MsgBox.ShowInfo(TopLevel.GetTopLevel(this) as Window ?? throw new InvalidOperationException(), "Oh my! Do you have any idea what you just did?");
        }

        private async void BtnUnlockAllStatues_Click(object? sender, RoutedEventArgs e)
        {
            Model.Cheats.UnlockAllStatues(_chapterSave!);
            await MsgBox.ShowInfo(TopLevel.GetTopLevel(this) as Window ?? throw new InvalidOperationException(), "Done!");
        }
    }
}
