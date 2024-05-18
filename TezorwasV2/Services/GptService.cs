

using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using TezorwasV2.Helpers;

namespace TezorwasV2.Services
{
    public class GptService
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
        public async Task<dynamic> GenerateTasks(int levelOfWaste, string bearerToken)
        {
            List<string> tasksGenerated = new List<string>();
            string responseContent = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

                HttpCallResponseData responseData = new HttpCallResponseData();
                try
                {

                    string prompt = $"Stiind ca utilizatorul aplicatiei mele poate fi incadrat intr-o categorie de poluare cu o gravitate de la 1 la 5 si acesta are numarul {levelOfWaste}," +
                                    $" genereaza o sarcina de reciclare pe care o poate realiza in viata de zi cu zi cu usurinta pentru a adopta un comportament mai putin  daunator. Sarcina " +
                                    $"ar trebui sa fie destul de scurta si intuitiva. Vreau sa imi dai doar actiunea pe care acesta ar trebui sa o realizeze. Limiteaza-te la maxim 7 cuvinte. " +
                                    $"Sarcina trebuie sa includa si ce produs sau ce parte a produsului trebuie reciclata.Fiecare sarcina generata trebuie sa fie diferita.Genereaza 3 sarcini," +
                                    $" fiecare sarcina fiind separata prin separatorul: -. Fiecare sarcina ar trebui sa fie insotita cu un numar de la 1 la 10 (numarulreprezinta punctele de " +
                                    $"experiente capatate de utilizator prin indeplinirea taskului. Genereaza in engleza adaugand dupa fiecare numar cuvantul points";

                    var requestParameters = new Dictionary<string, string>
                    {
                        {"prompt",prompt }
                    };
                    var jsonBody = JsonConvert.SerializeObject(requestParameters);
                    var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(TezorwasApiHelper!.ApiUrl, content);
                    responseData.StatusCode = (int)response.StatusCode;
                    responseData.Response = await response.Content.ReadAsStringAsync();
                    if (responseData.StatusCode == (int)Enums.StatusCodes.Success)
                    {
                       responseContent =  responseData.Response;
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
}
