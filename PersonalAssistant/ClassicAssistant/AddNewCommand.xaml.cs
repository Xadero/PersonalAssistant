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
            if (actionTypeCb.SelectedIndex == (int)ActionType.OpenFile)
            {
                var openFileDialog = new OpenFileDialog();
                actionTxt.Text = openFileDialog.ShowDialog().Value ? openFileDialog.FileName : string.Empty;
            }
            else
            {
                using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
                {
                    var result = dialog.ShowDialog();
                    actionTxt.Text = dialog.SelectedPath;
                }
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
                case (int)ActionType.OpenUrl:
                    fileDialogBtn.IsEnabled = false;
                    actionTxt.Text = string.Empty;
                    actionTxt.IsEnabled = true;
                    actionLbl.Content = Properties.Resources.URLAddress;
                    break;
                case (int)ActionType.OpenFile:
                    fileDialogBtn.IsEnabled = true;
                    actionTxt.Text = string.Empty;
                    actionTxt.IsEnabled = true;
                    actionLbl.Content = Properties.Resources.FilePath;
                    break;
                case (int)ActionType.RunProcess:
                    fileDialogBtn.IsEnabled = true;
                    actionTxt.Text = string.Empty;
                    actionTxt.IsEnabled = true;
                    actionLbl.Content = Properties.Resources.ProgramPath;
                    break;
                case (int)ActionType.OpenDirectory:
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
                MessageBox.Show(Properties.Resources.RequiredFieldNotFill, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var newCommand = new Command
            {
                CommandText = "Sara, " + commandTxt.Text,
                ActionTypeId = actionTypeCb.SelectedIndex,
                Action = actionTxt.Text,
                Answer = answerTxt.Text,
                Editable = true,
                NeedsConfirmation = isConfirmation.IsChecked.Value
            };

            try
            {
                _assistantService.StoreCommand(newCommand, @"ClassicAssistant/Commands.json");
                ClassicPersonalAssistant.UpdateCommandsList();
                MessageBox.Show(Properties.Resources.CommandHasBeenAdded, Properties.Resources.Information, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(Properties.Resources.SaveDataError + ex.Message, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
