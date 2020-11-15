using System.Collections.Generic;

namespace PersonalAssistant.Common
{
    public class Command
    {
        public int? AssistantId { get; set; }
        /// <summary>
        /// Tekst podawany przez uzytkownika
        /// </summary>
        public string CommandText { get; set; }

        /// <summary>
        /// Typ akcji
        /// </summary>
        public int ActionTypeId { get; set; }

        /// <summary>
        /// Wykonywana akcja - proces, strona internetowa itp
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Odpowiedz podana przez asystenta
        /// </summary>
        public string Answer { get; set; }

        public bool Editable { get; set; }
    }

    public class CommandConfig
    {
        public List<Command> Command { get; set; }
    }
}


