using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using Newtonsoft.Json;
using GothicAssistant;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Linq;
using PersonalAssistant.GothicAssistant;
using PersonalAssistant.Service.Interfaces;
using PersonalAssistant.Common;

namespace PersonalAssistant
{
    public partial class GothicPersonalAssistant : Window
    {
        SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine();
        SpeechSynthesizer Bezi = new SpeechSynthesizer();
        SpeechRecognitionEngine listener = new SpeechRecognitionEngine();
        Random rnd = new Random();
        DispatcherTimer dispatcherTimer = new DispatcherTimer();

        int RecTimeout = 0;
        int SelectedAssistantId = 0;
        static string IconPath = "/PersonalAssistant;component/GothicAssistant/Images/";
        CommandConfig commands = null;
        ISoundService _soundService;

        public GothicPersonalAssistant(ISoundService soundService, ISpeechRecognizerService speechRecognizerService)
        {
            _soundService = soundService;
            InitializeComponent();
            SetAssistantIcon(SelectedAssistantId);
            commands = JsonConvert.DeserializeObject<CommandConfig>(File.ReadAllText(@"GothicAssistant/Commands.json"));
            speechRecognizerService.CreateNewSynthesizer(commands.Command.Select(x => x.CommandText).ToArray(), recognizer, Bezi, listener, DefaultSpeechRecognized, RecognizerSpeechRecognized, ListenerSpeechRecognize);

            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        public void DefaultSpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            var recognizer = new Recognizer(_soundService);
            recognizer.ExecuteRecognizedAction(commands.Command.Where(x => x.AssistantId == 0).ToList(), e.Result.Text, SelectedAssistantId);
        }

        private void RecognizerSpeechRecognized(object sender, SpeechDetectedEventArgs e)
        {
            RecTimeout = 0;
        }

        private void ListenerSpeechRecognize(object sender, SpeechRecognizedEventArgs e)
        {
            var x = e.Result.Text;

            if (x == "Wake up")
            {
                listener.RecognizeAsyncCancel();
                Bezi.SpeakAsync("What's up");
                recognizer.RecognizeAsync(RecognizeMode.Multiple);
            }
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (RecTimeout == 10)
            {
                recognizer.RecognizeAsyncCancel();
            }
            else if (RecTimeout == 11)
            {
                dispatcherTimer.Stop();
                listener.RecognizeAsync(RecognizeMode.Multiple);
                RecTimeout = 0;
            }
        }

        private void SelectAssistant_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedAssistantId = SelectAssistant.SelectedIndex;
            switch (SelectedAssistantId)
            {
                case (int)eAssistant.Diego:
                    _soundService.PlaySound(@"GothicAssistant/Sounds/Diego/INFO_DIEGO_GAMESTART_11_00.WAV");
                    break;
                case (int)eAssistant.Milten:
                    _soundService.PlaySound(@"GothicAssistant/Sounds/Milten/DIA_MILTENOW_HELLO_03_00.WAV");
                    break;
                case (int)eAssistant.Xardas:
                    _soundService.PlaySound(@"GothicAssistant/Sounds/Xardas/INFO_XARDAS_DISTURB_14_01.WAV");
                    break;
                case (int)eAssistant.Gorn:
                    _soundService.PlaySound(@"GothicAssistant/Sounds/Gorn/DIA_GORN_FIRST_09_02.WAV");
                    break;
                case (int)eAssistant.Lester:
                    _soundService.PlaySound(@"GothicAssistant/Sounds/Lester/DIA_LESTER_HALLO_05_01.WAV");
                    break;
            }

            if (AssistantIcon != null)
                SetAssistantIcon(SelectedAssistantId);
        }

        private void SetAssistantIcon (int assistantId)
        {
            switch (assistantId)
            {
                case (int)eAssistant.Diego:
                    AssistantIcon.Source = new BitmapImage(new Uri(IconPath + "Diego.png", UriKind.Relative));
                    break;
                case (int)eAssistant.Milten:
                    AssistantIcon.Source = new BitmapImage(new Uri(IconPath + "Milten.png", UriKind.Relative));
                    break;
                case (int)eAssistant.Xardas:
                    AssistantIcon.Source = new BitmapImage(new Uri(IconPath + "Xardas.png", UriKind.Relative));
                    break;
                case (int)eAssistant.Gorn:
                    AssistantIcon.Source = new BitmapImage(new Uri(IconPath + "Gorn.png", UriKind.Relative));
                    break;
                case (int)eAssistant.Lester:
                    AssistantIcon.Source = new BitmapImage(new Uri(IconPath + "Lester.png", UriKind.Relative));
                    break;
            }
        }
    }
}
