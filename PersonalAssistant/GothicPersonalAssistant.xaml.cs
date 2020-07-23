using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Speech.Recognition;
using System.Speech.Synthesis;

namespace GothicPersonalAssistant
{
    /// <summary>
    /// Interaction logic for GothicPersonalAssistant.xaml
    /// </summary>
    public partial class GothicPersonalAssistant : Window
    {
        SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine();
        SpeechSynthesizer Bezi = new SpeechSynthesizer();
        SpeechRecognitionEngine listener = new SpeechRecognitionEngine();
        int RecTimeout = 0;
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        Random rnd = new Random();
        public GothicPersonalAssistant()
        {
            InitializeComponent();

            recognizer.SetInputToDefaultAudioDevice();
            recognizer.LoadGrammarAsync(new Grammar(new GrammarBuilder(new Choices(File.ReadAllLines(@"DefaultCommands.txt")))));
            recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(DefaultSpeechRecognized);
            recognizer.SpeechDetected += new EventHandler<SpeechDetectedEventArgs>(RecognizerSpeechRecognized);
            recognizer.RecognizeAsync(RecognizeMode.Multiple);

            listener.SetInputToDefaultAudioDevice();
            listener.LoadGrammarAsync(new Grammar(new GrammarBuilder(new Choices(File.ReadAllLines(@"DefaultCommands.txt")))));
            listener.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(ListenerSpeechRecognize);
            listener.RecognizeAsync(RecognizeMode.Multiple);

            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
            Bezi.SpeakAsync("Bezi");
        }

        private void DefaultSpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            int ranNum;
            var speech = e.Result.Text.ToString();

            if (speech == "Hello")
            {
                Bezi.SpeakAsync("Hello, I am here");
            }
            else if (speech == "Quiet")
            {
                Bezi.SpeakAsyncCancelAll();
                ranNum = rnd.Next(1, 2);
                if (ranNum == 1)
                {
                    Bezi.SpeakAsync("Yes sir");
                }
        
                if (ranNum == 2)
                {
                    Bezi.SpeakAsync("I am sorry");
                }
            }
            else if (speech == "Stop")
            {
                Bezi.SpeakAsync("If you need me just ask");
                recognizer.RecognizeAsyncCancel();
                listener.RecognizeAsync(RecognizeMode.Multiple);
            }
            else
            {
                Bezi.SpeakAsync("Ni pani maju!");
            }
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
    }
}
