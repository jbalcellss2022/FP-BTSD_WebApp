using BusinessLogicLayer.Interfaces;
using Entities.DTOs;
using Microsoft.Extensions.Configuration;
using NLog;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace BusinessLogicLayer.Services
{
    public class PromptService(IConfiguration Configuration) : IPromptService
    {
        public async Task<string> TriggerPromptOpenAI(string question)
        {
            var apiKey = Configuration["OpenAISettings:APIKey"];
            //var apiKey = Configuration["OpenAISettings:DummyKey"];
            var baseUrl = Configuration["OpenAISettings:BaseUrl"];
            string? responseText;
            try
            {
                HttpClient client = new()
                {
                    BaseAddress = new Uri(baseUrl!),
                    DefaultRequestHeaders = { Authorization = new AuthenticationHeaderValue("Bearer", apiKey) }
                };

                var request = new OpenAIRequestDto
                {
                    Model = "gpt-3.5-turbo",
                    Messages = new List<OpenAIMessageRequestDto>{
                    new() {
                        Role = "user",
                        Content = question
                    }
                },
                    MaxTokens = 100
                };
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(baseUrl, content);
                var resjson = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = JsonSerializer.Deserialize<OpenAIErrorResponseDto>(resjson);
                    throw new Exception(errorResponse!.Error!.Message);
                }
                var data = JsonSerializer.Deserialize<OpenAIResponseDto>(resjson);
                responseText = data!.choices![0]!.message!.content;
            }
            catch (Exception ex)
            {
                Logger logger = LogManager.GetCurrentClassLogger();
                logger.Error(ex, "Error in TriggerPromptOpenAI");
                responseText = "Sorry, I am not able to answer that question right now.";
            }

            return responseText!;
        }
    }
}
