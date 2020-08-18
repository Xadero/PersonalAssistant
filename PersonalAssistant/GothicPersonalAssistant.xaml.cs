using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Diagnostics;
using SpeechRecognizer.Service;
using Newtonsoft.Json;
using GothicAssistant;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Linq;

namespace GothicPersonalAssistant
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
        static string IconPath = "/GothicAssistant;component/Images/";
        CommandConfig commands = null;

        public GothicPersonalAssistant()
        {
            InitializeComponent();
            SetAssistantIcon(SelectedAssistantId);
            commands = JsonConvert.DeserializeObject<CommandConfig>(File.ReadAllText(@"Commands.json"));
            var commandList = SpeechRecognizer.Service.SpeechRecognizer.CreateCommandList(commands.Command);

            SpeechRecognizer.Service.SpeechRecognizer.CreateNewSynthesizer(commandList, recognizer, Bezi, listener, DefaultSpeechRecognized, RecognizerSpeechRecognized, ListenerSpeechRecognize);

            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        public void DefaultSpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            var recognizer = new Recognizer();
            switch (SelectedAssistantId)
            {
                case (int)eAssistant.Diego:
                    recognizer.DiegoRecognizer(commands.Command.Where(x => x.AssistantId == 0).ToList(), e.Result.Text);
                    break;
                case (int)eAssistant.Milten:
                    recognizer.DiegoRecognizer(commands.Command.Where(x => x.AssistantId == 1).ToList(), e.Result.Text);
                    break;
                case (int)eAssistant.Xardas:
                    recognizer.DiegoRecognizer(commands.Command.Where(x => x.AssistantId == 2).ToList(), e.Result.Text);
                    break;
                case (int)eAssistant.Gorn:
                    recognizer.DiegoRecognizer(commands.Command.Where(x => x.AssistantId == 3).ToList(), e.Result.Text);
                    break;
                case (int)eAssistant.Lester:
                    recognizer.DiegoRecognizer(commands.Command.Where(x => x.AssistantId == 4).ToList(), e.Result.Text);
                    break;
            }

            //    int ranNum;
            //    var speech = e.Result.Text.ToString();

            //    if (speech == "Hello")
            //    {
            //        Bezi.SpeakAsync("Czy to z przodu głowy to twarz czy dupa");
            //    }
            //    else if (speech == "Bezi")
            //    {
            //        Bezi.SpeakAsync("Czego!");
            //    }
            //    else if (speech == "Quiet")
            //    {
            //        Bezi.SpeakAsyncCancelAll();
            //        ranNum = rnd.Next(1, 2);
            //        if (ranNum == 1)
            //        {
            //            Bezi.SpeakAsync("Yes sir");
            //        }

            //        if (ranNum == 2)
            //        {
            //            Bezi.SpeakAsync("I am sorry");
            //        }
            //    }
            //    //else if (speech == "Stop")
            //    //{
            //    //    Bezi.SpeakAsync("If you need me just ask");
            //    //    recognizer.RecognizeAsyncCancel();
            //    //    listener.RecognizeAsync(RecognizeMode.Multiple);
            //    //}
            //    else if (speech == "Pokaż mi swoje towary")
            //    {
            //        Bezi.SpeakAsync("Pewnie, zobacz sobie");
            //        Process.Start("chrome.exe", "http://www.allegro.pl");
            //    }
            //    else if (speech == "Piotrek to fajny chłopak")
            //    {
            //        Bezi.SpeakAsync("Gówno prawda, Piotrek to frajer!");
            //    }
            //    else if (speech == "Pokaż")
            //    {
            //        Bezi.SpeakAsync("Pokaż mi swoje towary. Tak, znam to");
            //        Process.Start("chrome.exe", "http://www.allegro.pl");
            //    }
            //    else if (speech == "Napiłbym sie piwa")
            //    {
            //        Bezi.SpeakAsync("Dostępne bary w okolicy");
            //        Process.Start("chrome.exe", "https://www.google.com/maps/search/bar+Location");
            //    }
            //    else if (speech == "Mój komputer")
            //    {
            //        Bezi.SpeakAsync("otwieram");
            //        Process.Start("explorer.exe");
            //    }
            //    else if (speech == "Chce sie zabawić" || speech == "Szukam kobiety")
            //    {
            //        Bezi.SpeakAsync("A, witaj przystojniaku");
            //        Process.Start("chrome.exe", "http://www.roksa.pl");
            //    }
            //    else if (speech == "Lodówka jest pusta")
            //    {
            //        Bezi.SpeakAsync("Najelpsze promocje dla polskiej cebuli");
            //        Process.Start("chrome.exe", "https://www.gazetkipromocyjne.net/");
            //    }
            //    else
            //    {
            //        Bezi.SpeakAsync("Ni pani maju!");
            //    }

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
                    SoundService.PlaySound(@"Sounds/Diego/INFO_DIEGO_GAMESTART_11_00.WAV");
                    break;
                case (int)eAssistant.Milten:
                    SoundService.PlaySound(@"Sounds/Milten/DIA_MILTENOW_HELLO_03_00.WAV");
                    break;
                case (int)eAssistant.Xardas:
                    SoundService.PlaySound(@"Sounds/Xardas/INFO_XARDAS_DISTURB_14_01.WAV");
                    break;
                case (int)eAssistant.Gorn:
                    SoundService.PlaySound(@"Sounds/Gorn/DIA_GORN_FIRST_09_02.WAV");
                    break;
                case (int)eAssistant.Lester:
                    SoundService.PlaySound(@"Sounds/Lester/DIA_LESTER_HALLO_05_01.WAV");
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
