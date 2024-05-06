using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using TezorwasV2.DTO;
using TezorwasV2.Helpers;

namespace TezorwasV2.Services
{
    public class PersonService : IPersonService
    {
        IConfiguration configuration;

        public TezorwasApiHelper TezorwasApiHelper { get; set; }

        public PersonService()
        {
            configuration = MauiProgram.Services.GetService<IConfiguration>()!;
            var settings = configuration!.GetRequiredSection("Settings").Get<Settings>();
            if (settings is not null)
            {
                string tezorwasApiUrl = settings.TezorwasApiUrl + "/" + Enums.Paths.persons;
                TezorwasApiHelper = new TezorwasApiHelper(apiUrl: tezorwasApiUrl);
            }
        }

        public async Task<HttpCallResponseData> CreatePerson(PersonDto personToCreate, string bearerToken)
        {
            using (HttpClient client = new HttpClient())
            {
                Dictionary<string, dynamic> requestParameters;

                requestParameters = new Dictionary<string, dynamic>
                {
                    { "lastName", personToCreate.LastName },
                    { "firstName", personToCreate.FirstName },
                    { "email", personToCreate.Email }
                };
                if (personToCreate.Age is not null)
                {
                    requestParameters.Add(nameof(PersonDto.Age), personToCreate.Age.Value);
                }

                if (personToCreate.Address is not null)
                {
                    var addressDictionary = new Dictionary<string, dynamic>();
                    if (personToCreate.Address.StreetName is not null)
                    {
                        addressDictionary.Add("streetName", personToCreate.Address.StreetName);
                    }
                    if (personToCreate.Address.County is not null)
                    {
                        addressDictionary.Add("county", personToCreate.Address.County);
                    }
                    if (personToCreate.Address.City is not null)
                    {
                        addressDictionary.Add("city", personToCreate.Address.City);
                    }

                    requestParameters.Add("address", addressDictionary);
                }

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
        public async Task<dynamic> GetPersonInfo(string personId, string bearerToken)
        {
            using (HttpClient client = new HttpClient())
            {

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

                HttpCallResponseData responseData = new HttpCallResponseData();
                try
                {
                    HttpResponseMessage response = await client.GetAsync(TezorwasApiHelper!.ApiUrl + "/" + personId);

                    responseData.StatusCode = (int)response.StatusCode;
                    responseData.Response = await response.Content.ReadAsStringAsync();
                    if (responseData.StatusCode == (int)Enums.StatusCodes.Success)
                    {
                        var personParsed = FirestoreObjectParser.ParseFirestoreProfileData(responseData.Response);
                        return personParsed;
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                return responseData;
            }
        }
        public async Task<List<PersonDto>> GetAllPersons(string bearerToken)
        {
            List<PersonDto> persons = new List<PersonDto>();
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
                        persons = FirestoreObjectParser.ParseFirestorePersonsData(responseData.Response);
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                return persons;
            }
        }
        public async Task<HttpCallResponseData> UpdateAPerson(PersonDto personToUpdate, string bearerToken)
        {
            using (HttpClient client = new HttpClient())
            {
                Dictionary<string, dynamic> requestParameters;

                requestParameters = new Dictionary<string, dynamic>
                {
                    { "lastName", personToUpdate.LastName },
                    { "firstName", personToUpdate.FirstName },
                    { "email", personToUpdate.Email }
                };
                if (personToUpdate.Age is not null)
                {
                    requestParameters.Add("age", personToUpdate.Age.Value);
                }

                if (personToUpdate.Address is not null)
                {
                    var addressDictionary = new Dictionary<string, dynamic>();
                    if (personToUpdate.Address.StreetName is not null)
                    {
                        addressDictionary.Add("streetName", personToUpdate.Address.StreetName);
                    }
                    if (personToUpdate.Address.County is not null)
                    {
                        addressDictionary.Add("county", personToUpdate.Address.County);
                    }
                    if (personToUpdate.Address.City is not null)
                    {
                        addressDictionary.Add("city", personToUpdate.Address.City);
                    }

                    requestParameters.Add("address", addressDictionary);
                }


                string jsonBody = JsonConvert.SerializeObject(requestParameters);

                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

                HttpCallResponseData responseData = new HttpCallResponseData();
                try
                {
                    HttpResponseMessage response = await client.PatchAsync(TezorwasApiHelper!.ApiUrl + "/" + personToUpdate.Id, content);

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
