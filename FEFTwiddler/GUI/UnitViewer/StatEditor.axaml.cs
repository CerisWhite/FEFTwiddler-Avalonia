using Avalonia.Controls;
using Avalonia.Interactivity;

namespace FEFTwiddler.GUI.UnitViewer
{
    public partial class StatEditor : Window
    {
        private readonly Model.Unit _unit;
        private Model.Stat _caps;
        public bool IsStatsChanged { get; private set; }

        public StatEditor(Model.Unit unit)
        {
            _unit = unit;
            InitializeComponent();
            Title = unit.GetDisplayName() + "'s stats";
            Opened += (_, _) => Load();
        }

        private void Load()
        {
            var statData = Data.Database.Stats;
            lblHP.Text = statData.GetByID(Enums.Stat.HP).DisplayName;
            lblStr.Text = statData.GetByID(Enums.Stat.Strength).DisplayName;
            lblMag.Text = statData.GetByID(Enums.Stat.Magic).DisplayName;
            lblSkl.Text = statData.GetByID(Enums.Stat.Skill).DisplayName;
            lblSpd.Text = statData.GetByID(Enums.Stat.Speed).DisplayName;
            lblLck.Text = statData.GetByID(Enums.Stat.Luck).DisplayName;
            lblDef.Text = statData.GetByID(Enums.Stat.Defense).DisplayName;
            lblRes.Text = statData.GetByID(Enums.Stat.Resistance).DisplayName;

            _caps = Utils.StatUtil.CalculateStatCaps(_unit);
            var stats = Utils.StatUtil.CalculateStats(_unit);

            numHP.Maximum = _caps.HP; numStr.Maximum = _caps.Str; numMag.Maximum = _caps.Mag;
            numSkl.Maximum = _caps.Skl; numSpd.Maximum = _caps.Spd; numLck.Maximum = _caps.Lck;
            numDef.Maximum = _caps.Def; numRes.Maximum = _caps.Res;

            numHP.Value = stats.HP; numStr.Value = stats.Str; numMag.Value = stats.Mag;
            numSkl.Value = stats.Skl; numSpd.Value = stats.Spd; numLck.Value = stats.Lck;
            numDef.Value = stats.Def; numRes.Value = stats.Res;

            var tonic = _unit.TonicBonusStats;
            chkHPTonic.IsChecked = tonic.HP == 5; chkStrTonic.IsChecked = tonic.Str == 2;
            chkMagTonic.IsChecked = tonic.Mag == 2; chkSklTonic.IsChecked = tonic.Skl == 2;
            chkSpdTonic.IsChecked = tonic.Spd == 2; chkLckTonic.IsChecked = tonic.Lck == 4;
            chkDefTonic.IsChecked = tonic.Def == 2; chkResTonic.IsChecked = tonic.Res == 2;

            var status = _unit.StatusBonusStats;
            chkStrStatus.IsChecked = status.Str == 4; chkMagStatus.IsChecked = status.Mag == 4;
            chkSklStatus.IsChecked = status.Skl == 4; chkSpdStatus.IsChecked = status.Spd == 4;
            chkLckStatus.IsChecked = status.Lck == 4; chkDefStatus.IsChecked = status.Def == 4;
            chkResStatus.IsChecked = status.Res == 4;

            var meal = _unit.MealBonusStats;
            chkStrMeal.IsChecked = meal.Str == 2; chkMagMeal.IsChecked = meal.Mag == 2;
            chkSklMeal.IsChecked = meal.Skl == 2; chkSpdMeal.IsChecked = meal.Spd == 2;
            chkLckMeal.IsChecked = meal.Lck == 2; chkDefMeal.IsChecked = meal.Def == 2;
            chkResMeal.IsChecked = meal.Res == 2;
        }

        private void BtnMax_Click(object? sender, RoutedEventArgs e)
        {
            numHP.Value = _caps.HP; numStr.Value = _caps.Str; numMag.Value = _caps.Mag;
            numSkl.Value = _caps.Skl; numSpd.Value = _caps.Spd; numLck.Value = _caps.Lck;
            numDef.Value = _caps.Def; numRes.Value = _caps.Res;
        }

        private void BtnAllBonuses_Click(object? sender, RoutedEventArgs e)
        {
            chkHPTonic.IsChecked = chkStrTonic.IsChecked = chkMagTonic.IsChecked = chkSklTonic.IsChecked =
            chkSpdTonic.IsChecked = chkLckTonic.IsChecked = chkDefTonic.IsChecked = chkResTonic.IsChecked = true;
            chkStrStatus.IsChecked = chkMagStatus.IsChecked = chkSklStatus.IsChecked = chkSpdStatus.IsChecked =
            chkLckStatus.IsChecked = chkDefStatus.IsChecked = chkResStatus.IsChecked = true;
            chkStrMeal.IsChecked = chkMagMeal.IsChecked = chkSklMeal.IsChecked = chkSpdMeal.IsChecked =
            chkLckMeal.IsChecked = chkDefMeal.IsChecked = chkResMeal.IsChecked = true;
        }

        private void BtnSave_Click(object? sender, RoutedEventArgs e)
        {
            var baseStats = Utils.StatUtil.CalculateBaseStats(_unit);
            var changes = new Model.Stat
            {
                HP = (sbyte)(numHP.Value ?? 0), Str = (sbyte)(numStr.Value ?? 0),
                Mag = (sbyte)(numMag.Value ?? 0), Skl = (sbyte)(numSkl.Value ?? 0),
                Spd = (sbyte)(numSpd.Value ?? 0), Lck = (sbyte)(numLck.Value ?? 0),
                Def = (sbyte)(numDef.Value ?? 0), Res = (sbyte)(numRes.Value ?? 0)
            } - baseStats;
            var finalStats = _unit.GainedStats;

            if (changes.HP != 0) { finalStats.HP = changes.HP; IsStatsChanged = true; }
            if (changes.Str != 0) { finalStats.Str = changes.Str; IsStatsChanged = true; }
            if (changes.Mag != 0) { finalStats.Mag = changes.Mag; IsStatsChanged = true; }
            if (changes.Skl != 0) { finalStats.Skl = changes.Skl; IsStatsChanged = true; }
            if (changes.Spd != 0) { finalStats.Spd = changes.Spd; IsStatsChanged = true; }
            if (changes.Lck != 0) { finalStats.Lck = changes.Lck; IsStatsChanged = true; }
            if (changes.Def != 0) { finalStats.Def = changes.Def; IsStatsChanged = true; }
            if (changes.Res != 0) { finalStats.Res = changes.Res; IsStatsChanged = true; }
            _unit.GainedStats = finalStats;

            _unit.TonicBonusStats = new Model.Stat
            {
                HP = chkHPTonic.IsChecked == true ? (sbyte)5 : (sbyte)0,
                Str = chkStrTonic.IsChecked == true ? (sbyte)2 : (sbyte)0,
                Mag = chkMagTonic.IsChecked == true ? (sbyte)2 : (sbyte)0,
                Skl = chkSklTonic.IsChecked == true ? (sbyte)2 : (sbyte)0,
                Spd = chkSpdTonic.IsChecked == true ? (sbyte)2 : (sbyte)0,
                Lck = chkLckTonic.IsChecked == true ? (sbyte)4 : (sbyte)0,
                Def = chkDefTonic.IsChecked == true ? (sbyte)2 : (sbyte)0,
                Res = chkResTonic.IsChecked == true ? (sbyte)2 : (sbyte)0
            };

            _unit.StatusBonusStats = new Model.Stat
            {
                HP = 0,
                Str = chkStrStatus.IsChecked == true ? (sbyte)4 : (sbyte)0,
                Mag = chkMagStatus.IsChecked == true ? (sbyte)4 : (sbyte)0,
                Skl = chkSklStatus.IsChecked == true ? (sbyte)4 : (sbyte)0,
                Spd = chkSpdStatus.IsChecked == true ? (sbyte)4 : (sbyte)0,
                Lck = chkLckStatus.IsChecked == true ? (sbyte)4 : (sbyte)0,
                Def = chkDefStatus.IsChecked == true ? (sbyte)4 : (sbyte)0,
                Res = chkResStatus.IsChecked == true ? (sbyte)4 : (sbyte)0
            };

            var oldMeal = _unit.MealBonusStats;
            _unit.MealBonusStats = new Model.Stat
            {
                HP = 0,
                Str = GetMeal(chkStrMeal, oldMeal.Str), Mag = GetMeal(chkMagMeal, oldMeal.Mag),
                Skl = GetMeal(chkSklMeal, oldMeal.Skl), Spd = GetMeal(chkSpdMeal, oldMeal.Spd),
                Lck = GetMeal(chkLckMeal, oldMeal.Lck), Def = GetMeal(chkDefMeal, oldMeal.Def),
                Res = GetMeal(chkResMeal, oldMeal.Res)
            };

            Close();
        }

        private static sbyte GetMeal(CheckBox chk, sbyte oldVal) =>
            chk.IsChecked == true ? (sbyte)2 : chk.IsChecked == false ? (sbyte)0 : oldVal;

        private void BtnCancel_Click(object? sender, RoutedEventArgs e) => Close();
    }
}
