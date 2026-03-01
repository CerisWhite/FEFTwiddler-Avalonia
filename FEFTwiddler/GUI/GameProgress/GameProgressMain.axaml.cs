using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace FEFTwiddler.GUI.GameProgress
{
    public partial class GameProgressMain : UserControl
    {
        private Model.IChapterSave? _chapterSave;
        private Model.NewGamePlus.TimeMachine? _timeMachine;

        public GameProgressMain()
        {
            InitializeComponent();
        }

        public void LoadChapterSave(Model.IChapterSave chapterSave)
        {
            _chapterSave = chapterSave;
            _timeMachine = new Model.NewGamePlus.TimeMachine(_chapterSave);
            PopulateHistoryPanel();
            UpdateAvailableBattlefields();
        }

        private void PopulateHistoryPanel()
        {
            flwChaptersCompleted.Children.Clear();
            foreach (var entry in _chapterSave!.UserRegion.ChapterHistory)
            {
                var panel = new ChapterHistoryPanel(entry,
                    canUnplay: _timeMachine!.CanUnplayChapter(entry.ChapterID),
                    onUnplay: () => { _timeMachine!.UnplayChapter(entry.ChapterID); UpdateAvailableBattlefields(); });
                flwChaptersCompleted.Children.Add(panel);
            }
        }

        private void UpdateAvailableBattlefields()
        {
            flwBattlefieldsAvailable.Children.Clear();
            foreach (var bf in _chapterSave!.BattlefieldRegion.Battlefields
                .Where(x => x.BattlefieldStatus == Enums.BattlefieldStatus.Available))
            {
                var lbl = new TextBlock { Text = Data.Database.Chapters.GetByID(bf.ChapterID).DisplayName };
                flwBattlefieldsAvailable.Children.Add(lbl);
            }
        }

        private async void BtnUnlockAmiiboChapters_Click(object? sender, RoutedEventArgs e)
        {
            _timeMachine!.UnlockChapter(Enums.Chapter.HeroBattle_Marth);
            _timeMachine.UnlockChapter(Enums.Chapter.HeroBattle_Ike);
            _timeMachine.UnlockChapter(Enums.Chapter.HeroBattle_Lucina);
            _timeMachine.UnlockChapter(Enums.Chapter.HeroBattle_Robin);
            UpdateAvailableBattlefields();
            await MsgBox.ShowInfo(TopLevel.GetTopLevel(this) as Window ?? throw new System.InvalidOperationException(), "Done!");
        }
    }
}
