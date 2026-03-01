using System.Linq;
using Avalonia.Controls;

namespace FEFTwiddler.GUI.UnitViewer
{
    public partial class Accessories : UserControl
    {
        private Model.Unit? _unit;
        private bool _loading;
        private bool _eventsBound;

        public Accessories()
        {
            InitializeComponent();
        }

        public void LoadUnit(Model.Unit unit)
        {
            _unit = unit;
            _loading = true;
            BindDataSources();
            cmbHeadwear.SelectedItem = (cmbHeadwear.ItemsSource as System.Collections.Generic.List<Data.Accessory>)?.FirstOrDefault(a => a.AccessoryID == _unit.Headwear);
            cmbFacewear.SelectedItem = (cmbFacewear.ItemsSource as System.Collections.Generic.List<Data.Accessory>)?.FirstOrDefault(a => a.AccessoryID == _unit.Facewear);
            cmbArmwear.SelectedItem = (cmbArmwear.ItemsSource as System.Collections.Generic.List<Data.Accessory>)?.FirstOrDefault(a => a.AccessoryID == _unit.Armwear);
            cmbUnderwear.SelectedItem = (cmbUnderwear.ItemsSource as System.Collections.Generic.List<Data.Accessory>)?.FirstOrDefault(a => a.AccessoryID == _unit.Underwear);
            _loading = false;
            if (!_eventsBound) { BindEvents(); _eventsBound = true; }
        }

        private void BindDataSources()
        {
            var all = Data.Database.Accessories.GetAll();
            cmbHeadwear.ItemsSource = all.Where(x => x.Type == Enums.AccessoryType.Headwear || x.Type == Enums.AccessoryType.None).OrderBy(x => x.DisplayName).ToList();
            cmbFacewear.ItemsSource = all.Where(x => x.Type == Enums.AccessoryType.Facewear || x.Type == Enums.AccessoryType.None).OrderBy(x => x.DisplayName).ToList();
            cmbArmwear.ItemsSource = all.Where(x => x.Type == Enums.AccessoryType.Armwear || x.Type == Enums.AccessoryType.None).OrderBy(x => x.DisplayName).ToList();
            cmbUnderwear.ItemsSource = all.Where(x => x.Type == Enums.AccessoryType.Underwear || x.Type == Enums.AccessoryType.None).OrderBy(x => x.DisplayName).ToList();
        }

        private void BindEvents()
        {
            cmbHeadwear.SelectionChanged += (_, _) => { if (!_loading && _unit != null && cmbHeadwear.SelectedItem is Data.Accessory a) _unit.Headwear = a.AccessoryID; };
            cmbFacewear.SelectionChanged += (_, _) => { if (!_loading && _unit != null && cmbFacewear.SelectedItem is Data.Accessory a) _unit.Facewear = a.AccessoryID; };
            cmbArmwear.SelectionChanged += (_, _) => { if (!_loading && _unit != null && cmbArmwear.SelectedItem is Data.Accessory a) _unit.Armwear = a.AccessoryID; };
            cmbUnderwear.SelectionChanged += (_, _) => { if (!_loading && _unit != null && cmbUnderwear.SelectedItem is Data.Accessory a) _unit.Underwear = a.AccessoryID; };
        }
    }
}
