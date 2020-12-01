using Newtonsoft.Json;
using PersonalAssistant.Common;
using PersonalAssistant.Service.Interfaces;
using System.IO;

namespace PersonalAssistant.Service.Services
{
    public class AssistantService : IAssistantService
    {
        public void StoreCommand(Command command, string filePath)
        {
            var jsonData = File.ReadAllText(filePath);
            var commands = JsonConvert.DeserializeObject<CommandConfig>(jsonData) ?? new CommandConfig();
            commands.Command.Add(command);
            jsonData = JsonConvert.SerializeObject(commands);
            File.WriteAllText(filePath, jsonData);
        }
    }
}
