
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using TezorwasV2.DTO;
using TezorwasV2.Helpers;

namespace TezorwasV2.Services
{
    public class ProfileService : IProfileService
    {
        IConfiguration configuration;
        public TezorwasApiHelper TezorwasApiHelper { get; set; }

        public ProfileService()
        {
            configuration = MauiProgram.Services.GetService<IConfiguration>()!;
            var settings = configuration!.GetRequiredSection("Settings").Get<Settings>();
            if (settings is not null)
            {
                string tezorwasApiUrl = settings.TezorwasApiUrl + "/" + Enums.Paths.profiles;
                TezorwasApiHelper = new TezorwasApiHelper(apiUrl: tezorwasApiUrl);
            }
        }
        public async Task<HttpCallResponseData> CreateProfile(ProfileDto profileToCreate, string bearerToken)
        {
            using (HttpClient client = new HttpClient())
            {
                Dictionary<string, dynamic> requestParameters;

                requestParameters = new Dictionary<string, dynamic>
                {
                    { "joinDate", profileToCreate.JoinDate},
                    { "personId", profileToCreate.PersonId},
                    { "level", profileToCreate.Level},
                    {"xp",profileToCreate.Xp}
                };
                if (profileToCreate.Achievments is not null)
                {
                    requestParameters.Add("achievments", profileToCreate.Achievments);
                }

                if (profileToCreate.Habbits is not null)
                {
                    requestParameters.Add("habbits", profileToCreate.Habbits);
                }
                //todo de facut si pentru friendlist si tasks cand se vor adauga

                string jsonBody = JsonConvert.SerializeObject(requestParameters);

                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

                HttpCallResponseData responseData = new HttpCallResponseData();
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
        public async Task<dynamic> GetProfileInfo(string profileId, string bearerToken)
        {
            using (HttpClient client = new HttpClient())
            {

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

                HttpCallResponseData responseData = new HttpCallResponseData();
                try
                {
                    HttpResponseMessage response = await client.GetAsync(TezorwasApiHelper!.ApiUrl + "/" + profileId);

                    responseData.StatusCode = (int)response.StatusCode;
                    responseData.Response = await response.Content.ReadAsStringAsync();
                    if (responseData.StatusCode == (int)Enums.StatusCodes.Success)
                    {
                        var profileParsed = FirestoreObjectParser.ParseFirestoreProfileData(responseData.Response);
                        return profileParsed;
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                return responseData;
            }
        }
        public async Task<List<ProfileDto>> GetAllProfiles(string bearerToken)
        {
            List<ProfileDto> profiles = new List<ProfileDto>();
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
                        profiles = FirestoreObjectParser.ParseFirestoreProfilesData(responseData.Response);;
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                return profiles;
            }
        }
        public async Task<HttpCallResponseData> UpdateAProfile(ProfileDto profileToUpdate, string bearerToken)
        {
            using (HttpClient client = new HttpClient())
            {
                Dictionary<string, dynamic> requestParameters;

                requestParameters = new Dictionary<string, dynamic>
                {
                    { "joinDate", profileToUpdate.JoinDate},
                    { "personId", profileToUpdate.PersonId},
                    { "level", profileToUpdate.Level},
                    { "xp", profileToUpdate.Xp}
                };
                if (profileToUpdate.Achievments is not null)
                {
                    requestParameters.Add("achievments", profileToUpdate.Achievments);
                }

                if (profileToUpdate.Habbits is not null)
                {
                    requestParameters.Add("habbits", profileToUpdate.Habbits);
                }
                if (profileToUpdate.Tasks is not null)
                {
                    requestParameters.Add("tasks", profileToUpdate.Tasks);
                }
                //todo de adaugat friendlist

                string jsonBody = JsonConvert.SerializeObject(requestParameters);

                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

                HttpCallResponseData responseData = new HttpCallResponseData();
                try
                {
                    HttpResponseMessage response = await client.PatchAsync(TezorwasApiHelper!.ApiUrl + "/" + profileToUpdate.Id, content);

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
