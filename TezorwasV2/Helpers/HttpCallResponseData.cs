
namespace TezorwasV2.Helpers
{
    public class HttpCallResponseData
    {
        public int StatusCode { get; set; }
        public string Response { get; set; } = string.Empty;

        public string GetKeyValue(string keyName)
        {
            JsonDocument jsonDocument = JsonDocument.Parse(Response);

            JsonElement root = jsonDocument.RootElement;

            JsonElement keyValue = root.GetProperty(keyName);

            if (keyValue.GetString() != null)
            {
#pragma warning disable CS8603 // Possible null reference return.
                return keyValue.GetString();
#pragma warning restore CS8603 // Possible null reference return.
            }

            return "The key provided does not exist";
        }

        public string GetDataDictionaryValue(string keyName)
        {
            JsonDocument jsonDocument = JsonDocument.Parse(Response);

            JsonElement root = jsonDocument.RootElement;

            JsonElement dataDictionary = root.GetProperty("data");

            JsonElement keyValue = dataDictionary.GetProperty(keyName);

            if (keyValue.GetString() != null)
            {
#pragma warning disable CS8603 // Possible null reference return.
                return keyValue.GetString();
#pragma warning restore CS8603 // Possible null reference return.
            }

            return "The key provided does not exist";
        }
    }
}
