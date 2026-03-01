using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Avalonia.Controls;
using Avalonia.Media;
using FEFTwiddler.Extensions;

namespace FEFTwiddler.GUI.Controls
{
    public partial class HexBox : UserControl
    {
        private byte[] _bytes = Array.Empty<byte>();
        private const int BytesPerRow = 0x10;

        public HexBox()
        {
            InitializeComponent();
        }

        public void SetBytes(byte[] bytes)
        {
            _bytes = (byte[])bytes.Clone();
            Rebuild();
        }

        public byte[] GetBytes() => _bytes;

        private void Rebuild()
        {
            pnlRows.Children.Clear();
            var rowCount = (_bytes.Length == 0) ? 0 : ((_bytes.Length - 1) / BytesPerRow) + 1;
            for (int row = 0; row < rowCount; row++)
            {
                int bytesInRow = Math.Min(_bytes.Length - (row * BytesPerRow), BytesPerRow);
                int capturedRow = row;

                var rowPanel = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal, Spacing = 4 };
                var lbl = new TextBlock
                {
                    Text = $"{row * BytesPerRow:X2}",
                    Foreground = Brushes.Blue,
                    FontFamily = new FontFamily("Courier New"),
                    FontSize = 12,
                    Width = 24,
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center
                };
                var tb = new TextBox
                {
                    Text = GetRowText(row, bytesInRow),
                    FontFamily = new FontFamily("Courier New"),
                    FontSize = 12,
                    Width = 380
                };
                tb.TextChanged += (_, _) => UpdateBytesFromRow(capturedRow, tb.Text ?? "");
                rowPanel.Children.Add(lbl);
                rowPanel.Children.Add(tb);
                pnlRows.Children.Add(rowPanel);
            }
        }

        private string GetRowText(int row, int bytesInRow)
        {
            var parts = new List<string>();
            int first = row * BytesPerRow;
            for (int i = first; i < first + bytesInRow; i++)
                parts.Add($"{_bytes[i]:X2}");
            return string.Join(" ", parts);
        }

        private void UpdateBytesFromRow(int row, string text)
        {
            var pattern = @"^(?<first>[0-9a-fA-F]{2})( (?<rest>[0-9a-fA-F]{2}))*$";
            var matches = Regex.Matches(text.Trim(), pattern);
            if (matches.Count == 0) return;

            var rowBytes = new List<byte>();
            foreach (Match m in matches)
            {
                foreach (Capture cap in m.Groups["first"].Captures)
                {
                    var b = new byte[1]; b.TryParseHex(cap.Value); rowBytes.Add(b[0]);
                }
                foreach (Capture cap in m.Groups["rest"].Captures)
                {
                    var b = new byte[1]; b.TryParseHex(cap.Value); rowBytes.Add(b[0]);
                }
            }

            int dest = row * BytesPerRow;
            Array.Copy(rowBytes.ToArray(), 0, _bytes, dest, Math.Min(rowBytes.Count, _bytes.Length - dest));
        }
    }
}
