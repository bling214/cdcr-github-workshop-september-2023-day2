using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using PartyTriviaShared.Models;
using Newtonsoft.Json;

namespace PartyTriviaShared.Services
{
    public class OpenTriviaDbService
    {
        private const string QuestionBaseUrl = "https://opentdb.com/api.php";
        private const string CategoryBaseUrl = "https://opentdb.com/api_category.php";
        private const string TokenBaseUrl = "https://opentdb.com/api_token.php";

        public OpenTriviaDbService()
        {
        }

        public string SessionToken { get; private set; }

        public async Task<TriviaCategory[]> GetCategoriesAsync()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage res = await client.GetAsync(CategoryBaseUrl);

            string content = await res.Content.ReadAsStringAsync();
            CategoryResponse apiResponse = JsonConvert.DeserializeObject<CategoryResponse>(content);
            return apiResponse.TriviaCategories;
        }

        public async Task<Question[]> GetQuestionsAsync(int numberOfQuestions, int? category = null, OpenTriviaDbEnums.QuestionType? questionType = null, OpenTriviaDbEnums.Difficulty? difficulty = null)
        {
            if (string.IsNullOrEmpty(this.SessionToken))
            {
                await this.CreateSessionTokenAsync();
            }

            HttpClient client = new HttpClient();
            string query = this.GetQuestionQueryString(numberOfQuestions, category, questionType, difficulty);
            string url = $"{QuestionBaseUrl}?{query}";
            HttpResponseMessage res = await client.GetAsync(url);

            string content = await res.Content.ReadAsStringAsync();
            QuestionResponse apiResponse = JsonConvert.DeserializeObject<QuestionResponse>(content);
            return apiResponse.Results;
        }

        public async Task ResetSessionTokenAsync()
        {
            if (string.IsNullOrEmpty(this.SessionToken))
            {
                await this.CreateSessionTokenAsync();
                return;
            }

            HttpClient client = new HttpClient();
            string url = $"{TokenBaseUrl}?command=reset&token={this.SessionToken}";
            HttpResponseMessage res = await client.GetAsync(url);
        }

        public void SetSessionToken(string token)
        {
            this.SessionToken = token;
        }

        public async Task CreateSessionTokenAsync()
        {
            HttpClient client = new HttpClient();
            string url = $"{TokenBaseUrl}?command=request";
            HttpResponseMessage res = await client.GetAsync(url);

            string content = await res.Content.ReadAsStringAsync();
            TokenResponse apiResponse = JsonConvert.DeserializeObject<TokenResponse>(content);

            this.SessionToken = apiResponse.Token;
        }

        private string GetQuestionQueryString(int numberOfQuestions, int? category, OpenTriviaDbEnums.QuestionType? questionType, OpenTriviaDbEnums.Difficulty? difficulty)
        {
            List<string> query = new List<string> { $"amount={numberOfQuestions}" };

            if (category.HasValue)
            {
                query.Add($"category={category}");
            }

            switch (questionType)
            {
                case OpenTriviaDbEnums.QuestionType.MultiChoice:
                    query.Add("type=multiple");
                    break;
                case OpenTriviaDbEnums.QuestionType.TrueOrFalse:
                    query.Add("type=boolean");
                    break;
                case null:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(questionType), questionType, null);
            }

            switch (difficulty)
            {
                case OpenTriviaDbEnums.Difficulty.Easy:
                    query.Add("difficulty=easy");
                    break;
                case OpenTriviaDbEnums.Difficulty.Medium:
                    query.Add("difficulty=medium");
                    break;
                case OpenTriviaDbEnums.Difficulty.Hard:
                    query.Add("difficulty=hard");
                    break;
                case null:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null);
            }

            if (!string.IsNullOrEmpty(this.SessionToken))
            {
                query.Add($"token={this.SessionToken}");
            }

            string queryString = string.Join("&", query);
            return queryString;
        }
    }
}
