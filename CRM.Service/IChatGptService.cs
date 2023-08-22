namespace CRM.Service
{
    public interface IChatGptService
    {
        Task<string> GenerateSuggestedContent(string contentBody);
    }
}