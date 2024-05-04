
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
                JsonElement inputDate = mapValue.GetProperty("InputDate").GetProperty("stringValue");
                JsonElement description = mapValue.GetProperty("Description").GetProperty("stringValue");
                JsonElement levelOfWaste = mapValue.GetProperty("LevelOfWaste").GetProperty("doubleValue");


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
                    completionDate = DateTime.Parse(completionDate.GetString()!),
                    creationDate = DateTime.Parse(creationDate.GetString()!),
                    XpEarned = int.Parse(xpEarned.GetString()!),
                    IsCompleted = isCompleted.GetBoolean(),
                });
            }
            return profileToParse;
        }
        //public static PersonDto ParseFirestorePersonData(dynamic personObject)
        //{
        //    PersonDto personToParse = new PersonDto();

        //    JsonDocument jsonDocument = JsonDocument.Parse(personObject);
        //    JsonElement root = jsonDocument.RootElement;


        //    JsonElement id = root.GetProperty("_fieldsProto").GetProperty("id").GetProperty("stringValue");
        //    profileToParse.Id = id.ToString();

        //    JsonElement payload = root.GetProperty("_fieldsProto").GetProperty("payload").GetProperty("mapValue").GetProperty("fields");

        //    JsonElement joinDate = payload.GetProperty("joinDate").GetProperty("stringValue");
        //    profileToParse.JoinDate = joinDate.GetDateTime();

        //    JsonElement level = payload.GetProperty("level").GetProperty("integerValue");
        //    profileToParse.Level = int.Parse(level.GetString()!);

        //    JsonElement xp = payload.GetProperty("xp").GetProperty("integerValue");
        //    profileToParse.Xp = int.Parse(xp.GetString()!);

        //    JsonElement personId = payload.GetProperty("personId").GetProperty("stringValue");
        //    if (personId.GetString() is not null)
        //    {
        //        profileToParse.PersonId = personId.GetString()!;
        //    }


        //    profileToParse.Habbits = new List<HabbitModel>();
        //    JsonElement habbits = payload.GetProperty("habbits").GetProperty("arrayValue").GetProperty("values");

        //    foreach (var item in habbits.EnumerateArray())
        //    {
        //        JsonElement mapValue = item.GetProperty("mapValue").GetProperty("fields");
        //        JsonElement inputDate = mapValue.GetProperty("InputDate").GetProperty("stringValue");
        //        JsonElement description = mapValue.GetProperty("Description").GetProperty("stringValue");
        //        JsonElement levelOfWaste = mapValue.GetProperty("LevelOfWaste").GetProperty("doubleValue");


        //        profileToParse.Habbits.Add(new HabbitModel
        //        {
        //            InputDate = DateTime.Parse(inputDate.GetString()!),
        //            Description = description.ToString(),
        //            LevelOfWaste = levelOfWaste.GetDouble(),
        //        });
        //    }


        //    profileToParse.Tasks = new List<TaskModel>();
        //    JsonElement tasks = payload.GetProperty("tasks").GetProperty("arrayValue").GetProperty("values");
        //    foreach (var item in tasks.EnumerateArray())
        //    {
        //        JsonElement mapValue = item.GetProperty("mapValue").GetProperty("fields");
        //        JsonElement name = mapValue.GetProperty("name").GetProperty("stringValue");
        //        JsonElement description = mapValue.GetProperty("description").GetProperty("stringValue");
        //        JsonElement completionDate = mapValue.GetProperty("completionDate").GetProperty("stringValue");
        //        JsonElement creationDate = mapValue.GetProperty("creationDate").GetProperty("stringValue");
        //        JsonElement xpEarned = mapValue.GetProperty("xpEarned").GetProperty("integerValue");
        //        JsonElement isCompleted = mapValue.GetProperty("isCompleted").GetProperty("booleanValue");


        //        profileToParse.Tasks.Add(new TaskModel
        //        {
        //            Name = name.ToString(),
        //            Description = description.ToString(),
        //            completionDate = DateTime.Parse(completionDate.GetString()!),
        //            creationDate = DateTime.Parse(creationDate.GetString()!),
        //            XpEarned = int.Parse(xpEarned.GetString()!),
        //            IsCompleted = isCompleted.GetBoolean(),
        //        });
        //    }
        //    return profileToParse;
        //}
    }
}
