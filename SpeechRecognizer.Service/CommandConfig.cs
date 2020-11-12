using System.Collections.Generic;

namespace PersonalAssistant.Service
{
    public class Command
    {
        public int AssistantId { get; set; }
        public string CommandText { get; set; }
        public string ActionType { get; set; }
        public string Action { get; set; }
    }

    public class CommandConfig
    {
        public List<Command> Command { get; set; }
    }
}


