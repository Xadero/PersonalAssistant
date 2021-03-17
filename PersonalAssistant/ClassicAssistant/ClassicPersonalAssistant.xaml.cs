using Newtonsoft.Json;
using PersonalAssistant.ClassicAssistant;
using PersonalAssistant.Common;
using PersonalAssistant.Service.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Windows;
using System.Windows.Threading;
using Forms = System.Windows.Forms;

namespace PersonalAssistant
{
    /// <summary>
    /// Interaction logic for ClassicPersonalAssistant.xaml
    /// </summary>
    public partial class ClassicPersonalAssistant : Window
    {
        ISpeechRecognizerService _speechRecognizerService;
        IAssistantService _assistantService;

        SpeechSynthesizer Sara = new SpeechSynthesizer();
        static SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine();
        SpeechRecognitionEngine listener = new SpeechRecognitionEngine();
        DispatcherTimer dispatcherTimer = new DispatcherTimer();

        public static CommandConfig commands = null;
        Command recognizedCommand = null;

        Forms.NotifyIcon notifyIcon = null;
        Forms.ContextMenu contextMenu = null;

        int RecTimeout = 0;

        public ClassicPersonalAssistant(ISpeechRecognizerService speechRecognizerService, IAssistantService assistantService)
        {
            _speechRecognizerService = speechRecognizerService;
            _assistantService = assistantService;

            InitializeComponent();
            UpdateCommandsList();
            _speechRecognizerService.CreateNewSynthesizer(commands.Command.Where(x => !x.IsConfimation).Select(x => x.CommandText).ToArray(), recognizer, Sara, listener, DefaultSpeechRecognized, RecognizerSpeechRecognized, ListenerSpeechRecognize);

            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
            Sara.SpeakAsync(Properties.Resources.SaraIntroduce);
            PrepareSystemTray();
            contextMenu.MenuItems[0].Enabled = false;
        }

        public void DefaultSpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            recognizedCommand = AssistantHelper.GetRecognizedCommand(commands.Command, e.Result.Text);
            if (recognizedCommand.CommandText == Properties.Resources.AddNewCommand)
            {
                if (!AssistantHelper.IsWindowOpen<AddNewCommand>())
                {
                    var addNewCommand = new AddNewCommand(_assistantService);
                    addNewCommand.Show();
                    Sara.SpeakAsync(recognizedCommand.Answer);
                }
                else
                    Sara.SpeakAsync(Properties.Resources.FormIsAlreadyOpen);
            }
            else if (recognizedCommand.CommandText == Properties.Resources.ShowCommands)
            {
                if (!AssistantHelper.IsWindowOpen<CommandManagement>())
                {
                    var commandManagement = new CommandManagement();
                    commandManagement.Show();
                    Sara.SpeakAsync(recognizedCommand.Answer);
                }
                else
                    Sara.SpeakAsync(Properties.Resources.FormIsAlreadyOpen);
            }
            else if (recognizedCommand.CommandText == Properties.Resources.TimeForBreak)
            {
                Sara.SpeakAsync(recognizedCommand.Answer);
                CancelRecognize();
                contextMenu.MenuItems[0].Enabled = true;
                contextMenu.MenuItems[1].Enabled = false;
            }
            else if (recognizedCommand.NeedsConfirmation)
            {
                Sara.SpeakAsync(Properties.Resources.ConfirmOperation);
                StopRecognize();
                recognizer = new SpeechRecognitionEngine();
                _speechRecognizerService.CreateNewSynthesizer(commands.Command.Where(x => x.IsConfimation).Select(x => x.CommandText).ToArray(), recognizer, Sara, listener, ConfirmationSpeechRecognized, RecognizerSpeechRecognized, ListenerSpeechRecognize);
            }
            else
                _speechRecognizerService.ExecuteRecognizedAction(Sara, recognizedCommand);
        }

        private void RecognizerSpeechRecognized(object sender, SpeechDetectedEventArgs e)
        {
            RecTimeout = 0;
        }

        private void ConfirmationSpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Text == Properties.Resources.Yes)
                _speechRecognizerService.ExecuteRecognizedAction(Sara, recognizedCommand);
            else
                Sara.SpeakAsync(Properties.Resources.OperationCancelled);

            StopRecognize();
            recognizer = new SpeechRecognitionEngine();
            _speechRecognizerService.CreateNewSynthesizer(commands.Command.Where(x => !x.IsConfimation).Select(x => x.CommandText).ToArray(), recognizer, Sara, listener, DefaultSpeechRecognized, RecognizerSpeechRecognized, ListenerSpeechRecognize);
        }



        private void ListenerSpeechRecognize(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Text == Properties.Resources.SaraIsNeeded)
            {
                Sara.SpeakAsync(Properties.Resources.WhatCanDo);
                recognizer.RecognizeAsync(RecognizeMode.Multiple);
                contextMenu.MenuItems[0].Enabled = false;
                contextMenu.MenuItems[1].Enabled = true;
                dispatcherTimer.Start();
            }
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            RecTimeout++;
            if (RecTimeout == 10)
            {
                Sara.SpeakAsync(Properties.Resources.AskIfYouNeed);
                CancelRecognize();
                dispatcherTimer.Stop();
                RecTimeout = 0;
            }
        }

        public static void UpdateCommandsList()
        {
            commands = JsonConvert.DeserializeObject<CommandConfig>(File.ReadAllText(@"ClassicAssistant/Commands.json"), new JsonSerializerSettings { Culture = new System.Globalization.CultureInfo("pl-pl") });
            recognizer.LoadGrammarAsync(new Grammar(new GrammarBuilder(new Choices(commands.Command.Where(x => !x.IsConfimation).Select(x => x.CommandText).ToArray()))));
        }

        private void CancelRecognize()
        {
            recognizer.RecognizeAsyncCancel();
            listener.RecognizeAsyncCancel();
        }

        private void StopRecognize()
        {
            recognizer.RecognizeAsyncStop();
            listener.RecognizeAsyncStop();
        }

        #region System Tray
        private void PrepareSystemTray()
        {
            notifyIcon = new Forms.NotifyIcon();
            notifyIcon.Icon = new System.Drawing.Icon(@"ClassicAssistant\avatar.ico");
            notifyIcon.Visible = true;
            contextMenu = new Forms.ContextMenu(new Forms.MenuItem[]
            {
                new Forms.MenuItem(Properties.Resources.SysTrayEnableRecognizer, SysTrayEnableRecognizer),
                new Forms.MenuItem(Properties.Resources.SysTrayDisableRecognizer, SysTrayDisableRecognizer),
                new Forms.MenuItem(Properties.Resources.SysTrayAddCommand, SysTrayAddCommand),
                new Forms.MenuItem(Properties.Resources.SysTrayShowCommands, SysTrayShowCommands),
                new Forms.MenuItem(Properties.Resources.SysTrayShowConfig, SysTrayShowConfig),
                new Forms.MenuItem(Properties.Resources.SysTrayCloseApplication, SysTrayCloseApplication),
            });

            notifyIcon.ContextMenu = contextMenu;
        }

        private void SysTrayShowConfig(object sender, EventArgs e)
        {
            if (!AssistantHelper.IsWindowOpen<ClassicAssistant.Configuration>())
            {
                var configuration = new ClassicAssistant.Configuration();
                configuration.Show();
            }
            else
                MessageBox.Show(Properties.Resources.FormIsAlreadyOpen, Properties.Resources.Information, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SysTrayEnableRecognizer(object sender, EventArgs e)
        {
            contextMenu.MenuItems[0].Enabled = false;
            contextMenu.MenuItems[1].Enabled = true;
            recognizer.RecognizeAsync(RecognizeMode.Multiple);
        }

        private void SysTrayDisableRecognizer(object sender, EventArgs e)
        {
            contextMenu.MenuItems[0].Enabled = true;
            contextMenu.MenuItems[1].Enabled = false;
            CancelRecognize();
        }

        private void SysTrayCloseApplication(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void SysTrayShowCommands(object sender, EventArgs e)
        {
            if (!AssistantHelper.IsWindowOpen<CommandManagement>())
            {
                var commandManagement = new CommandManagement();
                commandManagement.Show();
            }
            else
                MessageBox.Show(Properties.Resources.FormIsAlreadyOpen, Properties.Resources.Information, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SysTrayAddCommand(object sender, EventArgs e)
        {
            if (!AssistantHelper.IsWindowOpen<AddNewCommand>())
            {
                var addNewCommand = new AddNewCommand(_assistantService);
                addNewCommand.Show();
            }
            else
                MessageBox.Show(Properties.Resources.FormIsAlreadyOpen, Properties.Resources.Information, MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion
    }
}
