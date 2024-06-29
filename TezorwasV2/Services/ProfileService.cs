using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using TezorwasV2.DTO;
using TezorwasV2.Helpers;
using TezorwasV2.Model;

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
                    //todo de facut la fel ca la habbits atunci cand adaugam achievments
                }

                if (profileToCreate.Habbits is not null)
                {
                    var habbitsList = new List<Dictionary<string, dynamic>>();
                    foreach (HabbitModel habbit in profileToCreate.Habbits)
                    {
                        var habbitsDictionary = new Dictionary<string, dynamic>();
                        if (habbit.Description is not null)
                        {
                            habbitsDictionary.Add("description", habbit.Description);
                        }

                        habbitsDictionary.Add("levelOfWaste", habbit.LevelOfWaste);
                        habbitsDictionary.Add("inputDate", habbit.InputDate);
                        habbitsList.Add(habbitsDictionary);
                    }
                    requestParameters.Add("habbits", habbitsList);
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
                        profiles = FirestoreObjectParser.ParseFirestoreProfilesData(responseData.Response); ;
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
                    var habbitsList = new List<Dictionary<string, dynamic>>();
                    foreach (HabbitModel habbit in profileToUpdate.Habbits)
                    {
                        var habbitsDictionary = new Dictionary<string, dynamic>();
                        if (habbit.Description is not null)
                        {
                            habbitsDictionary.Add("description", habbit.Description);
                        }

                        habbitsDictionary.Add("levelOfWaste", habbit.LevelOfWaste);
                        habbitsDictionary.Add("inputDate", habbit.InputDate);
                        habbitsList.Add(habbitsDictionary);
                    }
                    requestParameters.Add("habbits", habbitsList);
                }
                if (profileToUpdate.Tasks is not null)
                {
                    var tasksList = new List<Dictionary<string, dynamic>>();
                    foreach (TaskModel task in profileToUpdate.Tasks)
                    {
                        var tasksDictionary = new Dictionary<string, dynamic>();
                        if (task.Description is not null)
                        {
                            tasksDictionary.Add("description", task.Description);
                        }
                        tasksDictionary.Add("isCompleted", task.IsCompleted);
                        if (task.Name is not null)
                        {
                            tasksDictionary.Add("name", task.Name);
                        }
                        tasksDictionary.Add("xpEarned", task.XpEarned);
                        tasksDictionary.Add("creationDate", task.CreationDate);
                        tasksDictionary.Add("completionDate", task.CompletionDate);

                        tasksList.Add(tasksDictionary);
                    }
                    requestParameters.Add("tasks", tasksList);
                }
                if (profileToUpdate.Receipts is not null)
                {
                    var receiptList = new List<Dictionary<string, dynamic>>();
                    foreach (ReceiptModel receipt in profileToUpdate.Receipts)
                    {
                        var receiptDictionary = new Dictionary<string, dynamic>();
                        if (receipt.Id is not null)
                        {
                            receiptDictionary.Add("id", receipt.Id);
                        }
                        receiptDictionary.Add("inputDate", receipt.CreationDate);
                        receiptDictionary.Add("completionDate", receipt.CompletionDate);
                        if(receipt.Name is not null)
                        {
                            receiptDictionary.Add("name",receipt.Name);
                        }
                        else
                        {
                            receiptDictionary.Add("name", "Receipt" + receipt.CreationDate.ToString("dd.MM.yyyy"));

                        }


                        var receiptItemsList = new List<Dictionary<string, dynamic>>();
                        if (receipt.ReceiptItems is not null)
                        {

                            foreach (ReceiptItemModel receiptItem in receipt.ReceiptItems)
                            {
                                var receiptItems = new Dictionary<string, dynamic>();
                                receiptItems.Add("creationDate", receiptItem.CreationDate);
                                receiptItems.Add("completionDate", receiptItem.CompletionDate);
                                if (receiptItem.Name is not null)
                                {
                                    receiptItems.Add("name", receiptItem.Name);
                                }
                                if (receiptItem.Id is not null)
                                {
                                    receiptItems.Add("id", receiptItem.Id);
                                }
                                receiptItems.Add("xpEarned", receiptItem.XpEarned);
                                receiptItems.Add("isRecycled", receiptItem.IsRecycled);
                                receiptItemsList.Add(receiptItems);
                            }
                        }
                        receiptDictionary.Add("items", receiptItemsList);
                        receiptList.Add(receiptDictionary);
                    }
                    requestParameters.Add("receipts", receiptList);
                }

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
