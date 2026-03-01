using Avalonia.Controls;

namespace FEFTwiddler.GUI.UnitViewer
{
    public partial class DragonVein : UserControl
    {
        private Model.Unit? _unit;
        private bool _loading;

        public DragonVein()
        {
            InitializeComponent();
        }

        public void LoadUnit(Model.Unit unit)
        {
            _unit = unit;
            _loading = true;
            var characterData = Data.Database.Characters.GetByID(_unit.CharacterID);
            if (characterData != null)
            {
                var isDefault = characterData.CanUseDragonVein;
                chkDragonVein.IsChecked = _unit.Trait_CanUseDragonVein || isDefault;
                chkDragonVein.IsEnabled = !isDefault;
            }
            else
            {
                chkDragonVein.IsChecked = _unit.Trait_CanUseDragonVein;
            }
            _loading = false;
            chkDragonVein.IsCheckedChanged += (_, _) =>
            {
                if (_loading || _unit == null) return;
                var cd = Data.Database.Characters.GetByID(_unit.CharacterID);
                if (chkDragonVein.IsChecked == true && cd != null && !cd.CanUseDragonVein)
                    _unit.Trait_CanUseDragonVein = true;
                else
                    _unit.Trait_CanUseDragonVein = false;
            };
        }
    }
}
