namespace KLG.Backend.Promotion.Services.Business.Engine
{
    public interface IEngineManager
    {
        Task<string[]> GetWorkflow();
    }
}
