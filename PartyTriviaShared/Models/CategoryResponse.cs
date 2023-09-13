using Newtonsoft.Json;

namespace PartyTriviaShared.Models
{
    internal class CategoryResponse
    {
        [JsonProperty("trivia_categories")]
        public TriviaCategory[] TriviaCategories { get; set; }
    }
}
