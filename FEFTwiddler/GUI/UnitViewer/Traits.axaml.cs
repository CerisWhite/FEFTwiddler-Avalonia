using Avalonia.Controls;
using Avalonia.Interactivity;

namespace FEFTwiddler.GUI.UnitViewer
{
    public partial class Traits : Window
    {
        private readonly Model.Unit _unit;

        public Traits(Model.Unit unit)
        {
            _unit = unit;
            InitializeComponent();
            Title = unit.GetDisplayName() + "'s traits";
            Opened += (_, _) => { PopulateControls(); BindEvents(); };
        }

        private void PopulateControls()
        {
            chkTrait_00_01.IsChecked = _unit.Trait_IsFemale;
            chkTrait_00_02.IsChecked = _unit.Trait_Hero;
            chkTrait_00_04.IsChecked = _unit.Trait_Player;
            chkTrait_00_08.IsChecked = _unit.Trait_AdvancedClasses;
            chkTrait_00_10.IsChecked = _unit.Trait_Leader;
            chkTrait_00_20.IsChecked = _unit.Trait_DefeatCondition;
            chkTrait_00_40.IsChecked = _unit.Trait_MovementBan;
            chkTrait_00_80.IsChecked = _unit.Trait_HitBan;
            chkTrait_01_01.IsChecked = _unit.Trait_CriticalBan;
            chkTrait_01_02.IsChecked = _unit.Trait_AvoidBan;
            chkTrait_01_04.IsChecked = _unit.Trait_ForceHit;
            chkTrait_01_08.IsChecked = _unit.Trait_ForceCritical;
            chkTrait_01_10.IsChecked = _unit.Trait_ForceAvoid;
            chkTrait_01_20.IsChecked = _unit.Trait_ForceDodge;
            chkTrait_01_40.IsChecked = _unit.Trait_ResistStatus;
            chkTrait_01_80.IsChecked = _unit.Trait_ImmuneStatus;
            chkTrait_02_01.IsChecked = _unit.Trait_NegateLethality;
            chkTrait_02_02.IsChecked = _unit.Trait_02_02;
            chkTrait_02_04.IsChecked = _unit.Trait_02_04;
            chkTrait_02_08.IsChecked = _unit.Trait_DoubleExpWhenDefeated;
            chkTrait_02_10.IsChecked = _unit.Trait_HalfExpWhenDefeated;
            chkTrait_02_20.IsChecked = _unit.Trait_RareFacelessExp;
            chkTrait_02_40.IsChecked = _unit.Trait_ExpCorrection;
            chkTrait_02_80.IsChecked = _unit.Trait_IsManakete;
            chkTrait_03_01.IsChecked = _unit.Trait_IsBeast;
            chkTrait_03_02.IsChecked = _unit.Trait_Sing;
            chkTrait_03_04.IsChecked = _unit.Trait_DestroysVillages;
            chkTrait_03_08.IsChecked = _unit.Trait_EnemyOnly;
            chkTrait_03_10.IsChecked = _unit.Trait_03_10;
            chkTrait_03_20.IsChecked = _unit.Trait_03_20;
            chkTrait_03_40.IsChecked = _unit.Trait_Takumi;
            chkTrait_03_80.IsChecked = _unit.Trait_Ryoma;
            chkTrait_04_01.IsChecked = _unit.Trait_Leo;
            chkTrait_04_02.IsChecked = _unit.Trait_Xander;
            chkTrait_04_04.IsChecked = _unit.Trait_CannotUseSpecialWeapon;
            chkTrait_04_08.IsChecked = _unit.Trait_CanUseDragonVein;
            chkTrait_04_10.IsChecked = _unit.Trait_CannotUseAttackStance;
            chkTrait_04_20.IsChecked = _unit.Trait_CannotDoubleAttack;
            chkTrait_04_40.IsChecked = _unit.Trait_CannotBeInherited;
            chkTrait_04_80.IsChecked = _unit.Trait_CannotBeObtainedViaSupport;
            chkTrait_05_01.IsChecked = _unit.Trait_RouteLimited;
            chkTrait_05_02.IsChecked = _unit.Trait_05_02;
            chkTrait_05_04.IsChecked = _unit.Trait_CanUseStaff;
            chkTrait_05_08.IsChecked = _unit.Trait_CannotBeTraded;
            chkTrait_05_10.IsChecked = _unit.Trait_CannotObtainExp;
            chkTrait_05_20.IsChecked = _unit.Trait_CannotWarp;
            chkTrait_05_40.IsChecked = _unit.Trait_SalespersonInMyCastle;
            chkTrait_05_80.IsChecked = _unit.Trait_DefeatConditionWithdrawal;
            chkTrait_06_01.IsChecked = _unit.Trait_Ophelia;
            chkTrait_06_02.IsChecked = _unit.Trait_CannotTriggerOffensiveSkills;
            chkTrait_06_04.IsChecked = _unit.Trait_TriggerOffensiveSkills;
            chkTrait_06_08.IsChecked = _unit.Trait_Ties;
            chkTrait_06_10.IsChecked = _unit.Trait_CapturedUnit;
            chkTrait_06_20.IsChecked = _unit.Trait_AvoidMinus10;
            chkTrait_06_40.IsChecked = _unit.Trait_AvoidMinus20;
            chkTrait_06_80.IsChecked = _unit.Trait_AvoidPlus10;
            chkTrait_07_01.IsChecked = _unit.Trait_AvoidPlus20;
            chkTrait_07_02.IsChecked = _unit.Trait_HitPlus10;
            chkTrait_07_04.IsChecked = _unit.Trait_HitPlus20;
            chkTrait_07_08.IsChecked = _unit.Trait_HitPlus30;
            chkTrait_07_10.IsChecked = _unit.Trait_07_10;
            chkTrait_07_20.IsChecked = _unit.Trait_CannotBePromoted;
            chkTrait_07_40.IsChecked = _unit.Trait_IsAmiibo;
            chkTrait_07_80.IsChecked = _unit.Trait_07_80;
        }

        private void BindEvents()
        {
            chkTrait_00_01.IsCheckedChanged += (_, _) => _unit.Trait_IsFemale = chkTrait_00_01.IsChecked == true;
            chkTrait_00_02.IsCheckedChanged += (_, _) => _unit.Trait_Hero = chkTrait_00_02.IsChecked == true;
            chkTrait_00_04.IsCheckedChanged += (_, _) => _unit.Trait_Player = chkTrait_00_04.IsChecked == true;
            chkTrait_00_08.IsCheckedChanged += (_, _) => _unit.Trait_AdvancedClasses = chkTrait_00_08.IsChecked == true;
            chkTrait_00_10.IsCheckedChanged += (_, _) => _unit.Trait_Leader = chkTrait_00_10.IsChecked == true;
            chkTrait_00_20.IsCheckedChanged += (_, _) => _unit.Trait_DefeatCondition = chkTrait_00_20.IsChecked == true;
            chkTrait_00_40.IsCheckedChanged += (_, _) => _unit.Trait_MovementBan = chkTrait_00_40.IsChecked == true;
            chkTrait_00_80.IsCheckedChanged += (_, _) => _unit.Trait_HitBan = chkTrait_00_80.IsChecked == true;
            chkTrait_01_01.IsCheckedChanged += (_, _) => _unit.Trait_CriticalBan = chkTrait_01_01.IsChecked == true;
            chkTrait_01_02.IsCheckedChanged += (_, _) => _unit.Trait_AvoidBan = chkTrait_01_02.IsChecked == true;
            chkTrait_01_04.IsCheckedChanged += (_, _) => _unit.Trait_ForceHit = chkTrait_01_04.IsChecked == true;
            chkTrait_01_08.IsCheckedChanged += (_, _) => _unit.Trait_ForceCritical = chkTrait_01_08.IsChecked == true;
            chkTrait_01_10.IsCheckedChanged += (_, _) => _unit.Trait_ForceAvoid = chkTrait_01_10.IsChecked == true;
            chkTrait_01_20.IsCheckedChanged += (_, _) => _unit.Trait_ForceDodge = chkTrait_01_20.IsChecked == true;
            chkTrait_01_40.IsCheckedChanged += (_, _) => _unit.Trait_ResistStatus = chkTrait_01_40.IsChecked == true;
            chkTrait_01_80.IsCheckedChanged += (_, _) => _unit.Trait_ImmuneStatus = chkTrait_01_80.IsChecked == true;
            chkTrait_02_01.IsCheckedChanged += (_, _) => _unit.Trait_NegateLethality = chkTrait_02_01.IsChecked == true;
            chkTrait_02_02.IsCheckedChanged += (_, _) => _unit.Trait_02_02 = chkTrait_02_02.IsChecked == true;
            chkTrait_02_04.IsCheckedChanged += (_, _) => _unit.Trait_02_04 = chkTrait_02_04.IsChecked == true;
            chkTrait_02_08.IsCheckedChanged += (_, _) => _unit.Trait_DoubleExpWhenDefeated = chkTrait_02_08.IsChecked == true;
            chkTrait_02_10.IsCheckedChanged += (_, _) => _unit.Trait_HalfExpWhenDefeated = chkTrait_02_10.IsChecked == true;
            chkTrait_02_20.IsCheckedChanged += (_, _) => _unit.Trait_RareFacelessExp = chkTrait_02_20.IsChecked == true;
            chkTrait_02_40.IsCheckedChanged += (_, _) => _unit.Trait_ExpCorrection = chkTrait_02_40.IsChecked == true;
            chkTrait_02_80.IsCheckedChanged += (_, _) => _unit.Trait_IsManakete = chkTrait_02_80.IsChecked == true;
            chkTrait_03_01.IsCheckedChanged += (_, _) => _unit.Trait_IsBeast = chkTrait_03_01.IsChecked == true;
            chkTrait_03_02.IsCheckedChanged += (_, _) => _unit.Trait_Sing = chkTrait_03_02.IsChecked == true;
            chkTrait_03_04.IsCheckedChanged += (_, _) => _unit.Trait_DestroysVillages = chkTrait_03_04.IsChecked == true;
            chkTrait_03_08.IsCheckedChanged += (_, _) => _unit.Trait_EnemyOnly = chkTrait_03_08.IsChecked == true;
            chkTrait_03_10.IsCheckedChanged += (_, _) => _unit.Trait_03_10 = chkTrait_03_10.IsChecked == true;
            chkTrait_03_20.IsCheckedChanged += (_, _) => _unit.Trait_03_20 = chkTrait_03_20.IsChecked == true;
            chkTrait_03_40.IsCheckedChanged += (_, _) => _unit.Trait_Takumi = chkTrait_03_40.IsChecked == true;
            chkTrait_03_80.IsCheckedChanged += (_, _) => _unit.Trait_Ryoma = chkTrait_03_80.IsChecked == true;
            chkTrait_04_01.IsCheckedChanged += (_, _) => _unit.Trait_Leo = chkTrait_04_01.IsChecked == true;
            chkTrait_04_02.IsCheckedChanged += (_, _) => _unit.Trait_Xander = chkTrait_04_02.IsChecked == true;
            chkTrait_04_04.IsCheckedChanged += (_, _) => _unit.Trait_CannotUseSpecialWeapon = chkTrait_04_04.IsChecked == true;
            chkTrait_04_08.IsCheckedChanged += (_, _) => _unit.Trait_CanUseDragonVein = chkTrait_04_08.IsChecked == true;
            chkTrait_04_10.IsCheckedChanged += (_, _) => _unit.Trait_CannotUseAttackStance = chkTrait_04_10.IsChecked == true;
            chkTrait_04_20.IsCheckedChanged += (_, _) => _unit.Trait_CannotDoubleAttack = chkTrait_04_20.IsChecked == true;
            chkTrait_04_40.IsCheckedChanged += (_, _) => _unit.Trait_CannotBeInherited = chkTrait_04_40.IsChecked == true;
            chkTrait_04_80.IsCheckedChanged += (_, _) => _unit.Trait_CannotBeObtainedViaSupport = chkTrait_04_80.IsChecked == true;
            chkTrait_05_01.IsCheckedChanged += (_, _) => _unit.Trait_RouteLimited = chkTrait_05_01.IsChecked == true;
            chkTrait_05_02.IsCheckedChanged += (_, _) => _unit.Trait_05_02 = chkTrait_05_02.IsChecked == true;
            chkTrait_05_04.IsCheckedChanged += (_, _) => _unit.Trait_CanUseStaff = chkTrait_05_04.IsChecked == true;
            chkTrait_05_08.IsCheckedChanged += (_, _) => _unit.Trait_CannotBeTraded = chkTrait_05_08.IsChecked == true;
            chkTrait_05_10.IsCheckedChanged += (_, _) => _unit.Trait_CannotObtainExp = chkTrait_05_10.IsChecked == true;
            chkTrait_05_20.IsCheckedChanged += (_, _) => _unit.Trait_CannotWarp = chkTrait_05_20.IsChecked == true;
            chkTrait_05_40.IsCheckedChanged += (_, _) => _unit.Trait_SalespersonInMyCastle = chkTrait_05_40.IsChecked == true;
            chkTrait_05_80.IsCheckedChanged += (_, _) => _unit.Trait_DefeatConditionWithdrawal = chkTrait_05_80.IsChecked == true;
            chkTrait_06_01.IsCheckedChanged += (_, _) => _unit.Trait_Ophelia = chkTrait_06_01.IsChecked == true;
            chkTrait_06_02.IsCheckedChanged += (_, _) => _unit.Trait_CannotTriggerOffensiveSkills = chkTrait_06_02.IsChecked == true;
            chkTrait_06_04.IsCheckedChanged += (_, _) => _unit.Trait_TriggerOffensiveSkills = chkTrait_06_04.IsChecked == true;
            chkTrait_06_08.IsCheckedChanged += (_, _) => _unit.Trait_Ties = chkTrait_06_08.IsChecked == true;
            chkTrait_06_10.IsCheckedChanged += (_, _) => _unit.Trait_CapturedUnit = chkTrait_06_10.IsChecked == true;
            chkTrait_06_20.IsCheckedChanged += (_, _) => _unit.Trait_AvoidMinus10 = chkTrait_06_20.IsChecked == true;
            chkTrait_06_40.IsCheckedChanged += (_, _) => _unit.Trait_AvoidMinus20 = chkTrait_06_40.IsChecked == true;
            chkTrait_06_80.IsCheckedChanged += (_, _) => _unit.Trait_AvoidPlus10 = chkTrait_06_80.IsChecked == true;
            chkTrait_07_01.IsCheckedChanged += (_, _) => _unit.Trait_AvoidPlus20 = chkTrait_07_01.IsChecked == true;
            chkTrait_07_02.IsCheckedChanged += (_, _) => _unit.Trait_HitPlus10 = chkTrait_07_02.IsChecked == true;
            chkTrait_07_04.IsCheckedChanged += (_, _) => _unit.Trait_HitPlus20 = chkTrait_07_04.IsChecked == true;
            chkTrait_07_08.IsCheckedChanged += (_, _) => _unit.Trait_HitPlus30 = chkTrait_07_08.IsChecked == true;
            chkTrait_07_10.IsCheckedChanged += (_, _) => _unit.Trait_07_10 = chkTrait_07_10.IsChecked == true;
            chkTrait_07_20.IsCheckedChanged += (_, _) => _unit.Trait_CannotBePromoted = chkTrait_07_20.IsChecked == true;
            chkTrait_07_40.IsCheckedChanged += (_, _) => _unit.Trait_IsAmiibo = chkTrait_07_40.IsChecked == true;
            chkTrait_07_80.IsCheckedChanged += (_, _) => _unit.Trait_07_80 = chkTrait_07_80.IsChecked == true;
        }

        private void BtnClose_Click(object? sender, RoutedEventArgs e) => Close();
    }
}
