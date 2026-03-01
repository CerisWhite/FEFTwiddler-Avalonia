using Avalonia.Controls;
using Avalonia.Interactivity;

namespace FEFTwiddler.GUI.UnitViewer
{
    public partial class Stats : UserControl
    {
        private Model.Unit? _unit;

        public Stats()
        {
            InitializeComponent();
        }

        public void LoadUnit(Model.Unit unit)
        {
            _unit = unit;
            PopulateControls();
        }

        private void PopulateControls()
        {
            var stats = Utils.StatUtil.CalculateStats(_unit!);
            if (stats == null)
            {
                lblStats.Text = "Stats (raw):";
                txtStatBytes.Text = _unit!.GainedStats.ToString();
                btnStats.IsEnabled = false;
            }
            else
            {
                lblStats.Text = "Stats:";
                txtStatBytes.Text = stats.ToString();
                btnStats.IsEnabled = true;
            }
        }

        private async void BtnStats_Click(object? sender, RoutedEventArgs e)
        {
            var win = TopLevel.GetTopLevel(this) as Window;
            if (win == null || _unit == null) return;
            var dlg = new StatEditor(_unit);
            await dlg.ShowDialog(win);
            if (dlg.IsStatsChanged) PopulateControls();
        }
    }
}
