using System;
using System.Collections.Generic;
using System.Speech.Recognition;
using System.Speech.Synthesis;
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

        public string[] CreateCommandList(List<Command> commands)
        {
            var commandList = new List<string>();

            foreach (var command in commands)
            {
                commandList.Add(command.CommandText);
            }

            return commandList.ToArray();
        }
    }
}
