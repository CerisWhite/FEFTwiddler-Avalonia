using System;

namespace FEFTwiddler.Model
{
    /// <summary>
    /// Cross-platform color value used in the model layer (replaces System.Drawing.Color).
    /// </summary>
    public readonly struct GameColor
    {
        public byte R { get; }
        public byte G { get; }
        public byte B { get; }
        public byte A { get; }

        public GameColor(byte r, byte g, byte b, byte a = 255)
        {
            R = r; G = g; B = b; A = a;
        }

        public static GameColor FromArgb(byte a, byte r, byte g, byte b) => new(r, g, b, a);
        public static GameColor FromRgb(byte r, byte g, byte b) => new(r, g, b, 255);

        /// <summary>
        /// Sentinel "no color" value: ARGB(1,0,0,0). Used when a hair color slot is absent.
        /// </summary>
        public static readonly GameColor None = new(0, 0, 0, 1);

        /// <summary>Returns true if this color equals the sentinel None value.</summary>
        public bool IsNone => A == 1 && R == 0 && G == 0 && B == 0;

        public bool Equals(GameColor other) => R == other.R && G == other.G && B == other.B && A == other.A;
        public override bool Equals(object? obj) => obj is GameColor c && Equals(c);
        public override int GetHashCode() => HashCode.Combine(R, G, B, A);
        public static bool operator ==(GameColor a, GameColor b) => a.Equals(b);
        public static bool operator !=(GameColor a, GameColor b) => !a.Equals(b);
    }
}
