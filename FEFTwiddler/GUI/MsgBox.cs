using System.Threading.Tasks;
using Avalonia.Controls;

namespace FEFTwiddler.GUI
{
    /// <summary>
    /// Simple message box helper for Avalonia (replaces System.Windows.Forms.MessageBox).
    /// </summary>
    public static class MsgBox
    {
        public static async Task ShowInfo(Window parent, string message)
        {
            var dlg = new MsgBoxWindow(message, "FEFTwiddler", false);
            await dlg.ShowDialog(parent);
        }

        public static async Task<bool> ShowYesNo(Window parent, string message, string title)
        {
            var dlg = new MsgBoxWindow(message, title, true);
            await dlg.ShowDialog(parent);
            return dlg.Result;
        }
    }
}
