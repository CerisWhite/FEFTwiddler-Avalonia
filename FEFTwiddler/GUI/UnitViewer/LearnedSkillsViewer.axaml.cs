using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace FEFTwiddler.GUI.UnitViewer
{
    public partial class LearnedSkillsViewer : Window
    {
        private readonly Model.Unit _unit;
        private readonly Model.LearnedSkills _tempLearnedSkills;
        private readonly Dictionary<string, List<SkillIcon>> _groups = new();

        public LearnedSkillsViewer(Model.Unit unit)
        {
            _unit = unit;
            _tempLearnedSkills = new Model.LearnedSkills(_unit.LearnedSkills.Raw.ToArray());
            InitializeComponent();
            Title = "Learned Skills for " + unit.GetDisplayName();
            Opened += (_, _) => Load();
        }

        private void Load()
        {
            var allSkills = Data.Database.Skills.GetAllLearnable()
                .Where(x => x.SkillID != Enums.Skill.None)
                .OrderBy(x => x.DisplayName);

            AddSkillGroup(flwNormalClassSkills, "Normal", allSkills.Where(x => x.IsNormalClassSkill));
            AddSkillGroup(flwCorrinOnlySkills, "Corrin", allSkills.Where(x => x.IsCorrinOnlySkill));
            AddSkillGroup(flwAzuraOnlySkills, "Azura", allSkills.Where(x => x.IsAzuraOnlySkill));
            AddSkillGroup(flwBeastOnlySkills, "Beast", allSkills.Where(x => x.IsKitsuneOnlySkill && x.IsWolfskinOnlySkill));
            AddSkillGroup(flwKitsuneOnlySkills, "Kitsune", allSkills.Where(x => x.IsKitsuneOnlySkill && !x.IsWolfskinOnlySkill));
            AddSkillGroup(flwWolfskinOnlySkills, "Wolfskin", allSkills.Where(x => x.IsWolfskinOnlySkill && !x.IsKitsuneOnlySkill));
            AddSkillGroup(flwVillagerOnlySkills, "Villager", allSkills.Where(x => x.IsVillagerOnlySkill));
            AddSkillGroup(flwPathBonusClassSkills, "PathBonus", allSkills.Where(x => x.IsPathBonusClassSkill));
            AddSkillGroup(flwDlcClassSkills, "Dlc", allSkills.Where(x => x.IsDlcClassSkill && !x.IsAmiiboClassSkill));
            AddSkillGroup(flwAmiiboClassSkills, "Amiibo", allSkills.Where(x => x.IsAmiiboClassSkill));
            AddSkillGroup(flwEnemyAndNpcSkills, "EnemyNpc", allSkills.Where(x => x.IsEnemyAndNpcSkill));
        }

        private void AddSkillGroup(WrapPanel panel, string key, IEnumerable<Data.Skill> skills)
        {
            var icons = new List<SkillIcon>();
            foreach (var skill in skills)
            {
                var icon = new SkillIcon(_tempLearnedSkills, skill);
                icons.Add(icon);
                panel.Children.Add(icon);
            }
            _groups[key] = icons;
        }

        private void BtnLearnSkillGroup_Click(object? sender, RoutedEventArgs e)
        {
            var tag = ((Button)sender!).Tag?.ToString() ?? "";
            if (_groups.TryGetValue(tag, out var icons)) foreach (var icon in icons) icon.Enable();
        }

        private void BtnUnlearnSkillGroup_Click(object? sender, RoutedEventArgs e)
        {
            var tag = ((Button)sender!).Tag?.ToString() ?? "";
            if (_groups.TryGetValue(tag, out var icons)) foreach (var icon in icons) icon.Disable();
        }

        private void BtnSave_Click(object? sender, RoutedEventArgs e)
        {
            _unit.LearnedSkills = _tempLearnedSkills;
            Close();
        }

        private void BtnCancel_Click(object? sender, RoutedEventArgs e) => Close();
    }

    public class SkillIcon : Avalonia.Controls.Image
    {
        private readonly Model.LearnedSkills _tempLearnedSkills;
        private readonly Data.Skill _skill;
        private bool _enabled;

        private static readonly Uri DefaultUri = new("avares://FEFTwiddler/Resources/Images/Skills/Skill_000.png");

        public SkillIcon(Model.LearnedSkills tempLearnedSkills, Data.Skill skill)
        {
            _tempLearnedSkills = tempLearnedSkills;
            _skill = skill;
            Width = 24; Height = 24;
            Cursor = new Cursor(StandardCursorType.Hand);
            PointerPressed += (_, _) => Toggle();

            Source = LoadImage();
            _enabled = !_tempLearnedSkills.Contains(skill.SkillID);
            Toggle(); // set to correct initial state
        }

        private Bitmap? LoadImage()
        {
            try
            {
                var uri = new Uri($"avares://FEFTwiddler/Resources/Images/Skills/Skill_{(byte)_skill.SkillID:000}.png");
                return new Bitmap(AssetLoader.Open(uri));
            }
            catch { return null; }
        }

        public void Toggle() { if (_enabled) Disable(); else Enable(); }

        public void Enable()
        {
            _enabled = true;
            _tempLearnedSkills.Add(_skill.SkillID);
            Opacity = 1.0;
        }

        public void Disable()
        {
            _enabled = false;
            _tempLearnedSkills.Remove(_skill.SkillID);
            Opacity = 0.3;
        }
    }
}
