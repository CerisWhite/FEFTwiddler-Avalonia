using Avalonia.Controls;
using Avalonia.Layout;

namespace FEFTwiddler.GUI.GameProgress
{
    public class BattlefieldPanel : StackPanel
    {
        public BattlefieldPanel(Model.Battlefield battlefield)
        {
            Orientation = Orientation.Horizontal;

            var chapterData = Data.Database.Chapters.GetByID(battlefield.ChapterID);

            Children.Add(new TextBlock
            {
                Text = chapterData.DisplayName,
                VerticalAlignment = VerticalAlignment.Center
            });
        }
    }
}
