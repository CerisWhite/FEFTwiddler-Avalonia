using System;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

namespace FEFTwiddler.GUI.GameProgress
{
    public class ChapterHistoryPanel : StackPanel
    {
        public ChapterHistoryPanel(Model.ChapterHistoryEntry entry, bool canUnplay, Action onUnplay)
        {
            Orientation = Orientation.Horizontal;
            Spacing = 8;

            var btn = new Button { Content = "X", Foreground = Brushes.Red, MinWidth = 30 };
            if (canUnplay)
            {
                btn.Click += (_, _) =>
                {
                    onUnplay();
                    (Parent as Avalonia.Controls.Panel)?.Children.Remove(this);
                };
            }
            else
            {
                btn.IsEnabled = false;
            }

            var lbl1 = new TextBlock
            {
                Text = Data.Database.Chapters.GetByID(entry.ChapterID).DisplayName,
                Width = 200,
                VerticalAlignment = VerticalAlignment.Center
            };

            var hero1 = Data.Database.Characters.GetByID(entry.HeroCharacterID_1).DisplayName;
            var hero2 = Data.Database.Characters.GetByID(entry.HeroCharacterID_2).DisplayName;
            var lbl2 = new TextBlock
            {
                Text = $"Turns: {entry.TurnCount} / Heroes: {hero1}, {hero2}",
                VerticalAlignment = VerticalAlignment.Center
            };

            Children.Add(btn);
            Children.Add(lbl1);
            Children.Add(lbl2);
        }
    }
}
