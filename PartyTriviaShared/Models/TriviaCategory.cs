using Newtonsoft.Json;

namespace PartyTriviaShared.Models
{
    public class TriviaCategory
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
