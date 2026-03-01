using Avalonia.Controls;

namespace FEFTwiddler.GUI.ChapterData
{
    public partial class Difficulty : UserControl
    {
        private Model.IChapterSave? _chapterSave;
        private bool _loading;

        public Difficulty()
        {
            InitializeComponent();
        }

        public void LoadChapterSave(Model.IChapterSave chapterSave)
        {
            _chapterSave = chapterSave;
            _loading = true;
            PopulateControls();
            _loading = false;
            BindEvents();
        }

        private void PopulateControls()
        {
            switch (_chapterSave!.UserRegion.Difficulty)
            {
                case Enums.Difficulty.Normal: rdoNormal.IsChecked = true; break;
                case Enums.Difficulty.Hard: rdoHard.IsChecked = true; break;
                case Enums.Difficulty.Lunatic: rdoLunatic.IsChecked = true; break;
            }

            if (_chapterSave.UserRegion.UnitsReviveAfterTurn) rdoPhoenix.IsChecked = true;
            else if (_chapterSave.UserRegion.UnitsReviveAfterChapter) rdoCasual.IsChecked = true;
            else rdoClassic.IsChecked = true;
        }

        private void BindEvents()
        {
            rdoNormal.IsCheckedChanged += (_, _) => { if (rdoNormal.IsChecked == true && !_loading) SetDifficulty(Enums.Difficulty.Normal); };
            rdoHard.IsCheckedChanged += (_, _) => { if (rdoHard.IsChecked == true && !_loading) SetDifficulty(Enums.Difficulty.Hard); };
            rdoLunatic.IsCheckedChanged += (_, _) => { if (rdoLunatic.IsChecked == true && !_loading) SetDifficulty(Enums.Difficulty.Lunatic); };

            rdoPhoenix.IsCheckedChanged += (_, _) => { if (rdoPhoenix.IsChecked == true && !_loading) SetPhoenix(); };
            rdoCasual.IsCheckedChanged += (_, _) => { if (rdoCasual.IsChecked == true && !_loading) SetCasual(); };
            rdoClassic.IsCheckedChanged += (_, _) => { if (rdoClassic.IsChecked == true && !_loading) SetClassic(); };
        }

        private void SetDifficulty(Enums.Difficulty d)
        {
            _chapterSave!.UserRegion.Difficulty = d;
            _chapterSave.Header.Difficulty = d;
        }

        private void SetPhoenix()
        {
            _chapterSave!.UserRegion.UnitsReviveAfterTurn = true;
            _chapterSave.UserRegion.UnitsReviveAfterChapter = true;
            _chapterSave.Header.UnitsReviveAfterTurn = true;
            _chapterSave.Header.UnitsReviveAfterChapter = true;
        }

        private void SetCasual()
        {
            _chapterSave!.UserRegion.UnitsReviveAfterTurn = false;
            _chapterSave.UserRegion.UnitsReviveAfterChapter = true;
            _chapterSave.Header.UnitsReviveAfterTurn = false;
            _chapterSave.Header.UnitsReviveAfterChapter = true;
        }

        private void SetClassic()
        {
            _chapterSave!.UserRegion.UnitsReviveAfterTurn = false;
            _chapterSave.UserRegion.UnitsReviveAfterChapter = false;
            _chapterSave.Header.UnitsReviveAfterTurn = false;
            _chapterSave.Header.UnitsReviveAfterChapter = false;
        }
    }
}
