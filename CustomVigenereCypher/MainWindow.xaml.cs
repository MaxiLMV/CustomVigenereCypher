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
        public VigenereMain()
        {
            InitializeComponent();
        }

        private void ResultingMessage_Initialized(object sender, EventArgs e)
        {
            ResultingMessage.IsReadOnly = true;
        }
    }
}