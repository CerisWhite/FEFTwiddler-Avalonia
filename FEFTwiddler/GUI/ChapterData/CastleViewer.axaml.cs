using System;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace FEFTwiddler.GUI.ChapterData
{
    public partial class CastleViewer : Window
    {
        private readonly Model.ChapterSaveRegions.MyCastleRegion _castleRegion;
        private readonly CastleMap _castleMap;

        public CastleViewer(Model.ChapterSaveRegions.MyCastleRegion castleRegion)
        {
            _castleRegion = castleRegion;
            InitializeComponent();

            _castleMap = new CastleMap();
            _castleMap.SelectionChanged += (_, text) => lblSelectedBuilding.Text = text;
            castleMapHost.Content = _castleMap;

            Opened += OnOpened;
        }

        private void OnOpened(object? sender, EventArgs e)
        {
            txtBuildingList.Text = "";
            foreach (var building in _castleRegion.Buildings)
            {
                txtBuildingList.Text += building.GetCondensedInformation() + Environment.NewLine;
            }

            _castleMap.LoadCastleRegion(_castleRegion);
        }

        private void BtnClose_Click(object? sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
