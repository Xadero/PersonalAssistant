using System;
using System.Diagnostics;
using System.IO;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using PersonalAssistant.Common;
using PersonalAssistant.Common.Enums;
using PersonalAssistant.Service.Interfaces;

namespace PersonalAssistant.Service.Services
{
    public class SpeechRecognizerService : ISpeechRecognizerService
    {
        public void CreateNewSynthesizer(string[] commandList, SpeechRecognitionEngine recognizer, SpeechSynthesizer sythesizer, SpeechRecognitionEngine listener, EventHandler<SpeechRecognizedEventArgs> DefaultSpeechRecognized, EventHandler<SpeechDetectedEventArgs> RecognizerSpeechRecognized, EventHandler<SpeechRecognizedEventArgs> ListenerSpeechRecognize)
        {
            recognizer.SetInputToDefaultAudioDevice();
            recognizer.LoadGrammarAsync(new Grammar(new GrammarBuilder(new Choices(commandList))));
            recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(DefaultSpeechRecognized);
            recognizer.SpeechDetected += new EventHandler<SpeechDetectedEventArgs>(RecognizerSpeechRecognized);
            recognizer.RecognizeAsync(RecognizeMode.Multiple);

            listener.SetInputToDefaultAudioDevice();
            listener.LoadGrammarAsync(new Grammar(new GrammarBuilder(new Choices(commandList))));
            listener.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(ListenerSpeechRecognize);
            listener.RecognizeAsync(RecognizeMode.Multiple);
        }

        public void ExecuteRecognizedAction(SpeechSynthesizer Sara, Command command)
        {
            try
            {
                switch (command.ActionTypeId)
                {
                    case (int)ActionType.OpenDirectory:
                        ValidateDirectoryPath(command.Action);
                        break;
                    case (int)ActionType.OpenFile:
                        ValidateFilePath(command.Action);
                        break;
                    case (int)ActionType.OpenUrl:
                        ValidateUrl(command.Action);
                        break;
                }

                if(command.ActionTypeId != (int)ActionType.None)
                    Process.Start(command.Action);
                Sara.SpeakAsync(command.Answer);
            }
            catch (InvalidDataException ex)
            {
                Sara.SpeakAsync(ex.Message);
            }
        }

        private void ValidateUrl(string url)
        {
            Uri uriResult;
            var result = Uri.TryCreate(url, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            if (!result)
                throw new InvalidDataException(Resources.Resource.ResourceManager.GetString("InvalidUrl"));
        }

        private void ValidateDirectoryPath(string directortyPath)
        {
            if (!Directory.Exists(directortyPath))
                throw new InvalidDataException(Resources.Resource.ResourceManager.GetString("InvalidDirectoryPath"));
        }

        private void ValidateFilePath(string filepath) 
        {
            if (!File.Exists(filepath))
              throw new InvalidDataException(Resources.Resource.ResourceManager.GetString("InvalidFilePath"));
        }
    }
}
