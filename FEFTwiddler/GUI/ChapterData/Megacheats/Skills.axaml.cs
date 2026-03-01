using Avalonia.Controls;
using Avalonia.Interactivity;

namespace FEFTwiddler.GUI.ChapterData.Megacheats
{
    public partial class Skills : UserControl
    {
        private Model.IChapterSave? _chapterSave;

        public Skills()
        {
            InitializeComponent();
            BindEvents();
        }

        public void LoadChapterSave(Model.IChapterSave chapterSave)
        {
            _chapterSave = chapterSave;
        }

        private void BindEvents()
        {
            btnLearnNormalClassSkills.Click += (_, _) => { foreach (var u in _chapterSave!.UnitRegion.Units) u.LearnNormalClassSkills(); };
            btnUnlearnNormalClassSkills.Click += (_, _) => { foreach (var u in _chapterSave!.UnitRegion.Units) u.UnlearnNormalClassSkills(); };
            btnLearnCorrinOnlySkills.Click += (_, _) => { foreach (var u in _chapterSave!.UnitRegion.Units) u.LearnCorrinOnlySkills(); };
            btnUnlearnCorrinOnlySkills.Click += (_, _) => { foreach (var u in _chapterSave!.UnitRegion.Units) u.UnlearnCorrinOnlySkills(); };
            btnLearnAzuraOnlySkills.Click += (_, _) => { foreach (var u in _chapterSave!.UnitRegion.Units) u.LearnAzuraOnlySkills(); };
            btnUnlearnAzuraOnlySkills.Click += (_, _) => { foreach (var u in _chapterSave!.UnitRegion.Units) u.UnlearnAzuraOnlySkills(); };
            btnLearnBeastOnlySkills.Click += (_, _) => { foreach (var u in _chapterSave!.UnitRegion.Units) u.LearnBeastOnlySkills(); };
            btnUnlearnBeastOnlySkills.Click += (_, _) => { foreach (var u in _chapterSave!.UnitRegion.Units) u.UnlearnBeastOnlySkills(); };
            btnLearnKitsuneOnlySkills.Click += (_, _) => { foreach (var u in _chapterSave!.UnitRegion.Units) u.LearnKitsuneOnlySkills(); };
            btnUnlearnKitsuneOnlySkills.Click += (_, _) => { foreach (var u in _chapterSave!.UnitRegion.Units) u.UnlearnKitsuneOnlySkills(); };
            btnLearnWolfskinOnlySkills.Click += (_, _) => { foreach (var u in _chapterSave!.UnitRegion.Units) u.LearnWolfskinOnlySkills(); };
            btnUnlearnWolfskinOnlySkills.Click += (_, _) => { foreach (var u in _chapterSave!.UnitRegion.Units) u.UnlearnWolfskinOnlySkills(); };
            btnLearnVillagerOnlySkills.Click += (_, _) => { foreach (var u in _chapterSave!.UnitRegion.Units) u.LearnVillagerOnlySkills(); };
            btnUnlearnVillagerOnlySkills.Click += (_, _) => { foreach (var u in _chapterSave!.UnitRegion.Units) u.UnlearnVillagerOnlySkills(); };
            btnLearnPathBonusClassSkills.Click += (_, _) => { foreach (var u in _chapterSave!.UnitRegion.Units) u.LearnPathBonusClassSkills(); };
            btnUnlearnPathBonusClassSkills.Click += (_, _) => { foreach (var u in _chapterSave!.UnitRegion.Units) u.UnlearnPathBonusClassSkills(); };
            btnLearnDlcClassSkills.Click += (_, _) => { foreach (var u in _chapterSave!.UnitRegion.Units) u.LearnDlcClassSkills(); };
            btnUnlearnDlcClassSkills.Click += (_, _) => { foreach (var u in _chapterSave!.UnitRegion.Units) u.UnlearnDlcClassSkills(); };
            btnLearnAmiiboClassSkills.Click += (_, _) => { foreach (var u in _chapterSave!.UnitRegion.Units) u.LearnAmiiboClassSkills(); };
            btnUnlearnAmiiboClassSkills.Click += (_, _) => { foreach (var u in _chapterSave!.UnitRegion.Units) u.UnlearnAmiiboClassSkills(); };
            btnLearnEnemyAndNpcSkills.Click += (_, _) => { foreach (var u in _chapterSave!.UnitRegion.Units) u.LearnEnemyAndNpcSkills(); };
            btnUnlearnEnemyAndNpcSkills.Click += (_, _) => { foreach (var u in _chapterSave!.UnitRegion.Units) u.UnlearnEnemyAndNpcSkills(); };
            btnLearnAllSkills.Click += (_, _) => { foreach (var u in _chapterSave!.UnitRegion.Units) u.LearnAllSkills(); };
            btnUnlearnAllSkills.Click += (_, _) => { foreach (var u in _chapterSave!.UnitRegion.Units) u.UnlearnAllSkills(); };
        }
    }
}
