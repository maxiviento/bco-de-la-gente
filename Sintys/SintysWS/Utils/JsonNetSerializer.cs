using System.IO;
using Newtonsoft.Json;
using RestSharp.Deserializers;
using RestSharp.Serializers.Newtonsoft.Json;

namespace SintysWS.Utils
{
    public class JsonNetSerializer : IDeserializer
    {
        private readonly JsonSerializer _serializer;
        public JsonNetSerializer(JsonSerializer serializer)
        {
            _serializer = serializer;
        }

        public string ContentType => "application/json";

        public string DateFormat { get; set; }

        public string Namespace { get; set; }

        public string RootElement { get; set; }

        public T Deserialize<T>(RestSharp.IRestResponse response)
        {
            var content = response.Content;

            using (var stringReader = new StringReader(content))
            {
                using (var jsonTextReader = new JsonTextReader(stringReader))
                {
                    return _serializer.Deserialize<T>(jsonTextReader);
                }
            }
        }

        public static NewtonsoftJsonSerializer Default => new NewtonsoftJsonSerializer(new JsonSerializer()
        {
            MissingMemberHandling = MissingMemberHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore
        });
    }
}