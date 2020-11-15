using GothicAssistant;
using PersonalAssistant.Common;
using PersonalAssistant.Service.Interfaces;
using System.Collections.Generic;

namespace PersonalAssistant.GothicAssistant
{
    public class Recognizer
    {
        public ISoundService _soundService;
        public Recognizer(ISoundService soundService)
        {
            _soundService = soundService;
        }

        public void ExecuteRecognizedAction(List<Command> commands, string text, int assistantId)
        {
            foreach (var command in commands)
            {
                if (command.CommandText == text)
                {
                    _soundService.PlaySound(command.Answer);
                }
            }
        }
    }
}
