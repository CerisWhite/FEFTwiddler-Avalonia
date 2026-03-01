using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace FEFTwiddler.GUI.UnitViewer
{
    public partial class Supports : Window
    {
        private readonly Model.Unit _unit;
        private readonly Model.IChapterSave _chapterSave;
        private readonly List<SupportPanel> _supportPanels = new();

        public Supports(Model.IChapterSave chapterSave, Model.Unit unit)
        {
            _chapterSave = chapterSave;
            _unit = unit;
            InitializeComponent();
            Title = unit.GetDisplayName() + "'s supports";
            Opened += (_, _) => Load();
        }

        private void Load()
        {
            stkSupports.Children.Clear();
            _supportPanels.Clear();

            var supportData = Data.Database.Characters.GetByID(_unit.CharacterID).SupportPool;
            byte supportCount = _unit.RawNumberOfSupports;

            for (int i = 0; i < supportCount; i++)
            {
                Model.Unit? partnerUnit = null;
                foreach (var u in _chapterSave.UnitRegion.Units)
                {
                    if (u.CharacterID == supportData[i].CharacterID && !u.IsRecruited)
                    { partnerUnit = u; break; }
                }

                int idx = i;
                var panel = new SupportPanel();
                panel.LoadMainSupport(_unit, partnerUnit, idx, onSChanged: RemoveAllSSupportExceptOne);
                _supportPanels.Add(panel);
                stkSupports.Children.Add(panel);
            }

            var charData = Data.Database.Characters.GetByID(_unit.CharacterID);
            if (charData.IsChild)
            {
                var fp = new SupportPanel(); fp.LoadFatherSupport(_unit); stkSupports.Children.Add(fp);
                var mp = new SupportPanel(); mp.LoadMotherSupport(_unit); stkSupports.Children.Add(mp);
                foreach (var u in _chapterSave.UnitRegion.Units)
                {
                    if (Data.Database.Characters.GetByID(u.CharacterID).IsChild && u.CharacterID != _unit.CharacterID &&
                        !u.IsRecruited && (u.FatherID == _unit.FatherID || u.MotherID == _unit.MotherID))
                    {
                        var sp = new SupportPanel(); sp.LoadSiblingSupport(_unit, u); stkSupports.Children.Add(sp);
                    }
                }
            }
            else
            {
                foreach (var u in _chapterSave.UnitRegion.Units)
                {
                    if (Data.Database.Characters.GetByID(u.CharacterID).IsChild && !u.IsRecruited &&
                        (u.FatherID == _unit.CharacterID || u.MotherID == _unit.CharacterID))
                    {
                        var cp = new SupportPanel(); cp.LoadChildSupport(_unit, u); stkSupports.Children.Add(cp);
                    }
                }
            }

            // A+ combo
            if (!charData.IsCorrin)
            {
                var aPlusIds = supportData
                    .Where(s => s.HasSSupport && !Data.Database.Characters.GetByID(s.CharacterID).IsCorrin)
                    .Select(s => s.CharacterID);
                var aPlusCandidates = Data.Database.Characters.GetAll()
                    .Where(c => aPlusIds.Any(id => id == c.CharacterID) || c.CharacterID == Enums.Character.None)
                    .OrderBy(c => c.DisplayName)
                    .ToList();
                cmbAPlus.ItemsSource = aPlusCandidates;
                cmbAPlus.SelectedItem = aPlusCandidates.FirstOrDefault(c => c.CharacterID == _unit.APlusSupportCharacter);
                cmbAPlus.IsEnabled = true;
                cmbAPlus.SelectionChanged += (_, _) =>
                {
                    if (cmbAPlus.SelectedItem is Data.Character c) _unit.APlusSupportCharacter = c.CharacterID;
                };
            }
            else
            {
                cmbAPlus.IsEnabled = false;
            }
        }

        public void RemoveAllSSupportExceptOne(int keepIndex)
        {
            if (chkPolygamy.IsChecked == true) return;
            for (int i = 0; i < _supportPanels.Count; i++)
            {
                if (i != keepIndex && _supportPanels[i].IsSSupport)
                    _supportPanels[i].MaxSupportWithConversation();
            }
        }

        private void BtnMaxSupports_Click(object? sender, RoutedEventArgs e)
        {
            foreach (var sp in _supportPanels) sp.MaxSupport();
        }

        private void BtnMaxSupportsConversation_Click(object? sender, RoutedEventArgs e)
        {
            foreach (var sp in _supportPanels) sp.MaxSupportWithConversation();
        }

        private void BtnClose_Click(object? sender, RoutedEventArgs e) => Close();
    }
}
