using TezorwasV2.Helpers;

namespace TezorwasV2.Services
{
    public interface IGptService
    {
        TezorwasApiHelper TezorwasApiHelper { get; set; }

        Task<dynamic> GenerateTasks(double levelOfWaste, string bearerToken);
    }
}