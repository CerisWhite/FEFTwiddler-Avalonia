using System;
using System.Collections.Generic;
using Avalonia.Controls;

namespace FEFTwiddler.GUI.UnitViewer
{
    public partial class SupportPanel : UserControl
    {
        private Model.Unit? _unit;
        private Model.Unit? _partnerUnit;
        private Model.Unit? _siblingUnit;
        private Model.Unit? _childUnit;
        private int _supportIndex;
        private sbyte[]? _supportRange;
        private Action<int>? _onSChanged; // notifies Supports window of S-rank change

        private static readonly string[] TypeA = { "-", "C (conversation)", "C", "B (conversation)", "B", "A (conversation)", "A" };
        private static readonly string[] TypeS = { "-", "C (conversation)", "C", "B (conversation)", "B", "A (conversation)", "A", "S (conversation)", "S" };
        private static readonly sbyte[] FamilySupportRange = { 0, 0, 1, 4, 5, 9, 10 };

        public bool IsSSupport => cmbSupport.SelectedIndex == TypeS.Length - 1;

        public SupportPanel()
        {
            InitializeComponent();
        }

        public void LoadMainSupport(Model.Unit unit, Model.Unit? partnerUnit, int supportIndex, Action<int>? onSChanged = null)
        {
            _unit = unit; _partnerUnit = partnerUnit; _supportIndex = supportIndex; _onSChanged = onSChanged;
            PopulateMainSupport();
        }

        public void LoadFatherSupport(Model.Unit unit) { _unit = unit; PopulateFatherSupport(); }
        public void LoadMotherSupport(Model.Unit unit) { _unit = unit; PopulateMotherSupport(); }
        public void LoadSiblingSupport(Model.Unit unit, Model.Unit siblingUnit) { _unit = unit; _siblingUnit = siblingUnit; PopulateSiblingSupport(); }
        public void LoadChildSupport(Model.Unit unit, Model.Unit childUnit) { _unit = unit; _childUnit = childUnit; PopulateChildSupport(); }

        private void PopulateMainSupport()
        {
            var supportData = Data.Database.Characters.GetByID(_unit!.CharacterID).SupportPool;
            if (_supportIndex >= supportData.Length) return;

            lblName.Text = Data.Database.Characters.GetByID(supportData[_supportIndex].CharacterID).DisplayName;

            string[] types;
            if (supportData[_supportIndex].HasSSupport)
            {
                types = TypeA;
                _supportRange = new sbyte[] { 0,
                    (sbyte)(supportData[_supportIndex].C - 1), supportData[_supportIndex].C,
                    (sbyte)(supportData[_supportIndex].B - 1), supportData[_supportIndex].B,
                    (sbyte)(supportData[_supportIndex].A - 1), supportData[_supportIndex].A };
            }
            else
            {
                types = TypeS;
                _supportRange = new sbyte[] { 0,
                    (sbyte)(supportData[_supportIndex].C - 1), supportData[_supportIndex].C,
                    (sbyte)(supportData[_supportIndex].B - 1), supportData[_supportIndex].B,
                    (sbyte)(supportData[_supportIndex].A - 1), supportData[_supportIndex].A,
                    (sbyte)(supportData[_supportIndex].S - 1), supportData[_supportIndex].S };
            }

            cmbSupport.ItemsSource = types;
            sbyte sp = (sbyte)_unit.RawSupports[_supportIndex];
            int i = 0;
            while (i < _supportRange.Length - 1 && sp >= _supportRange[i + 1]) i++;
            cmbSupport.SelectedIndex = i;

            cmbSupport.SelectionChanged += (_, _) => WriteSupportMain();
        }

        private void WriteSupportMain()
        {
            if (_unit == null || _supportRange == null) return;
            if (IsSSupport) _onSChanged?.Invoke(_supportIndex);

            var raw = _unit.RawSupports;
            raw[_supportIndex] = (byte)_supportRange[cmbSupport.SelectedIndex];
            _unit.RawSupports = raw;

            if (_partnerUnit != null)
            {
                var supportData = Data.Database.Characters.GetByID(_unit.CharacterID).SupportPool;
                var partnerData = Data.Database.Characters.GetByID(supportData[_supportIndex].CharacterID);
                int i = 0;
                while (i < partnerData.SupportPool.Length && partnerData.SupportPool[i].CharacterID != _unit.CharacterID) i++;
                var pRaw = _partnerUnit.RawSupports;
                if (i < pRaw.Length) { pRaw[i] = (byte)_supportRange[cmbSupport.SelectedIndex]; _partnerUnit.RawSupports = pRaw; }
            }
        }

        private void PopulateFatherSupport()
        {
            lblName.Text = _unit!.CorrinName ?? Data.Database.Characters.GetByID(_unit.FatherID).DisplayName;
            cmbSupport.ItemsSource = TypeA;
            int i = 1;
            while (i < FamilySupportRange.Length - 1 && _unit.FatherSupport >= FamilySupportRange[i + 1]) i++;
            cmbSupport.SelectedIndex = i;
            cmbSupport.SelectionChanged += (_, _) => { if (_unit != null) _unit.FatherSupport = FamilySupportRange[cmbSupport.SelectedIndex]; };
        }

        private void PopulateMotherSupport()
        {
            lblName.Text = _unit!.CorrinName ?? Data.Database.Characters.GetByID(_unit.MotherID).DisplayName;
            cmbSupport.ItemsSource = TypeA;
            int i = 1;
            while (i < FamilySupportRange.Length - 1 && _unit.MotherSupport >= FamilySupportRange[i + 1]) i++;
            cmbSupport.SelectedIndex = i;
            cmbSupport.SelectionChanged += (_, _) => { if (_unit != null) _unit.MotherSupport = FamilySupportRange[cmbSupport.SelectedIndex]; };
        }

        private void PopulateSiblingSupport()
        {
            lblName.Text = Data.Database.Characters.GetByID(_siblingUnit!.CharacterID).DisplayName;
            cmbSupport.ItemsSource = TypeA;
            int i = 1;
            while (i < FamilySupportRange.Length - 1 && _unit!.SiblingSupport >= FamilySupportRange[i + 1]) i++;
            cmbSupport.SelectedIndex = i;
            cmbSupport.SelectionChanged += (_, _) => { if (_unit != null && _siblingUnit != null) _unit.SiblingSupport = _siblingUnit.SiblingSupport = FamilySupportRange[cmbSupport.SelectedIndex]; };
        }

        private void PopulateChildSupport()
        {
            lblName.Text = Data.Database.Characters.GetByID(_childUnit!.CharacterID).DisplayName;
            cmbSupport.ItemsSource = TypeA;
            int i = 1;
            if (_childUnit.FatherID == _unit!.CharacterID)
                while (i < FamilySupportRange.Length - 1 && _childUnit.FatherSupport >= FamilySupportRange[i + 1]) i++;
            else
                while (i < FamilySupportRange.Length - 1 && _childUnit.MotherSupport >= FamilySupportRange[i + 1]) i++;
            cmbSupport.SelectedIndex = i;
            cmbSupport.SelectionChanged += (_, _) =>
            {
                if (_unit == null || _childUnit == null) return;
                var val = FamilySupportRange[cmbSupport.SelectedIndex];
                if (_unit.CharacterID == _childUnit.FatherID) _childUnit.FatherSupport = val;
                else _childUnit.MotherSupport = val;
            };
        }

        public void MaxSupport() => cmbSupport.SelectedIndex = (cmbSupport.Items?.Count ?? 1) - 1;
        public void MaxSupportWithConversation() => cmbSupport.SelectedIndex = Math.Max(0, (cmbSupport.Items?.Count ?? 2) - 2);
    }
}
