using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;
using TezorwasV2.DTO;
using TezorwasV2.Helpers;
using TezorwasV2.Model;


namespace TezorwasV2.Services
{
    public class RegisterService : IRegisterService
    {
        IConfiguration configuration;
        public AuthHelper? AuthHelper { get; set; }

        public RegisterService()
        {
            configuration = MauiProgram.Services.GetService<IConfiguration>()!;
            var settings = configuration!.GetRequiredSection("Settings").Get<Settings>();
            if (settings is not null)
            {
                string authUrl = settings.RegisterURL + "?key=" + settings.FBKey;
                AuthHelper = new AuthHelper(authUrl);
            }
        }

        public async Task<HttpCallResponseData> RegisterUser(UserDto userToRegister)
        {
            using (HttpClient client = new HttpClient())
            {
                var requestParameters = new Dictionary<string, dynamic>
                {
                    { nameof(UserModel.Email).ToLower(), userToRegister.Username },
                    { nameof(UserModel.Password).ToLower(), userToRegister.Password },
                    {"returnSecureToken",true}
                };

                string jsonBody = JsonConvert.SerializeObject(requestParameters);

                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                HttpCallResponseData responseData = new HttpCallResponseData();
                try
                {
                    HttpResponseMessage response = await client.PostAsync(AuthHelper!.authURL, content);

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
