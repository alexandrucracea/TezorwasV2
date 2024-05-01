using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TezorwasV2.DTO;
using TezorwasV2.Helpers;
using TezorwasV2.Model;

namespace TezorwasV2.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        IConfiguration configuration;
        public AuthHelper? AuthHelper { get; set; }
        public AuthenticationService()
        {
            configuration = MauiProgram.Services.GetService<IConfiguration>()!;
            var settings = configuration!.GetRequiredSection("Settings").Get<Settings>();
            if (settings is not null)
            {
                string authUrl = settings.AuthURL + "?key=" + settings.FBKey;
                AuthHelper = new AuthHelper(authUrl);
            }
        }
        public async Task<HttpCallResponseData> AuthenticateUser(UserDto userToAuthenticate)
        {
            using (HttpClient client = new HttpClient())
            {
                // Define your parameters
                var requestParameters = new Dictionary<string, string>
                {
                    { nameof(UserModel.Email).ToLower(), userToAuthenticate.Username },
                    { nameof(UserModel.Password).ToLower(), userToAuthenticate.Password }
                };

                string jsonBody = JsonConvert.SerializeObject(requestParameters);

                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                HttpCallResponseData responseData = new HttpCallResponseData();
                try
                {
                    HttpResponseMessage response = await client.PostAsync(AuthHelper!.authURL, content);


                    //var jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);
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
