using Newtonsoft.Json;

namespace PartyTriviaShared.Models
{
    internal class TokenResponse
    {
        [JsonProperty("response_code")]
        public long ResponseCode { get; set; }

        [JsonProperty("response_message")]
        public string ResponseMessage { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
