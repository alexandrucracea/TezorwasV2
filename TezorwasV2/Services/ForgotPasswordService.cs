using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;
using TezorwasV2.Helpers;

namespace TezorwasV2.Services
{
    public class ForgotPasswordService : IForgotPasswordService
    {
        IConfiguration configuration;
        public TezorwasApiHelper TezorwasApiHelper { get; set; }


        public ForgotPasswordService()
        {
            configuration = MauiProgram.Services.GetService<IConfiguration>()!;
            var settings = configuration!.GetRequiredSection("Settings").Get<Settings>();
            if (settings is not null)
            {
                string tezorwasApiUrl = settings.TezorwasApiUrl + "/" + Enums.Paths.forgotPassword;
                TezorwasApiHelper = new TezorwasApiHelper(apiUrl: tezorwasApiUrl);
            }
        }

        public async Task<HttpCallResponseData> SendEmailNewPassword(string email)
        {
            using (HttpClient client = new HttpClient())
            {
                Dictionary<string, dynamic> requestParameters;

                requestParameters = new Dictionary<string, dynamic>
                {
                    { "email", email}
                };

                HttpCallResponseData responseData = new HttpCallResponseData();

                string jsonBody = JsonConvert.SerializeObject(requestParameters);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                try
                {
                    HttpResponseMessage response = await client.PostAsync(TezorwasApiHelper!.ApiUrl, content);

                    responseData.StatusCode = (int)response.StatusCode;
                    responseData.Response = await response.Content.ReadAsStringAsync();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                return responseData;
            }
        }


    }
}
