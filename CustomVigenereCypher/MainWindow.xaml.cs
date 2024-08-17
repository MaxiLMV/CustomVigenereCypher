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
        static string defaultKey = "asdf";
        static string defaultAlphabet = "abcdefghijklmnopqrstuvwxyz";
        static Dictionary<char, int> charToIndex = new Dictionary<char, int>();
        static Dictionary<int, char> indexToChar = new Dictionary<int, char>();
        static int alphabetLength = defaultAlphabet.Length;

        public VigenereMain()
        {
            InitializeComponent();
            charToIndex = CreateCharToIndexMap(defaultAlphabet);
            indexToChar = CreateIndexToCharMap(defaultAlphabet);
        }

        private void ResultingMessageTextBox_Initialized(object sender, EventArgs e)
        {
            ResultingMessageTextBox.IsReadOnly = true;
        }

        private void SwapButton_Click(object sender, RoutedEventArgs e)
        {
            converterState = !converterState;
            ConverterButton.Content = converterState ? "Encrypt" : "Decrypt";
            EnterMessageLabel.Content = converterState ? "MESSAGE TO ENCRYPT" : "MESSAGE TO DECRYPT";

            if (converterState)
            {
                EnterMessageTextBox.Text = string.Empty;
                EnterMessageTextBox.Watermark = "Enter your message here";
            }
            else
            {
                EnterMessageTextBox.Text = ResultingMessageTextBox.Text;
            }

            ResultingMessageTextBox.Text = string.Empty;
        }

        private void ConverterButton_Click(object sender, RoutedEventArgs e)
        {
            if (EnterMessageTextBox.Text == string.Empty)
            {
                EnterMessageTextBox.Watermark = "Please enter a message";
                return;
            }
            string key = string.IsNullOrEmpty(EnterCypherKeyTextBox.Text) ? defaultKey : EnterCypherKeyTextBox.Text;
            ResultingMessageTextBox.Text = converterState ? Encrypt(EnterMessageTextBox.Text, key) : Decrypt(EnterMessageTextBox.Text, key);
            EnterMessageTextBox.Text = string.Empty;
            EnterMessageTextBox.Watermark = string.Empty;
        }

        private Dictionary<char, int> CreateCharToIndexMap(string alphabet)
        {
            Dictionary<char, int> map = new Dictionary<char, int>();
            for (int i = 0; i < alphabet.Length; i++)
            {
                map[alphabet[i]] = i;
            }
            return map;
        }

        private Dictionary<int, char> CreateIndexToCharMap(string alphabet)
        {
            Dictionary<int, char> map = new Dictionary<int, char>();
            for (int i = 0; i < alphabet.Length; i++)
            {
                map[i] = alphabet[i];
            }
            return map;
        }

        // plaintext = "I like trains"
        // key = "asfd"
        // neededKey = "asdfasdfasd"
        // expectedResult = "I dlpe lufifv"
        private static string ExtendKey(string key, int length)
        {
            if (key.Length >= length)
            {
                return key.Substring(0, length);
            }

            StringBuilder extendedKey = new StringBuilder();
            int keyIndex = 0;

            for (int i = 0; i < length; i++)
            {
                extendedKey.Append(key[keyIndex]);

                keyIndex++;
                if (keyIndex >= key.Length)
                {
                    keyIndex = 0;
                }
            }

            return extendedKey.ToString();
        }

        public static string Encrypt(string plaintext, string key)
        {
            string result = string.Empty;
            int skippedSymbols = 0;

            string extendedKey = ExtendKey(key, plaintext.Length);

            for (int i = 0; i < plaintext.Length; i++)
            {
                char plainChar = plaintext[i];

                if (charToIndex.ContainsKey(plainChar))
                {
                    int plainIndex = charToIndex[plainChar];
                    int keyIndex = charToIndex[extendedKey[i - skippedSymbols]];

                    int cipherIndex = (plainIndex + keyIndex) % alphabetLength;

                    char cipherChar = indexToChar[cipherIndex];

                    result += cipherChar;
                }
                else
                {
                    skippedSymbols++;
                    result += plainChar;
                }
            }

            return result;
        }

        public static string Decrypt(string ciphertext, string key)
        {
            string result = string.Empty;
            int skippedSymbols = 0;

            string extendedKey = ExtendKey(key, ciphertext.Length);

            for (int i = 0; i < ciphertext.Length; i++)
            {
                char cipherChar = ciphertext[i];

                if (charToIndex.ContainsKey(cipherChar))
                {
                    int cipherIndex = charToIndex[cipherChar];
                    int keyIndex = charToIndex[extendedKey[i - skippedSymbols]];

                    int plainIndex = (cipherIndex - keyIndex + alphabetLength) % alphabetLength;
                    char plainChar = indexToChar[plainIndex];

                    result += plainChar;
                }
                else
                {
                    skippedSymbols++;
                    result += cipherChar;
                }
            }

            return result;
        }
    }
}