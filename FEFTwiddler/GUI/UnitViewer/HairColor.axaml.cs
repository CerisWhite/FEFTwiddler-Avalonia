using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using FEFTwiddler.Extensions;

namespace FEFTwiddler.GUI.UnitViewer
{
    public partial class HairColor : UserControl
    {
        private Model.Unit? _unit;
        private bool _loading;

        public HairColor()
        {
            InitializeComponent();
        }

        public void LoadUnit(Model.Unit unit)
        {
            _unit = unit;
            _loading = true;
            PopulateControls();
            _loading = false;
            HairColorHex.TextChanged += HandleHexChanged;
        }

        private void PopulateControls()
        {
            var c = _unit!.HairColor;
            HairColorBox.Background = new SolidColorBrush(Avalonia.Media.Color.FromArgb(c[3], c[0], c[1], c[2]));
            HairColorHex.Text = $"{c[0]:X2}{c[1]:X2}{c[2]:X2}";
        }

        private void HandleHexChanged(object? sender, EventArgs e)
        {
            if (_loading || _unit == null) return;
            var text = HairColorHex.Text ?? "";
            if (text.Length < 6) return;
            var bytes = new byte[4];
            if (bytes.TryParseHex(text + "FF"))
            {
                HairColorBox.Background = new SolidColorBrush(Avalonia.Media.Color.FromArgb(bytes[3], bytes[0], bytes[1], bytes[2]));
                _unit.HairColor = bytes;
                if (_unit.AvatarHairColor != null) _unit.AvatarHairColor = bytes;
            }
        }

        private void BtnPickColor_Click(object? sender, RoutedEventArgs e)
        {
            // Avalonia doesn't have a built-in color dialog, so show a simple input dialog
            // The user can type hex in the text box directly
        }
    }
}
