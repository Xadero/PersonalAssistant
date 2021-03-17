using Newtonsoft.Json;
using PersonalAssistant.Common;
using PersonalAssistant.Service.Interfaces;
using System.IO;
using System.Linq;

namespace PersonalAssistant.Service.Services
{
    public class AssistantService : IAssistantService
    {
        public void StoreCommand(Command command, string filePath)
        {
            var jsonData = File.ReadAllText(filePath);
            var commands = JsonConvert.DeserializeObject<CommandConfig>(jsonData) ?? new CommandConfig();

            if (commands.Command.Where(x => x.CommandText.ToLower().Contains(command.CommandText.ToLower())).Any())
                throw new System.Exception("Komenda o takiej treści już istnieje!");

            commands.Command.Add(command);
            jsonData = JsonConvert.SerializeObject(commands);
            File.WriteAllText(filePath, jsonData);
        }
    }
}
