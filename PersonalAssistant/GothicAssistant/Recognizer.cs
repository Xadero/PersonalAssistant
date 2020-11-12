using PersonalAssistant.Service;
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
        public void DiegoRecognizer(List<Command> commands, string text)
        {
            foreach (var command in commands)
            {
                if (command.CommandText == text)
                {
                    _soundService.PlaySound(@"GothicAssistant/Sounds/Diego/" + command.Action);
                }
            }
        }

        public void GornRecognizer(List<Command> commands, string text)
        {
            foreach (var command in commands)
            {
                if (command.CommandText == text)
                {
                    _soundService.PlaySound(@"GothicAssistant/Sounds/Gorn/" + command.Action);
                }
            }
        }

        public void XardasRecognizer(List<Command> commands, string text)
        {
            foreach (var command in commands)
            {
                if (command.CommandText == text)
                {
                    _soundService.PlaySound(@"GothicAssistant/Sounds/Xardas/" + command.Action);
                }
            }
        }

        public void MiltenRecognizer(List<Command> commands, string text)
        {
            foreach (var command in commands)
            {
                if (command.CommandText == text)
                {
                    _soundService.PlaySound(@"GothicAssistant/Sounds/Milten/" + command.Action);
                }
            }
        }

        public void LesterRecognizer(List<Command> commands, string text)
        {
            foreach (var command in commands)
            {
                if (command.CommandText == text)
                {
                    _soundService.PlaySound(@"GothicAssistant/Sounds/Lester/" + command.Action);
                }
            }
        }
    }
}
