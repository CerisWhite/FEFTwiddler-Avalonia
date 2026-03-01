using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace FEFTwiddler.GUI.UnitViewer
{
    public partial class Skills : UserControl
    {
        private Model.Unit? _unit;
        private bool _loading;
        private bool _eventsBound;

        public Skills()
        {
            InitializeComponent();
        }

        public void LoadUnit(Model.Unit unit)
        {
            if (unit == null) return;
            _unit = unit;
            _loading = true;
            BindDataSources();
            ReadEquippedSkills();
            _loading = false;
            if (!_eventsBound) { BindEvents(); _eventsBound = true; }
        }

        private List<Data.Skill> GetSkillDataSource()
        {
            if (_unit == null) return new List<Data.Skill>();
            return Data.Database.Skills.GetAllLearnable()
                .Where(x => _unit.LearnedSkills.Contains(x.SkillID) || x.SkillID == Enums.Skill.None)
                .OrderBy(x => x.DisplayName)
                .ToList();
        }

        private void BindDataSources()
        {
            var src = GetSkillDataSource();
            cmbSkill1.ItemsSource = GetSkillDataSource();
            cmbSkill2.ItemsSource = GetSkillDataSource();
            cmbSkill3.ItemsSource = GetSkillDataSource();
            cmbSkill4.ItemsSource = GetSkillDataSource();
            cmbSkill5.ItemsSource = GetSkillDataSource();
        }

        private void ReadEquippedSkills()
        {
            SetSkillCombo(cmbSkill1, pictSkill1, _unit!.EquippedSkill_1);
            SetSkillCombo(cmbSkill2, pictSkill2, _unit.EquippedSkill_2);
            SetSkillCombo(cmbSkill3, pictSkill3, _unit.EquippedSkill_3);
            SetSkillCombo(cmbSkill4, pictSkill4, _unit.EquippedSkill_4);
            SetSkillCombo(cmbSkill5, pictSkill5, _unit.EquippedSkill_5);
        }

        private void SetSkillCombo(ComboBox cmb, Avalonia.Controls.Image img, Enums.Skill skillId)
        {
            cmb.SelectedItem = (cmb.ItemsSource as List<Data.Skill>)?.FirstOrDefault(s => s.SkillID == skillId)
                ?? (cmb.ItemsSource as List<Data.Skill>)?.FirstOrDefault(s => s.SkillID == Enums.Skill.None);
            img.Source = GetSkillImage(skillId);
        }

        private Bitmap? GetSkillImage(Enums.Skill skillId)
        {
            try
            {
                var uri = new System.Uri($"avares://FEFTwiddler/Resources/Images/SkillIcons/Skill_{(byte)skillId:000}.png");
                return new Bitmap(AssetLoader.Open(uri));
            }
            catch { return null; }
        }

        private void WriteEquippedSkills()
        {
            _unit!.EquippedSkill_1 = GetSkill(cmbSkill1);
            _unit.EquippedSkill_2 = GetSkill(cmbSkill2);
            _unit.EquippedSkill_3 = GetSkill(cmbSkill3);
            _unit.EquippedSkill_4 = GetSkill(cmbSkill4);
            _unit.EquippedSkill_5 = GetSkill(cmbSkill5);
            _unit.CollapseEquippedSkills();
        }

        private static Enums.Skill GetSkill(ComboBox cmb) =>
            cmb.SelectedItem is Data.Skill s ? s.SkillID : Enums.Skill.None;

        private void BindEvents()
        {
            void OnChanged(ComboBox cmb, Avalonia.Controls.Image img)
            {
                if (!_loading && _unit != null)
                {
                    WriteEquippedSkills();
                    var skill = GetSkill(cmb);
                    img.Source = GetSkillImage(skill);
                }
            }
            cmbSkill1.SelectionChanged += (_, _) => OnChanged(cmbSkill1, pictSkill1);
            cmbSkill2.SelectionChanged += (_, _) => OnChanged(cmbSkill2, pictSkill2);
            cmbSkill3.SelectionChanged += (_, _) => OnChanged(cmbSkill3, pictSkill3);
            cmbSkill4.SelectionChanged += (_, _) => OnChanged(cmbSkill4, pictSkill4);
            cmbSkill5.SelectionChanged += (_, _) => OnChanged(cmbSkill5, pictSkill5);
        }

        private async void BtnEditLearnedSkills_Click(object? sender, RoutedEventArgs e)
        {
            if (_unit == null) return;
            var win = TopLevel.GetTopLevel(this) as Window;
            if (win == null) return;
            var dlg = new LearnedSkillsViewer(_unit);
            await dlg.ShowDialog(win);
            _unit.UnequipUnlearnedSkills();
            _loading = true;
            BindDataSources();
            ReadEquippedSkills();
            _loading = false;
        }
    }
}
