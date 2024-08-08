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
        string tempKey = "asdf";
        static char[] tempMatrixCore = ("abcdefghijklmnopqrstuvwxyz").ToCharArray();

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
            if (converterState)
            {
                char[][] matrix = FormMatrix(tempMatrixCore);
                ResultingMessageTextBox.Text = new string(matrix[1]); // Encrypt(EnterMessageTextBox.Text, tempKey);
            }
            if (!converterState)
            {
                ResultingMessageTextBox.Text = Decrypt(EnterMessageTextBox.Text, tempKey);
            }
            EnterMessageTextBox.Text = string.Empty;
            EnterMessageTextBox.Watermark = string.Empty;
        }

        public static char[][] FormMatrix(char[] core)
        {
            char[][] matrix = new char[core.Length][];
            char buffer;
            for (int i = 0; i < core.Length; i++)
            {
                matrix[i] = (char[])core.Clone();
                buffer = core[0];
                Array.Copy(core, 1, core, 0, core.Length - 1);
                core[core.Length - 1] = buffer;
            }

            return matrix;
        }


        public static string Encrypt(string plaintext, string key)
        {
            string result = string.Empty;

            return result;
        }

        public static string Decrypt(string ciphertext, string key)
        {
            string result = string.Empty;

            return result;
        }
    }
}