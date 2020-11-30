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
            if (openFileDialog.ShowDialog().Value)
            {
                actionTxt.Text = openFileDialog.FileName;
            }

        }

        private void actionTypeCb_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (actionTxt == null)
                return;

            switch (actionTypeCb.SelectedIndex)
            {
                case 0:
                    fileDialogBtn.IsEnabled = false;
                    actionTxt.Text = string.Empty;
                    actionTxt.IsEnabled = false;
                    actionLbl.Content = Properties.Resources.Action;
                    break;
                case 1:
                    fileDialogBtn.IsEnabled = false;
                    actionTxt.Text = string.Empty;
                    actionTxt.IsEnabled = true;
                    actionLbl.Content = Properties.Resources.URLAddress;
                    break;
                case 2:
                    fileDialogBtn.IsEnabled = true;
                    actionTxt.Text = string.Empty;
                    actionTxt.IsEnabled = true;
                    actionLbl.Content = Properties.Resources.FilePath;
                    break;
                case 3:
                    fileDialogBtn.IsEnabled = true;
                    actionTxt.Text = string.Empty;
                    actionTxt.IsEnabled = true;
                    actionLbl.Content = Properties.Resources.ProgramPath;
                    break;
                case 4:
                    fileDialogBtn.IsEnabled = true;
                    actionTxt.Text = string.Empty;
                    actionTxt.IsEnabled = true;
                    actionLbl.Content = Properties.Resources.DirectoryPath;
                    break;
            }
        }
    }
}
