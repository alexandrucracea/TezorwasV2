using System.Text.Json.Nodes;
using TezorwasV2.DTO;
using TezorwasV2.Model;

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

            for (int i = 0; i < choices.Count; i++)
            {
                JsonObject deserializedChoice = choices[i].Deserialize<JsonObject>();
                completionsToParse += deserializedChoice["message"]["content"].ToString();
            }

            //trebuie parsat jsonul in string si el si dupa aplicat ce e aici
            string[] responsedSplitted = completionsToParse.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string completion in responsedSplitted)
            {
                CompletionsDto taskParsed = new CompletionsDto();
                int finalWordInTaskIndex = completion.LastIndexOf("xp");
                if (finalWordInTaskIndex >= 0)
                {
                    int xpEarned = int.Parse(completion.Substring(finalWordInTaskIndex - 2, 2));
                    string description = completion.Substring(0, finalWordInTaskIndex - 5).Trim();

                    taskParsed.TaskDescription = description;
                    taskParsed.XpEarned = xpEarned;

                    tasksParsed.Add(taskParsed);
                }
            }

            return tasksParsed;
        }
        public static ReceiptModel ParseGptReceiptTasksModel(string receiptToParse)
        {
            ReceiptModel receipt = new ReceiptModel();

            // Parse the JSON content from the string
            JsonDocument jsonDocument = JsonDocument.Parse(receiptToParse);
            JsonElement jsonRoot = jsonDocument.RootElement;

            JsonElement choices = jsonRoot.GetProperty("choices");
            receiptToParse = string.Empty;

            foreach (JsonElement choice in choices.EnumerateArray())
            {
                JsonElement message = choice.GetProperty("message");
                receiptToParse += message.GetProperty("content").GetString();
            }

            // Separate each line and create ReceiptItemModel objects
            string[] responsedSplitted = receiptToParse.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            List<ReceiptItemModel> receiptItems = new List<ReceiptItemModel>();

            foreach (string itemTask in responsedSplitted)
            {
                // Remove the number and period from the beginning
                string cleanedTask = itemTask.Substring(itemTask.IndexOf('.') + 1).Trim();

                ReceiptItemModel itemToRecycle = new ReceiptItemModel
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = cleanedTask,
                    XpEarned = new Random().Next(1, 6),
                    CreationDate = DateTime.Now,
                    CompletionDate = DateTime.Now,
                    IsRecycled = false
                };

                receiptItems.Add(itemToRecycle);
            }

            receipt.Id = Guid.NewGuid().ToString();
            receipt.CreationDate = DateTime.Now;
            receipt.CompletionDate = DateTime.Now;
            receipt.ReceiptItems = receiptItems;

            return receipt;
        }

    }
}

