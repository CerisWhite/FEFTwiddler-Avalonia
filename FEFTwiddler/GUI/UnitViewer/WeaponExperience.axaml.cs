using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace FEFTwiddler.GUI.UnitViewer
{
    public partial class WeaponExperience : UserControl
    {
        private Model.Unit? _unit;
        private bool _loading;
        private bool _eventsBound;

        public WeaponExperience()
        {
            InitializeComponent();
        }

        public void LoadUnit(Model.Unit unit)
        {
            _unit = unit;
            _loading = true;
            PopulateControls();
            _loading = false;
            if (!_eventsBound) { BindEvents(); _eventsBound = true; }
        }

        private static Bitmap? LoadWeaponIcon(string name)
        {
            try { return new Bitmap(AssetLoader.Open(new System.Uri($"avares://FEFTwiddler/Resources/Images/WeaponExpIcons/WeaponExp_{name}.png"))); }
            catch { return null; }
        }

        private void PopulateControls()
        {
            imgSword.Source = LoadWeaponIcon("sword");
            imgLance.Source = LoadWeaponIcon("lance");
            imgAxe.Source = LoadWeaponIcon("axe");
            imgShuriken.Source = LoadWeaponIcon("shuriken");
            imgBow.Source = LoadWeaponIcon("bow");
            imgTome.Source = LoadWeaponIcon("tome");
            imgStaff.Source = LoadWeaponIcon("staff");
            imgStone.Source = LoadWeaponIcon("stone");

            numSword.Value = Model.Unit.FixWeaponExperience(_unit!.WeaponExperience_Sword);
            numLance.Value = Model.Unit.FixWeaponExperience(_unit.WeaponExperience_Lance);
            numAxe.Value = Model.Unit.FixWeaponExperience(_unit.WeaponExperience_Axe);
            numShuriken.Value = Model.Unit.FixWeaponExperience(_unit.WeaponExperience_Shuriken);
            numBow.Value = Model.Unit.FixWeaponExperience(_unit.WeaponExperience_Bow);
            numTome.Value = Model.Unit.FixWeaponExperience(_unit.WeaponExperience_Tome);
            numStaff.Value = Model.Unit.FixWeaponExperience(_unit.WeaponExperience_Staff);

            var cd = Data.Database.Characters.GetByID(_unit.CharacterID);
            if (cd != null && cd.CanUseStones)
            {
                numStone.Value = Model.Unit.FixWeaponExperience(_unit.WeaponExperience_Stone);
                numStone.IsEnabled = true;
            }
            else
            {
                numStone.Value = Model.Unit.MinWeaponExperience;
                numStone.IsEnabled = false;
            }

            UpdateRankLabel(lblSwordRank, (decimal)(numSword.Value ?? 0));
            UpdateRankLabel(lblLanceRank, (decimal)(numLance.Value ?? 0));
            UpdateRankLabel(lblAxeRank, (decimal)(numAxe.Value ?? 0));
            UpdateRankLabel(lblShurikenRank, (decimal)(numShuriken.Value ?? 0));
            UpdateRankLabel(lblBowRank, (decimal)(numBow.Value ?? 0));
            UpdateRankLabel(lblTomeRank, (decimal)(numTome.Value ?? 0));
            UpdateRankLabel(lblStaffRank, (decimal)(numStaff.Value ?? 0));
            UpdateRankLabel(lblStoneRank, (decimal)(numStone.Value ?? 0));
        }

        private void BindEvents()
        {
            numSword.ValueChanged += (_, _) => { if (!_loading && _unit != null) { _unit.WeaponExperience_Sword = (byte)(numSword.Value ?? 0); UpdateRankLabel(lblSwordRank, (decimal)(numSword.Value ?? 0)); } };
            numLance.ValueChanged += (_, _) => { if (!_loading && _unit != null) { _unit.WeaponExperience_Lance = (byte)(numLance.Value ?? 0); UpdateRankLabel(lblLanceRank, (decimal)(numLance.Value ?? 0)); } };
            numAxe.ValueChanged += (_, _) => { if (!_loading && _unit != null) { _unit.WeaponExperience_Axe = (byte)(numAxe.Value ?? 0); UpdateRankLabel(lblAxeRank, (decimal)(numAxe.Value ?? 0)); } };
            numShuriken.ValueChanged += (_, _) => { if (!_loading && _unit != null) { _unit.WeaponExperience_Shuriken = (byte)(numShuriken.Value ?? 0); UpdateRankLabel(lblShurikenRank, (decimal)(numShuriken.Value ?? 0)); } };
            numBow.ValueChanged += (_, _) => { if (!_loading && _unit != null) { _unit.WeaponExperience_Bow = (byte)(numBow.Value ?? 0); UpdateRankLabel(lblBowRank, (decimal)(numBow.Value ?? 0)); } };
            numTome.ValueChanged += (_, _) => { if (!_loading && _unit != null) { _unit.WeaponExperience_Tome = (byte)(numTome.Value ?? 0); UpdateRankLabel(lblTomeRank, (decimal)(numTome.Value ?? 0)); } };
            numStaff.ValueChanged += (_, _) => { if (!_loading && _unit != null) { _unit.WeaponExperience_Staff = (byte)(numStaff.Value ?? 0); UpdateRankLabel(lblStaffRank, (decimal)(numStaff.Value ?? 0)); } };
            numStone.ValueChanged += (_, _) => { if (!_loading && _unit != null) { _unit.WeaponExperience_Stone = (byte)(numStone.Value ?? 0); UpdateRankLabel(lblStoneRank, (decimal)(numStone.Value ?? 0)); } };
        }

        private static void UpdateRankLabel(TextBlock lbl, decimal val)
        {
            if (val >= 251) { lbl.Text = "S"; lbl.Foreground = Brushes.Green; }
            else if (val >= 161) { lbl.Text = "A"; lbl.Foreground = Brushes.Black; }
            else if (val >= 96) { lbl.Text = "B"; lbl.Foreground = Brushes.Black; }
            else if (val >= 51) { lbl.Text = "C"; lbl.Foreground = Brushes.Black; }
            else if (val >= 21) { lbl.Text = "D"; lbl.Foreground = Brushes.Black; }
            else { lbl.Text = "E"; lbl.Foreground = Brushes.Black; }
        }
    }
}
