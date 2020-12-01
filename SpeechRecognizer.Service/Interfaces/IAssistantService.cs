using PersonalAssistant.Common;

namespace PersonalAssistant.Service.Interfaces
{
    public interface IAssistantService
    {
        void StoreCommand(Command command, string filepath);
    }
}
