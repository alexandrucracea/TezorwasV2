
using TezorwasV2.Model;

namespace TezorwasV2.Services
{
    public class ArticleService
    {
        HttpClient _httpClient;
        public ArticleService()
        {
            _httpClient = new HttpClient();
        }

        List<ArticleModel> articleList = new();
        public async Task<List<ArticleModel>> GetArticles()
        {
            if (articleList.Count > 0)
                return articleList;

            return articleList;
        }
    }
}
