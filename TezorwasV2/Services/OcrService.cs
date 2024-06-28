using Google.Cloud.Vision.V1;
using Google.Apis.Auth.OAuth2;
using Grpc.Auth;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq;

public class OcrService
{
    private readonly ImageAnnotatorClient _client;

    public OcrService()
    {
        var configuration = GetConfiguration();
        var jsonCredentials = configuration.GetSection("GoogleCloud:ServiceAccount").Get<Dictionary<string, string>>();
        var credential = GoogleCredential.FromJson(Newtonsoft.Json.JsonConvert.SerializeObject(jsonCredentials))
                                          .CreateScoped(ImageAnnotatorClient.DefaultScopes);

        _client = new ImageAnnotatorClientBuilder
        {
            JsonCredentials = Newtonsoft.Json.JsonConvert.SerializeObject(jsonCredentials)
        }.Build();
    }

    private IConfiguration GetConfiguration()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var stream = assembly.GetManifestResourceStream("TezorwasV2.appsettings.json");

        return new ConfigurationBuilder()
            .AddJsonStream(stream)
            .Build();
    }

    public async Task<string> ExtractTextFromImageAsync(string imagePath)
    {
        if (!File.Exists(imagePath))
        {
            throw new FileNotFoundException($"File not found: {imagePath}");
        }

        using (var fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
        {
            var image = Google.Cloud.Vision.V1.Image.FromStream(fileStream);
            var response = await _client.DetectTextAsync(image);
            var textAnnotations = response;

            return string.Join("\n", textAnnotations.Select(annotation => annotation.Description));
        }
    }
}
