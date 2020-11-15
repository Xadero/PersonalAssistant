using Newtonsoft.Json;
using PersonalAssistant.ClassicAssistant;
using PersonalAssistant.Common;
using PersonalAssistant.Service;
using PersonalAssistant.Service.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace PersonalAssistant
{
    /// <summary>
    /// Interaction logic for ClassicPersonalAssistant.xaml
    /// </summary>
    public partial class ClassicPersonalAssistant : Window
    {
        ISpeechRecognizerService _speechRecognizerService;
        SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine();
        SpeechSynthesizer Sara = new SpeechSynthesizer();
        SpeechRecognitionEngine listener = new SpeechRecognitionEngine();
        DispatcherTimer dispatcherTimer = new DispatcherTimer();

        int RecTimeout = 0;
        CommandConfig commands = null;
        public ClassicPersonalAssistant(ISpeechRecognizerService speechRecognizerService)
        {
            _speechRecognizerService = speechRecognizerService;
            InitializeComponent();
            commands = JsonConvert.DeserializeObject<CommandConfig>(File.ReadAllText(@"ClassicAssistant/Commands.json"), new JsonSerializerSettings { Culture = new System.Globalization.CultureInfo("pl-pl") });
            var c = AssistantHelper.CreateCommandList(commands.Command);
            _speechRecognizerService.CreateNewSynthesizer(commands.Command.Select(x => x.CommandText).ToArray(), recognizer, Sara, listener, DefaultSpeechRecognized, RecognizerSpeechRecognized, ListenerSpeechRecognize);

            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
            Sara.SpeakAsync("Cześć jestem Sara i jestem twoim osobistym asystentem");
        }

        public void DefaultSpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            var recognizedCommand = AssistantHelper.GetRecognizedCommand(commands.Command, e.Result.Text);
            _speechRecognizerService.ExecuteRecognizedAction(Sara, recognizedCommand);
        }

        private void RecognizerSpeechRecognized(object sender, SpeechDetectedEventArgs e)
        {
            RecTimeout = 0;
        }

        private void ListenerSpeechRecognize(object sender, SpeechRecognizedEventArgs e)
        {
            var x = e.Result.Text;

            if (x == "ślad")
            {
                listener.RecognizeAsyncCancel();
                Sara.SpeakAsync("What's up");
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
    }
}
