using TezorwasV2.Helpers;
using TezorwasV2.Model;

namespace TezorwasV2.Services
{
    public interface IArticleService
    {
        TezorwasApiHelper TezorwasApiHelper { get; set; }

        Task<List<ArticleModel>> GetAllArticles(string bearerToken);
        Task<List<ArticleModel>> GetArticles();
    }
}