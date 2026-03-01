using Avalonia.Controls;
using Avalonia.Interactivity;

namespace FEFTwiddler.GUI.UnitViewer
{
    public partial class HexEditor : Window
    {
        private readonly Model.Unit _unit;

        public HexEditor(Model.Unit unit)
        {
            _unit = unit;
            InitializeComponent();
            Title = "Hex editing: " + unit.GetDisplayName();
            Opened += (_, _) => Load();
        }

        private void Load()
        {
            hexRawBlock1.SetBytes(_unit.RawBlock1);
            hexRawInventory.SetBytes(_unit.RawInventory);
            hexRawSupports.SetBytes(_unit.RawSupports);
            lblRawNumberOfSupports.Text = $"Count: {_unit.RawNumberOfSupports:X2}";
            hexRawBlock2.SetBytes(_unit.RawBlock2);
            hexRawLearnedSkills.SetBytes(_unit.RawLearnedSkills);
            hexRawDeployedUnitInfo.SetBytes(_unit.RawDeployedUnitInfo);
            hexRawBlock3.SetBytes(_unit.RawBlock3);
            lblRawEndBlockType.Text = $"Type: {_unit.RawEndBlockType:X2}";
            hexRawEndBlock.SetBytes(_unit.RawEndBlock);
        }

        private void BtnSave_Click(object? sender, RoutedEventArgs e)
        {
            _unit.RawBlock1 = hexRawBlock1.GetBytes();
            _unit.RawInventory = hexRawInventory.GetBytes();
            _unit.RawSupports = hexRawSupports.GetBytes();
            _unit.RawBlock2 = hexRawBlock2.GetBytes();
            _unit.RawLearnedSkills = hexRawLearnedSkills.GetBytes();
            _unit.RawDeployedUnitInfo = hexRawDeployedUnitInfo.GetBytes();
            _unit.RawBlock3 = hexRawBlock3.GetBytes();
            _unit.RawEndBlock = hexRawEndBlock.GetBytes();
            Close();
        }

        private void BtnCancel_Click(object? sender, RoutedEventArgs e) => Close();
    }
}
