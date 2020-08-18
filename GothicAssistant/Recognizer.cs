using SpeechRecognizer.Service;
using System.Collections.Generic;
using System.Speech.Synthesis;

namespace GothicAssistant
{
    public class Recognizer
    {
        public void DiegoRecognizer(List<Command> commands, string text)
        {
            foreach (var command in commands)
            {
                if (command.CommandText == text)
                {
                    SoundService.PlaySound(@"Sounds/Diego/" + command.Action);
                }
            }
        }
    }
}
