using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;

namespace FEFTwiddler.GUI.UnitViewer
{
    public partial class ClassAndLevel : UserControl
    {
        private Model.Unit? _unit;
        private bool _loading;
        private bool _eventsBound;

        public ClassAndLevel()
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

        private void PopulateControls()
        {
            var classes = Data.Database.Classes.GetAll().OrderBy(x => x.DisplayName).ToList();
            cmbClass.ItemsSource = classes;
            cmbClass.SelectedItem = classes.FirstOrDefault(c => c.ClassID == _unit!.ClassID);

            numEternalSeals.Maximum = Model.Unit.MaxEternalSealsUsed;
            numEternalSeals.Value = _unit!.FixEternalSealsUsed();
            numLevel.Maximum = _unit.GetTheoreticalMaxLevel();
            numLevel.Value = _unit.FixLevel();
            numInternalLevel.Value = _unit.InternalLevel;
            numExperience.Value = _unit.Experience;
            numBoots.Value = Model.Unit.FixBoots(_unit.Boots);

            var maxLevel = _unit.GetModifiedMaxLevel();
            numExperience.IsEnabled = _unit.Level < maxLevel;
        }

        private void BindEvents()
        {
            cmbClass.SelectionChanged += (_, _) =>
            {
                if (_loading || _unit == null) return;
                if (cmbClass.SelectedItem is Data.Class cls)
                {
                    _unit.ClassID = cls.ClassID;
                    if (_unit.ClassID == Enums.Class.PegasusKnight) _unit.HeartSeal_PegasusKnight = true;
                    var maxLevel = _unit.GetModifiedMaxLevel();
                    if (_unit.Level > maxLevel)
                    {
                        numLevel.Value = maxLevel;
                        _unit.Level = maxLevel;
                    }
                }
            };

            numLevel.ValueChanged += (_, _) =>
            {
                if (_loading || _unit == null) return;
                _unit.Level = (byte)(numLevel.Value ?? 1);
                var minSeals = _unit.GetMinimumEternalSealsForCurrentLevel();
                if (_unit.EternalSealsUsed < minSeals) { numEternalSeals.Value = minSeals; _unit.EternalSealsUsed = minSeals; }
                var maxLevel = _unit.GetModifiedMaxLevel();
                numExperience.IsEnabled = _unit.Level < maxLevel;
                if (_unit.Level >= maxLevel) { numExperience.Value = 0; _unit.Experience = 0; }
            };

            numInternalLevel.ValueChanged += (_, _) => { if (!_loading && _unit != null) _unit.InternalLevel = (byte)(numInternalLevel.Value ?? 0); };

            numEternalSeals.ValueChanged += (_, _) =>
            {
                if (_loading || _unit == null) return;
                _unit.EternalSealsUsed = (byte)(numEternalSeals.Value ?? 0);
                var maxLevel = _unit.GetModifiedMaxLevel();
                if (_unit.Level > maxLevel) { numLevel.Value = maxLevel; _unit.Level = maxLevel; }
                numExperience.IsEnabled = _unit.Level < maxLevel;
                if (_unit.Level >= maxLevel) { numExperience.Value = 0; _unit.Experience = 0; }
            };

            numExperience.ValueChanged += (_, _) => { if (!_loading && _unit != null) _unit.Experience = (byte)(numExperience.Value ?? 0); };
            numBoots.ValueChanged += (_, _) => { if (!_loading && _unit != null) _unit.Boots = (byte)(numBoots.Value ?? 0); };
        }
    }
}
