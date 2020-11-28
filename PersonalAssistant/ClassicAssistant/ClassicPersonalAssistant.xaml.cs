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
            _speechRecognizerService.CreateNewSynthesizer(commands.Command.Select(x => x.CommandText).ToArray(), recognizer, Sara, listener, DefaultSpeechRecognized, RecognizerSpeechRecognized, ListenerSpeechRecognize);

            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
            Sara.SpeakAsync(Properties.Resources.SaraIntroduce);
        }

        public void DefaultSpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            var recognizedCommand = AssistantHelper.GetRecognizedCommand(commands.Command, e.Result.Text);
            if (recognizedCommand.CommandText == Properties.Resources.AddNewCommand)
            {
                if (!IsWindowOpen<AddNewCommand>())
                {
                    var addNewCommand = new AddNewCommand();
                    addNewCommand.Show();
                    Sara.SpeakAsync(recognizedCommand.Answer);
                }
                else
                    Sara.SpeakAsync(Properties.Resources.FormIsAlreadyOpen);
            }
            else if (recognizedCommand.CommandText == Properties.Resources.ShowCommands)
            {
                if (!IsWindowOpen<CommandManagement>())
                {
                    var commandManagement = new CommandManagement();
                    commandManagement.Show();
                    Sara.SpeakAsync(recognizedCommand.Answer);
                }
                else
                    Sara.SpeakAsync(Properties.Resources.FormIsAlreadyOpen);
            }
            else
            {
                _speechRecognizerService.ExecuteRecognizedAction(Sara, recognizedCommand);
            }
        }

        private void RecognizerSpeechRecognized(object sender, SpeechDetectedEventArgs e)
        {
            RecTimeout = 0;
        }

        private void ListenerSpeechRecognize(object sender, SpeechRecognizedEventArgs e)
        {
            var x = e.Result.Text;

            if (x == "Ĺ›lad")
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

        public static bool IsWindowOpen<T>(string name = "") where T : Window
        {
            return string.IsNullOrEmpty(name) ? Application.Current.Windows.OfType<T>().Any() : Application.Current.Windows.OfType<T>().Any(w => w.Name.Equals(name));
        }
    }
}
