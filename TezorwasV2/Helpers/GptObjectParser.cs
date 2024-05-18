using System.Text.Json.Nodes;
using TezorwasV2.DTO;

namespace TezorwasV2.Helpers
{
    public static class GptObjectParser
    {
        public static List<CompletionsDto> ParseGptCompletionsData(string completionsToParse)
        {
            List<CompletionsDto> tasksParsed = new List<CompletionsDto>();

            //parse the json content from the string
            JsonDocument jsonDocument = JsonDocument.Parse(completionsToParse);
            JsonElement jsonRoot = jsonDocument.RootElement;

            JsonArray choices = jsonRoot.GetProperty("choices").Deserialize<JsonArray>();
            completionsToParse = string.Empty;

            for(int i=0; i<choices.Count; i++)
            {
                JsonObject deserializedChoice = choices[i].Deserialize<JsonObject>();
                completionsToParse += deserializedChoice["text"].ToString();
            }

            //trebuie parsat jsonul in string si el si dupa aplicat ce e aici
            string[] responsedSplitted = completionsToParse.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string completion in responsedSplitted)
            {
                CompletionsDto taskParsed = new CompletionsDto();
                int finalWordInTaskIndex = completion.LastIndexOf("points");
                if (finalWordInTaskIndex >= 0)
                {
                    int xpEarned = int.Parse(completion.Substring(finalWordInTaskIndex - 2, 2));
                    string description = completion.Substring(2, finalWordInTaskIndex - 2).Trim();

                    taskParsed.TaskDescription = description;
                    taskParsed.XpEarned = xpEarned;

                    tasksParsed.Add(taskParsed);
                }
            }

            return tasksParsed;
        }
    }
}
