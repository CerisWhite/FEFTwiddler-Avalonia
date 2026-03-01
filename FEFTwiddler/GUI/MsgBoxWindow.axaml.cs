using Avalonia.Controls;
using Avalonia.Interactivity;

namespace FEFTwiddler.GUI
{
    public partial class MsgBoxWindow : Window
    {
        public bool Result { get; private set; } = false;

        public MsgBoxWindow(string message, string title, bool isYesNo)
        {
            InitializeComponent();
            Title = title;
            lblMessage.Text = message;

            if (isYesNo)
            {
                btnOk.IsVisible = false;
            }
            else
            {
                btnYes.IsVisible = false;
                btnNo.IsVisible = false;
            }
        }

        private void BtnYes_Click(object? sender, RoutedEventArgs e)
        {
            Result = true;
            Close();
        }

        private void BtnNo_Click(object? sender, RoutedEventArgs e)
        {
            Result = false;
            Close();
        }

        private void BtnOk_Click(object? sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
