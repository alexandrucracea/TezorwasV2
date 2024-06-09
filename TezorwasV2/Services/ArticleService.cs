
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using TezorwasV2.Helpers;
using TezorwasV2.Model;

namespace TezorwasV2.Services
{
    public class ArticleService : IArticleService
    {

        IConfiguration configuration;

        public TezorwasApiHelper TezorwasApiHelper { get; set; }

        public ArticleService()
        {
            configuration = MauiProgram.Services.GetService<IConfiguration>()!;
            var settings = configuration!.GetRequiredSection("Settings").Get<Settings>();
            if (settings is not null)
            {
                string tezorwasApiUrl = settings.TezorwasApiUrl + "/" + Enums.Paths.articles;
                TezorwasApiHelper = new TezorwasApiHelper(apiUrl: tezorwasApiUrl);
            }
        }

        public async Task<List<ArticleModel>> GetAllArticles(string bearerToken)
        {
            List<ArticleModel> articles = new List<ArticleModel>();
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

                HttpCallResponseData responseData = new HttpCallResponseData();
                try
                {
                    HttpResponseMessage response = await client.GetAsync(TezorwasApiHelper!.ApiUrl);

                    responseData.StatusCode = (int)response.StatusCode;
                    responseData.Response = await response.Content.ReadAsStringAsync();
                    if (responseData.StatusCode == (int)Enums.StatusCodes.Success)
                    {
                        articles = FirestoreObjectParser.ParseFirestoreArticlesData(responseData.Response);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());

                }
                return articles;

            }
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
