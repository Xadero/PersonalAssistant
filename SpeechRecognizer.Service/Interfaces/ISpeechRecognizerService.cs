using System;
using System.Collections.Generic;
using System.Speech.Recognition;
using System.Speech.Synthesis;

namespace PersonalAssistant.Service.Interfaces
{
    public interface ISpeechRecognizerService
    {
        void CreateNewSynthesizer(string[] commandList, SpeechRecognitionEngine recognizer, SpeechSynthesizer sythesizer, SpeechRecognitionEngine listener, EventHandler<SpeechRecognizedEventArgs> DefaultSpeechRecognized, EventHandler<SpeechDetectedEventArgs> RecognizerSpeechRecognized, EventHandler<SpeechRecognizedEventArgs> ListenerSpeechRecognize);

        string[] CreateCommandList(List<Command> commands);
    }
}
