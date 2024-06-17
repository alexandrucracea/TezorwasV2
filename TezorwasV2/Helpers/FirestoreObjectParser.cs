using System.Text.Json.Nodes;
using TezorwasV2.DTO;
using TezorwasV2.Model;

namespace TezorwasV2.Helpers
{
    public static class FirestoreObjectParser
    {
        public static ProfileDto ParseFirestoreProfileData(dynamic profileObject)
        {
            ProfileDto profileToParse = new ProfileDto();

            JsonDocument jsonDocument = JsonDocument.Parse(profileObject);
            JsonElement root = jsonDocument.RootElement;


            JsonElement id = root.GetProperty("_fieldsProto").GetProperty("id").GetProperty("stringValue");
            profileToParse.Id = id.ToString();

            JsonElement payload = root.GetProperty("_fieldsProto").GetProperty("payload").GetProperty("mapValue").GetProperty("fields");

            JsonElement joinDate = payload.GetProperty("joinDate").GetProperty("stringValue");
            profileToParse.JoinDate = joinDate.GetDateTime();

            JsonElement level = payload.GetProperty("level").GetProperty("integerValue");
            profileToParse.Level = int.Parse(level.GetString()!);

            JsonElement xp = payload.GetProperty("xp").GetProperty("integerValue");
            profileToParse.Xp = int.Parse(xp.GetString()!);

            JsonElement personId = payload.GetProperty("personId").GetProperty("stringValue");
            if (personId.GetString() is not null)
            {
                profileToParse.PersonId = personId.GetString()!;
            }


            profileToParse.Habbits = new List<HabbitModel>();
            JsonElement habbits = payload.GetProperty("habbits").GetProperty("arrayValue").GetProperty("values");

            foreach (var item in habbits.EnumerateArray())
            {
                JsonElement mapValue = item.GetProperty("mapValue").GetProperty("fields");
                JsonElement inputDate = mapValue.GetProperty("inputDate").GetProperty("stringValue");
                JsonElement description = mapValue.GetProperty("description").GetProperty("stringValue");
                JsonElement levelOfWaste = mapValue.GetProperty("levelOfWaste").GetProperty("doubleValue");


                profileToParse.Habbits.Add(new HabbitModel
                {
                    InputDate = DateTime.Parse(inputDate.GetString()!),
                    Description = description.ToString(),
                    LevelOfWaste = levelOfWaste.GetDouble(),
                });
            }


            profileToParse.Tasks = new List<TaskModel>();
            JsonElement tasks = payload.GetProperty("tasks").GetProperty("arrayValue").GetProperty("values");
            foreach (var item in tasks.EnumerateArray())
            {
                JsonElement mapValue = item.GetProperty("mapValue").GetProperty("fields");
                JsonElement name = mapValue.GetProperty("name").GetProperty("stringValue");
                JsonElement description = mapValue.GetProperty("description").GetProperty("stringValue");
                JsonElement completionDate = mapValue.GetProperty("completionDate").GetProperty("stringValue");
                JsonElement creationDate = mapValue.GetProperty("creationDate").GetProperty("stringValue");
                JsonElement xpEarned = mapValue.GetProperty("xpEarned").GetProperty("integerValue");
                JsonElement isCompleted = mapValue.GetProperty("isCompleted").GetProperty("booleanValue");


                profileToParse.Tasks.Add(new TaskModel
                {
                    Name = name.ToString(),
                    Description = description.ToString(),
                    CompletionDate = DateTime.Parse(completionDate.GetString()!),
                    CreationDate = DateTime.Parse(creationDate.GetString()!),
                    XpEarned = int.Parse(xpEarned.GetString()!),
                    IsCompleted = isCompleted.GetBoolean(),
                });
            }
            profileToParse.Receipts = new List<ReceiptModel>();
            JsonElement receipts = payload.GetProperty("receipts").GetProperty("arrayValue").GetProperty("values");

            foreach(var item in receipts.EnumerateArray())
            {
                JsonElement mapValue = item.GetProperty("mapValue").GetProperty("fields");
                JsonElement idReceipt = mapValue.GetProperty("id").GetProperty("stringValue");
                JsonElement completionDate = mapValue.GetProperty("completionDate").GetProperty("stringValue");
                JsonElement inputDate = mapValue.GetProperty("inputDate").GetProperty("stringValue");

                List<ReceiptItemModel> itemsToAdd = new List<ReceiptItemModel>();
                JsonElement itemsToParse = mapValue.GetProperty("items").GetProperty("arrayValue").GetProperty("values");
                foreach(var itm in itemsToParse.EnumerateArray())
                {
                    JsonElement itemMapValue = itm.GetProperty("mapValue").GetProperty("fields");
                    JsonElement isRecycled = itemMapValue.GetProperty("isRecycled").GetProperty("booleanValue");
                    JsonElement idItem = itemMapValue.GetProperty("id").GetProperty("stringValue");
                    JsonElement itemName = itemMapValue.GetProperty("name").GetProperty("stringValue");
                    JsonElement itemCompletionDate = itemMapValue.GetProperty("completionDate").GetProperty("stringValue");
                    JsonElement itemCreationDate = itemMapValue.GetProperty("creationDate").GetProperty("stringValue");
                    JsonElement xpEarned = itemMapValue.GetProperty("xpEarned").GetProperty("integerValue");

                    itemsToAdd.Add(new ReceiptItemModel
                    {
                        Id = idItem.ToString(),
                        CompletionDate = DateTime.Parse(itemCompletionDate.GetString()!),
                        CreationDate = DateTime.Parse(itemCreationDate.GetString()!),
                        IsRecycled = isRecycled.GetBoolean(),
                        Name = itemName.ToString(),
                        XpEarned = int.Parse(xpEarned.ToString())
                    });
                }

                profileToParse.Receipts.Add(new ReceiptModel
                {
                    Id = idReceipt.ToString(),
                    CompletionDate = DateTime.Parse(completionDate.GetString()!),
                    CreationDate = DateTime.Parse(inputDate.GetString()!),
                    ReceiptItems = itemsToAdd
                });
            }

            return profileToParse;
        }
        public static List<PersonDto> ParseFirestorePersonsData(dynamic personObject)
        {
            List<PersonDto> persons = new List<PersonDto>();


            JsonDocument jsonDocument = JsonDocument.Parse(personObject);
            JsonArray roots = jsonDocument.Deserialize<JsonArray>();

            foreach (JsonObject obj in roots.Cast<JsonObject>())
            {
                PersonDto personToParse = new PersonDto();
                var root = obj["_fieldsProto"];
                personToParse.Id = root["id"]["stringValue"].ToString();

                var payload = root["payload"]["mapValue"]["fields"];

                personToParse.FirstName = payload["firstName"]["stringValue"].ToString();
                personToParse.LastName = payload["lastName"]["stringValue"].ToString();
                personToParse.Age = int.Parse(payload["age"]["integerValue"].ToString());
                personToParse.Email = payload["email"]["stringValue"].ToString();

                personToParse.Address = new AddressModel();
                var address = payload["address"]["mapValue"]["fields"];
                var streetName = address["streetName"]["stringValue"].ToString();
                var city = address["city"]["stringValue"].ToString();
                var county = address["county"]["stringValue"].ToString();

                personToParse.Address.StreetName = streetName;
                personToParse.Address.City = city;
                personToParse.Address.County = county;

                persons.Add(personToParse);

            }
            return persons;
        }
        public static PersonDto ParseFirestoreSinglePersonData(dynamic personObject)
        {
            PersonDto personToParse = new PersonDto();

            JsonDocument jsonDocument = JsonDocument.Parse(personObject);
            JsonElement root = jsonDocument.RootElement;


            JsonElement id = root.GetProperty("_fieldsProto").GetProperty("id").GetProperty("stringValue");
            personToParse.Id = id.ToString();

            JsonElement payload = root.GetProperty("_fieldsProto").GetProperty("payload").GetProperty("mapValue").GetProperty("fields");

            JsonElement lastName = payload.GetProperty("lastName").GetProperty("stringValue");
            personToParse.LastName = lastName.ToString();

            JsonElement firstName = payload.GetProperty("firstName").GetProperty("stringValue");
            personToParse.FirstName = firstName.ToString();

            JsonElement email = payload.GetProperty("email").GetProperty("stringValue");
            personToParse.Email = email.ToString();

            JsonElement age = payload.GetProperty("age").GetProperty("integerValue");
            personToParse.Age = int.Parse(age.GetString()!);

            JsonElement address = payload.GetProperty("address").GetProperty("mapValue").GetProperty("fields");
            JsonElement streetName =  address.GetProperty("streetName").GetProperty("stringValue");
            JsonElement city = address.GetProperty("city").GetProperty("stringValue");
            JsonElement county = address.GetProperty("county").GetProperty("stringValue");


            AddressModel addressToAdd = new AddressModel
            {
                StreetName = streetName.ToString(),
                City = city.ToString(),
                County = county.ToString()
            };
            personToParse.Address = addressToAdd;

            return personToParse;
        }
        public static List<ProfileDto> ParseFirestoreProfilesData(dynamic profilesObject)
        {
            List<ProfileDto> profiles = new List<ProfileDto>();

            JsonDocument jsonDocument = JsonDocument.Parse(profilesObject);
            JsonArray roots = jsonDocument.Deserialize<JsonArray>();

            foreach (JsonObject obj in roots.Cast<JsonObject>())
            {
                ProfileDto profileToParse = new ProfileDto();
                var root = obj["_fieldsProto"];
                profileToParse.Id = root["id"]["stringValue"].ToString();

                var payload = root["payload"]["mapValue"]["fields"];

                profileToParse.JoinDate = DateTime.Parse(payload["joinDate"]["stringValue"].ToString());
                profileToParse.Level = int.Parse(payload["level"]["integerValue"].ToString());
                profileToParse.Xp = int.Parse(payload["xp"]["integerValue"].ToString());
                if (payload["personId"]["stringValue"].ToString() is not null)
                {
                    profileToParse.PersonId = payload["personId"]["stringValue"].ToString();
                }

                profileToParse.Habbits = new List<HabbitModel>();
                if (payload["habbits"]["arrayValue"]["values"].ToString() is not null)
                {
                    JsonArray habbitsArray = (JsonArray)payload["habbits"]["arrayValue"]["values"];
                    foreach (JsonObject habbitObj in habbitsArray.Cast<JsonObject>().ToArray())
                    {
                        HabbitModel habbitToAdd = new HabbitModel();
                        habbitToAdd.InputDate = DateTime.Parse(habbitObj["mapValue"]["fields"]["inputDate"]["stringValue"].ToString());
                        habbitToAdd.Description = habbitObj["mapValue"]["fields"]["description"]["stringValue"].ToString();
                        habbitToAdd.LevelOfWaste = double.Parse(habbitObj["mapValue"]["fields"]["levelOfWaste"]["doubleValue"].ToString());

                        profileToParse.Habbits.Add(habbitToAdd);
                    }

                }

                profileToParse.Tasks = new List<TaskModel>();
                if (payload["tasks"]["arrayValue"]["values"].ToString() is not null)
                {
                    JsonArray tasksArray = (JsonArray)payload["tasks"]["arrayValue"]["values"];
                    foreach (JsonObject taskObj in tasksArray.Cast<JsonObject>().ToArray())
                    {
                        TaskModel taskToAdd = new TaskModel();
                        taskToAdd.Name = taskObj["mapValue"]["fields"]["name"]["stringValue"].ToString();
                        taskToAdd.Description = taskObj["mapValue"]["fields"]["description"]["stringValue"].ToString();
                        taskToAdd.CompletionDate = DateTime.Parse(taskObj["mapValue"]["fields"]["completionDate"]["stringValue"].ToString());
                        taskToAdd.CreationDate = DateTime.Parse(taskObj["mapValue"]["fields"]["creationDate"]["stringValue"].ToString());
                        taskToAdd.XpEarned = int.Parse(taskObj["mapValue"]["fields"]["xpEarned"]["integerValue"].ToString());
                        taskToAdd.IsCompleted = bool.Parse(taskObj["mapValue"]["fields"]["isCompleted"]["booleanValue"].ToString());


                        profileToParse.Tasks.Add(taskToAdd);
                    }

                }

                profiles.Add(profileToParse);
            }
            return profiles;
        }
        public static List<ArticleModel> ParseFirestoreArticlesData(dynamic articlesObject)
        {
            List<ArticleModel> articles = new List<ArticleModel>();

            JsonDocument jsonDocument = JsonDocument.Parse(articlesObject);
            JsonArray roots = jsonDocument.Deserialize<JsonArray>();

            foreach (JsonObject obj in roots.Cast<JsonObject>())
            {
                ArticleModel articleToParse = new ArticleModel();
                var root = obj["_fieldsProto"];
                articleToParse.Id = root["id"]["stringValue"].ToString();

                var payload = root["payload"]["mapValue"]["fields"];

                articleToParse.DatePublished = DateTime.Parse(payload["datePublished"]["stringValue"].ToString());
                articleToParse.Title = payload["title"]["stringValue"].ToString();
                articleToParse.Content = payload["content"]["stringValue"].ToString();
                articleToParse.CoverUrl = payload["coverLink"]["stringValue"].ToString();
               
                articles.Add(articleToParse);
            }
            return articles;
        }

    }
}
