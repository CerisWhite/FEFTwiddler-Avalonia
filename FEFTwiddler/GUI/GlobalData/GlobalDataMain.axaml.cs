using Avalonia.Controls;
using Avalonia.Interactivity;

namespace FEFTwiddler.GUI.GlobalData
{
    public partial class GlobalDataMain : UserControl
    {
        private Model.GlobalSave? _globalSave;

        public GlobalDataMain()
        {
            InitializeComponent();
        }

        public void LoadGlobalSave(Model.GlobalSave globalSave)
        {
            _globalSave = globalSave;
        }

        private async void BtnUnlockSupportLog_Click(object? sender, RoutedEventArgs e)
        {
            Model.Cheats.UnlockSupportLog(_globalSave!);
            await MsgBox.ShowInfo(TopLevel.GetTopLevel(this) as Window ?? throw new System.InvalidOperationException(), "Done!");
        }

        private async void BtnHairColors_Click(object? sender, RoutedEventArgs e)
        {
            var win = TopLevel.GetTopLevel(this) as Window;
            if (win == null || _globalSave == null) return;
            var popup = new HairColors(_globalSave);
            await popup.ShowDialog(win);
        }
    }
}
