using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CustomVigenereCypher
{
    public partial class VigenereMain : Window
    {
        bool converterState = true;

        public VigenereMain()
        {
            InitializeComponent();
        }

        private void ResultingMessageTextBox_Initialized(object sender, EventArgs e)
        {
            ResultingMessageTextBox.IsReadOnly = true;
        }

        private void SwapButton_Click(object sender, RoutedEventArgs e)
        {
            if (converterState)
            {
                converterState = false;
                ConverterButton.Content = "Decrypt";
                EnterMessageLabel.Content = "MESSAGE TO DECRYPT";
                EnterMessageTextBox.Text = ResultingMessageTextBox.Text;
                ResultingMessageTextBox.Text = string.Empty;
                return;
            }
            if (!converterState)
            {
                converterState = true;
                ConverterButton.Content = "Encrypt";
                EnterMessageLabel.Content = "MESSAGE TO ENCRYPT";
                EnterMessageTextBox.Text = string.Empty;
                EnterMessageTextBox.Watermark = "Enter your message here";
                ResultingMessageTextBox.Text = string.Empty;
                return;
            }
        }

        private void ConverterButton_Click(object sender, RoutedEventArgs e)
        {
            if (EnterMessageTextBox.Text == string.Empty)
            {
                EnterMessageTextBox.Watermark = "Please enter a message";
                return;
            }
            ResultingMessageTextBox.Text = EnterMessageTextBox.Text;
            EnterMessageTextBox.Text = string.Empty;
            EnterMessageTextBox.Watermark = string.Empty;
        }
    }
}