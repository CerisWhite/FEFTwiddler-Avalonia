using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using FEFTwiddler.Model;

namespace FEFTwiddler.GUI.Controls
{
    public partial class ColorPicker : UserControl
    {
        private GameColor _color;
        private bool _updating;

        public GameColor Color
        {
            get => _color;
            set
            {
                _color = value;
                UpdatePreview();
                UpdateHex();
            }
        }

        public Action<GameColor>? OnChange { get; set; }

        public ColorPicker()
        {
            InitializeComponent();
            BindEvents();
        }

        private void BindEvents()
        {
            pnlPreview.PointerPressed += (_, _) =>
            {
                // No system color dialog in Avalonia without extra packages
                // User can edit hex directly
                OnChange?.Invoke(_color);
            };
            txtHex.TextChanged += (_, _) =>
            {
                if (_updating) return;
                ParseHex();
                OnChange?.Invoke(_color);
            };
        }

        private void UpdatePreview()
        {
            _updating = true;
            pnlPreview.Background = new SolidColorBrush(Avalonia.Media.Color.FromArgb(_color.A, _color.R, _color.G, _color.B));
            _updating = false;
        }

        private void UpdateHex()
        {
            _updating = true;
            txtHex.Text = $"{_color.R:X2}{_color.G:X2}{_color.B:X2}";
            _updating = false;
        }

        private void ParseHex()
        {
            var text = txtHex.Text ?? "";
            if (text.Length < 6) return;
            if (byte.TryParse(text.Substring(0, 2), System.Globalization.NumberStyles.HexNumber, null, out byte r) &&
                byte.TryParse(text.Substring(2, 2), System.Globalization.NumberStyles.HexNumber, null, out byte g) &&
                byte.TryParse(text.Substring(4, 2), System.Globalization.NumberStyles.HexNumber, null, out byte b))
            {
                _color = GameColor.FromArgb(_color.A, r, g, b);
                UpdatePreview();
            }
        }
    }
}
