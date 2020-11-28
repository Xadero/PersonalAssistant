using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PersonalAssistant.Common
{
    public class AssistantHelper
    {
        public static string[] CreateCommandList(List<Command> commands)
        {
            var commandList = new List<string>();

            foreach (var command in commands)
            {
                byte[] bytes = Encoding.ASCII.GetBytes(command.CommandText);
                command.CommandText = Encoding.GetEncoding("UTF-8").GetString(bytes);
                commandList.Add(command.CommandText);
            }

            return commandList.ToArray();
        }

        public static Command GetRecognizedCommand(List<Command> commands, string commandText)
        {
            foreach (var command in commands)
            {
                if (command.CommandText == commandText)
                    return command;
            }

            return new Command();
        }
    }
}
