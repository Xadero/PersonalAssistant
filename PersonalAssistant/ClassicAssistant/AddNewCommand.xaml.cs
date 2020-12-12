using Microsoft.Win32;
using PersonalAssistant.Common;
using PersonalAssistant.Common.Enums;
using PersonalAssistant.Service.Interfaces;
using System;
using System.Windows;

namespace PersonalAssistant.ClassicAssistant
{
    /// <summary>
    /// Interaction logic for AddNewCommand.xaml
    /// </summary>
    public partial class AddNewCommand : Window
    {
        private readonly IAssistantService _assistantService;
        public AddNewCommand(IAssistantService assistantService)
        {
            _assistantService = assistantService;
            InitializeComponent();
        }

        private void FileDialog_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();

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
                case (int)ActionType.None:
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

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(commandTxt.Text) || string.IsNullOrEmpty(commandTxt.Text) || (actionTypeCb.SelectedIndex != 0 && string.IsNullOrEmpty(actionTxt.Text)))
            {
                MessageBox.Show("Należy uzupełnić wszystkie pola!", "BŁĄD", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var newCommand = new Command
            {
                CommandText = "Sara, " + commandTxt.Text,
                ActionTypeId = actionTypeCb.SelectedIndex,
                Action = actionTxt.Text,
                Answer = answerTxt.Text,
                Editable = true,
                IsConfimation = isConfirmation.IsChecked.Value
            };

            try
            {
                _assistantService.StoreCommand(newCommand, @"ClassicAssistant/Commands.json");
                ClassicPersonalAssistant.UpdateCommandsList();
                MessageBox.Show("Komenda została dodana", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wystąpił błąd podczas zapisu danych: " + ex.Message, "BŁĄD", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
