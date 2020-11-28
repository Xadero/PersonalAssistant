using Microsoft.Win32;
using System.Windows;

namespace PersonalAssistant.ClassicAssistant
{
    /// <summary>
    /// Interaction logic for AddNewCommand.xaml
    /// </summary>
    public partial class AddNewCommand : Window
    {
        public AddNewCommand()
        {
            InitializeComponent();
        }

        private void FileDialog_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
        }
    }
}
