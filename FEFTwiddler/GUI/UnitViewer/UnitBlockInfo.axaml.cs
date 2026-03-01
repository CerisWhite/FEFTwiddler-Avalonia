using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.VisualTree;

namespace FEFTwiddler.GUI.UnitViewer
{
    public partial class UnitBlockInfo : UserControl
    {
        private Model.IChapterSave? _chapterSave;
        private Model.Unit? _unit;
        private bool _loading;
        private bool _eventsBound;

        public UnitBlockInfo()
        {
            InitializeComponent();
        }

        public void LoadUnit(Model.IChapterSave chapterSave, Model.Unit unit)
        {
            _chapterSave = chapterSave;
            _unit = unit;
            _loading = true;
            cmbDeathChapter.ItemsSource = Enum.GetValues(typeof(Enums.Chapter));
            PopulateControls();
            _loading = false;
            if (!_eventsBound) { BindEvents(); _eventsBound = true; }
        }

        private void PopulateControls()
        {
            switch (_unit!.UnitBlock)
            {
                case Enums.UnitBlock.Living: rdoLiving.IsChecked = true; break;
                case Enums.UnitBlock.Deployed: rdoDeployed.IsChecked = true; break;
                case Enums.UnitBlock.Absent: rdoAbsent.IsChecked = true; break;
                case Enums.UnitBlock.DeadByGameplay: rdoDeadByGameplay.IsChecked = true; break;
                case Enums.UnitBlock.DeadByPlot: rdoDeadByPlot.IsChecked = true; break;
            }
            chkChallenge.IsChecked = _unit.DiedOnChallengeMission;
            cmbDeathChapter.SelectedItem = _unit.DeathChapter;

            bool isCorrin = Enum.IsDefined(typeof(Enums.Character), _unit.CharacterID) &&
                Data.Database.Characters.GetByID(_unit.CharacterID).IsCorrin &&
                !_unit.IsEinherjar;
            bool isMapSave = _chapterSave!.GetSaveFileType() == Enums.SaveFileType.Map;

            if (isCorrin || isMapSave)
            {
                DisableAll();
            }
            else
            {
                EnableAll();
                if (!IsDead()) DisableDeathChapter();
            }
        }

        private void BindEvents()
        {
            rdoLiving.IsCheckedChanged += (_, _) => { if (rdoLiving.IsChecked == true && !_loading) HandleSelectLiving(); };
            rdoDeployed.IsCheckedChanged += (_, _) => { if (rdoDeployed.IsChecked == true && !_loading) HandleSelectDeployed(); };
            rdoAbsent.IsCheckedChanged += (_, _) => { if (rdoAbsent.IsChecked == true && !_loading) HandleSelectAbsent(); };
            rdoDeadByGameplay.IsCheckedChanged += (_, _) => { if (rdoDeadByGameplay.IsChecked == true && !_loading) HandleSelectDeadByGameplay(); };
            rdoDeadByPlot.IsCheckedChanged += (_, _) => { if (rdoDeadByPlot.IsChecked == true && !_loading) HandleSelectDeadByPlot(); };
            cmbDeathChapter.SelectionChanged += (_, _) => { if (!_loading && _unit != null && cmbDeathChapter.SelectedItem is Enums.Chapter ch) _unit.DeathChapter = ch; };
            chkChallenge.IsCheckedChanged += (_, _) => { if (!_loading && _unit != null) _unit.DiedOnChallengeMission = chkChallenge.IsChecked == true; };
        }

        private bool IsDead() => _unit!.UnitBlock == Enums.UnitBlock.DeadByGameplay || _unit.UnitBlock == Enums.UnitBlock.DeadByPlot;

        private void DisableAll()
        {
            rdoLiving.IsEnabled = rdoDeployed.IsEnabled = rdoAbsent.IsEnabled =
            rdoDeadByGameplay.IsEnabled = rdoDeadByPlot.IsEnabled =
            chkChallenge.IsEnabled = cmbDeathChapter.IsEnabled = false;
        }

        private void EnableAll()
        {
            rdoLiving.IsEnabled = true;
            rdoDeployed.IsEnabled = _chapterSave!.Header.IsBattlePrepSave;
            rdoAbsent.IsEnabled = rdoDeadByGameplay.IsEnabled = rdoDeadByPlot.IsEnabled = true;
            chkChallenge.IsEnabled = cmbDeathChapter.IsEnabled = true;
        }

        private void DisableDeathChapter() { cmbDeathChapter.IsEnabled = false; chkChallenge.IsEnabled = false; }
        private void EnableDeathChapter() { cmbDeathChapter.IsEnabled = true; chkChallenge.IsEnabled = true; }

        private void HandleSelectLiving()
        {
            if (_unit!.UnitBlock == Enums.UnitBlock.Living) return;
            _unit.UnitBlock = Enums.UnitBlock.Living;
            _unit.RawDeployedUnitInfo = Model.Unit.GetEmptyDeployedInfoBlock();
            _unit.IsDead = false; _unit.WasKilledByPlot = false;
            _unit.DeathChapter = Enums.Chapter.None; _unit.DiedOnChallengeMission = false;
            DisableDeathChapter();
            this.FindAncestorOfType<MainWindow>()?.LoadUnitViewer(_unit);
        }

        private void HandleSelectDeployed()
        {
            if (_unit!.UnitBlock == Enums.UnitBlock.Deployed) return;
            _unit.UnitBlock = Enums.UnitBlock.Deployed;
            _unit.RawDeployedUnitInfo = Model.Unit.GetFullDeployedInfoBlock();
            _unit.IsDead = false; _unit.WasKilledByPlot = false;
            _unit.DeathChapter = Enums.Chapter.None; _unit.DiedOnChallengeMission = false;
            DisableDeathChapter();
            this.FindAncestorOfType<MainWindow>()?.LoadUnitViewer(_unit);
        }

        private void HandleSelectAbsent()
        {
            if (_unit!.UnitBlock == Enums.UnitBlock.Absent) return;
            _unit.UnitBlock = Enums.UnitBlock.Absent;
            _unit.RawDeployedUnitInfo = Model.Unit.GetEmptyDeployedInfoBlock();
            _unit.IsDead = false; _unit.WasKilledByPlot = false;
            _unit.DeathChapter = Enums.Chapter.None; _unit.DiedOnChallengeMission = false;
            DisableDeathChapter();
            this.FindAncestorOfType<MainWindow>()?.LoadUnitViewer(_unit);
        }

        private void HandleSelectDeadByGameplay()
        {
            if (_unit!.UnitBlock == Enums.UnitBlock.DeadByGameplay) return;
            _unit.UnitBlock = Enums.UnitBlock.DeadByGameplay;
            _unit.RawDeployedUnitInfo = Model.Unit.GetEmptyDeployedInfoBlock();
            _unit.IsDead = true; _unit.WasKilledByPlot = false;
            EnableDeathChapter();
            this.FindAncestorOfType<MainWindow>()?.LoadUnitViewer(_unit);
        }

        private void HandleSelectDeadByPlot()
        {
            if (_unit!.UnitBlock == Enums.UnitBlock.DeadByPlot) return;
            _unit.UnitBlock = Enums.UnitBlock.DeadByPlot;
            _unit.RawDeployedUnitInfo = Model.Unit.GetEmptyDeployedInfoBlock();
            _unit.IsDead = true; _unit.WasKilledByPlot = true;
            EnableDeathChapter();
            this.FindAncestorOfType<MainWindow>()?.LoadUnitViewer(_unit);
        }
    }
}
