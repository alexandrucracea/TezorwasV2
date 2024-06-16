

using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using TezorwasV2.Helpers;

namespace TezorwasV2.Services
{
    public class GptService : IGptService
    {
        IConfiguration configuration;
        public TezorwasApiHelper TezorwasApiHelper { get; set; }

        public GptService()
        {
            configuration = MauiProgram.Services.GetService<IConfiguration>()!;
            var settings = configuration!.GetRequiredSection("Settings").Get<Settings>();
            if (settings is not null)
            {
                string tezorwasApiUrl = settings.TezorwasApiUrl + "/" + Enums.Paths.completions;
                TezorwasApiHelper = new TezorwasApiHelper(apiUrl: tezorwasApiUrl);
            }
        }
        public async Task<dynamic> GenerateTasks(double levelOfWaste, string bearerToken)
        {
            List<string> tasksGenerated = new List<string>();
            string responseContent = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

                HttpCallResponseData responseData = new HttpCallResponseData();
                try
                {

                    string prompt = $"User category: {levelOfWaste} pollution level (points out of 5). Generate three recycling tasks. Each task should be concise, intuitive, and include a specific number of products/parts to recycle. Tasks closer to pollution level 1 should be easier and involve fewer items, while tasks closer to pollution level 5 should be more challenging and involve more items. Tasks around level 3 should be of medium difficulty. Each task should be no longer than 7 words and specify the number of items to recycle and the experience points (0-20). Format the response as follows: Recycle: number of items product - numberOfExperiencePoints xp.";
                    RequestDto requestDto = new RequestDto();
                    requestDto.Temperature = 1;
                    requestDto.Messages.Add(new Messages
                    {
                        Role = "system",
                        Content = prompt
                    });


                    var requestBody = new
                    {
                        messages = new[]
           {
                new
                {
                    role = "system",
                    content = "User category: {pollution level} pollution level (points out of 5). Generate three recycling tasks. Each task should be concise, intuitive, and include a specific number of products/parts to recycle. Tasks closer to pollution level 1 should be easier and involve fewer items, while tasks closer to pollution level 5 should be more challenging and involve more items. Tasks around level 3 should be of medium difficulty. Each task should be no longer than 7 words and specify the number of items to recycle and the experience points (0-20). Format the response as follows: Recycle: number of items product - numberOfExperiencePoints xp."
                }
            },
                        temperature = 1
                    };
                    var requestParameters = new Dictionary<string, dynamic>
                    {
                        {"messages",requestDto.Messages},
                        { "temperature",requestDto.Temperature}
                    };
                    var jsonBody = JsonConvert.SerializeObject(requestBody);
                    var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(TezorwasApiHelper!.ApiUrl, content);
                    responseData.StatusCode = (int)response.StatusCode;
                    responseData.Response = await response.Content.ReadAsStringAsync();
                    if (responseData.StatusCode == (int)Enums.StatusCodes.Success)
                    {
                        responseContent = responseData.Response;
                        //trebuie parsat raspunsul si returnata lista
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());

                }
                return responseContent;
            }
        }
    }

    class RequestDto
    {
        public int Temperature { get; set; }
        public List<Messages> Messages { get; set; } = new List<Messages>();

    }
    class Messages
    {
        public string Role { get; set; }
        public string Content { get; set; }
    }
}
