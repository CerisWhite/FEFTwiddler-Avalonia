using Avalonia.Controls;

namespace FEFTwiddler.GUI.UnitViewer
{
    public partial class BattleData : UserControl
    {
        private Model.Unit? _unit;
        private bool _loading;

        public BattleData()
        {
            InitializeComponent();
        }

        public void LoadUnit(Model.Unit unit)
        {
            _unit = unit;
            _loading = true;
            numBattles.Value = _unit.BattleCount;
            numVictories.Value = _unit.VictoryCount;
            _loading = false;
            numBattles.ValueChanged += (_, _) => { if (!_loading && _unit != null) _unit.BattleCount = (ushort)(numBattles.Value ?? 0); };
            numVictories.ValueChanged += (_, _) => { if (!_loading && _unit != null) _unit.VictoryCount = (ushort)(numVictories.Value ?? 0); };
        }
    }
}
