using System.IO;
using Newtonsoft.Json;

namespace BetfairNG
{
    public static class JsonConvert
    {
        public static JsonResponse<T> Import<T>(TextReader reader)
        {
            var jsonResponse = reader.ReadToEnd();
            return Deserialize<JsonResponse<T>>(jsonResponse);
        }

        public static T Deserialize<T>(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }

        public static void Export(JsonRequest request, TextWriter writer)
        {
            var json = Serialize(request);
            writer.Write(json);
        }

        public static string Serialize<T>(T value)
        {
            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            return Newtonsoft.Json.JsonConvert.SerializeObject(value, settings);
        }
    }
}