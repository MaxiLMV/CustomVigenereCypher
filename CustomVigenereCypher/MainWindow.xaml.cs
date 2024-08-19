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
        bool keyVariant = true; // True - Repeating, False - Autokey
        bool includeForeign = true;

        static string defaultKey = "asdf";
        static string defaultAlphabet = "abcdefghijklmnopqrstuvwxyz";
        static int alphabetLength = defaultAlphabet.Length;

        static Dictionary<char, int> charToIndex = new Dictionary<char, int>();
        static Dictionary<int, char> indexToChar = new Dictionary<int, char>();

        public VigenereMain()
        {
            InitializeComponent();
            EnterAplhabetTextBox.Text = defaultAlphabet;
            EnterCypherKeyTextBox.Text = defaultKey;
            FormMaps(EnterAplhabetTextBox.Text);
        }

        private void ResultingMessageTextBox_Initialized(object sender, EventArgs e)
        {
            ResultingMessageTextBox.IsReadOnly = true;
        }

        private void SwapButton_Click(object sender, RoutedEventArgs e)
        {
            converterState = !converterState;

            if (converterState)
            {
                string key = string.IsNullOrEmpty(EnterCypherKeyTextBox.Text) ? defaultKey : EnterCypherKeyTextBox.Text;
                EnterMessageTextBox.Text = ResultingMessageTextBox.Text;
                ResultingMessageTextBox.Text = Encrypt(EnterMessageTextBox.Text, key, includeForeign);
            }
            else
            {
                string key = string.IsNullOrEmpty(EnterCypherKeyTextBox.Text) ? defaultKey : EnterCypherKeyTextBox.Text;
                EnterMessageTextBox.Text = ResultingMessageTextBox.Text;
                ResultingMessageTextBox.Text = Decrypt(EnterMessageTextBox.Text, key, includeForeign);
            }
        }

        private void EnterMessageTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateResult();
        }

        private void UpdateResult()
        {
            string key = string.IsNullOrEmpty(EnterCypherKeyTextBox.Text) ? defaultKey : EnterCypherKeyTextBox.Text;
            ResultingMessageTextBox.Text = converterState ? Encrypt(EnterMessageTextBox.Text, key, includeForeign) : Decrypt(EnterMessageTextBox.Text, key, includeForeign);
        }

        private void FormMaps(string alphabet)
        {
            charToIndex = CreateCharToIndexMap(alphabet);
            indexToChar = CreateIndexToCharMap(alphabet);
        }

        private void EnterCypherKeyTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CheckForSmallKey(EnterCypherKeyTextBox.Text)) return;
            UpdateResult();
        }

        private bool CheckForSmallKey(string key)
        {
            if (key.Length < 2)
            {
                SmallKeyWarningLabel.Visibility = Visibility.Visible;
                return true;
            }
            SmallKeyWarningLabel.Visibility = Visibility.Hidden;
            return false;
        }


        private void EnterAplhabetTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CheckDuplicates(EnterAplhabetTextBox.Text)) return;
            FormMaps(EnterAplhabetTextBox.Text);
            UpdateResult();
        }

        private bool CheckDuplicates(string alphabet)
        {
            HashSet<char> seenChars = new HashSet<char>();

            foreach (char c in alphabet)
            {
                if (!seenChars.Add(c))
                {
                    DuplicatesWarningLabel.Visibility = Visibility.Visible;
                    return true;
                }  
            }
            DuplicatesWarningLabel.Visibility = Visibility.Hidden;
            return false;
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

        private static string ExtendKey(string key, string text, bool keyVariant = true)
        {
            int length = text.Length;

            if (keyVariant)
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
            else
            {
                StringBuilder extendedKey = new StringBuilder(key);

                int textIndex = 0;
                while (extendedKey.Length < length)
                {
                    if (textIndex >= text.Length)
                    {
                        break;
                    }

                    if (text[textIndex] != ' ')
                    {
                        extendedKey.Append(text[textIndex]);
                    }

                    textIndex++;
                }

                while (extendedKey.Length < length)
                {
                    extendedKey.Append(extendedKey[extendedKey.Length % key.Length]);
                }

                return extendedKey.ToString();
            }
        }


        private string Encrypt(string plaintext, string key, bool includeForeign = true)
        {
            string result = string.Empty;
            int skippedSymbols = 0;

            string extendedKey = ExtendKey(key, plaintext, keyVariant);

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
                    if (includeForeign) result += plainChar;
                    else continue;
                }
            }

            return result;
        }

        private string Decrypt(string ciphertext, string key, bool includeForeign = true)
        {
            string result = string.Empty;
            int skippedSymbols = 0;

            string extendedKey = ExtendKey(key, ciphertext, keyVariant);

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
                    if (includeForeign) result += cipherChar;
                    else continue; 
                }
            }

            return result;
        }

        private void EnterAplhabetTextBox_Initialized(object sender, EventArgs e)
        {
            // 
        }

        private void EnterCypherKeyTextBox_Initialized(object sender, EventArgs e)
        {
            // 
        }

        private void EnterAplhabetTextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            EnterAplhabetTextBox.ScrollToHorizontalOffset(0);
        }

        private void EnterCypherKeyTextBox_LostKeyboardFocus_1(object sender, KeyboardFocusChangedEventArgs e)
        {
            EnterCypherKeyTextBox.ScrollToHorizontalOffset(0);
        }

        private void RepeatingKeyButton_Click(object sender, RoutedEventArgs e)
        {
            RepeatingKeyButton.IsEnabled = false;
            AutokeyButton.IsEnabled = true;
            keyVariant = true;
            UpdateResult();
        }

        private void AutokeyButton_Click(object sender, RoutedEventArgs e)
        {
            AutokeyButton.IsEnabled = false;
            RepeatingKeyButton.IsEnabled = true;
            keyVariant = false;
            UpdateResult();
        }

        private void IncludeForeignCharsButton_Click(object sender, RoutedEventArgs e)
        {
            IncludeForeignCharsButton.IsEnabled = false;
            IgnoreForeignCharsButton.IsEnabled = true;
            includeForeign = true;
            UpdateResult();
        }

        private void IgnoreForeignCharsButton_Click(object sender, RoutedEventArgs e)
        {
            IgnoreForeignCharsButton.IsEnabled = false;
            IncludeForeignCharsButton.IsEnabled = true;
            includeForeign = false;
            UpdateResult();
        }
    }
}