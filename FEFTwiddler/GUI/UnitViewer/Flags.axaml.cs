using Avalonia.Controls;

namespace FEFTwiddler.GUI.UnitViewer
{
    public partial class Flags : UserControl
    {
        private Model.Unit? _unit;
        private bool _loading;

        public Flags()
        {
            InitializeComponent();
        }

        public void LoadUnit(Model.Unit unit)
        {
            _unit = unit;
            _loading = true;
            chkEinherjar.IsChecked = _unit.IsEinherjar;
            chkRecruited.IsChecked = _unit.IsRecruited;
            _loading = false;
            chkEinherjar.IsCheckedChanged += (_, _) => { if (!_loading && _unit != null) _unit.IsEinherjar = chkEinherjar.IsChecked == true; };
            chkRecruited.IsCheckedChanged += (_, _) => { if (!_loading && _unit != null) _unit.IsRecruited = chkRecruited.IsChecked == true; };
        }
    }
}
