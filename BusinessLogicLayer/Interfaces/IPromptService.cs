namespace BusinessLogicLayer.Interfaces
{
    public interface IPromptService
    {
        public Task<string> TriggerPromptOpenAI(string prompt);
    }
}
