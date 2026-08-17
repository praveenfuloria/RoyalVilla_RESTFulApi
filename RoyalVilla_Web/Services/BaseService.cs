using RoyalVilla_DTO;
using RoyalVilla_Web.Models;
using RoyalVilla_Web.Services.IServices;
using System.Text.Json;

namespace RoyalVilla_Web.Services
{
    public class BaseService : IBaseServices
    {
        private readonly IHttpClientFactory _httpClient;
        private static readonly JsonSerializerOptions serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public ApiResponse<object> ResponseModel { get; set; }
        public BaseService(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
            ResponseModel = new();
        }

        public async Task<T?> SendAsync<T>(ApiRequest apiRequest)
        {
            try
            {
                var client = _httpClient.CreateClient("RoyalVillaAPI");
                var message = new HttpRequestMessage()
                {
                    RequestUri = new Uri(apiRequest.Url, uriKind: UriKind.Relative),
                    Method = GetHttpMethod(apiRequest.ApiType),

                };
                if (apiRequest.Data is not null)
                {
                    message.Content = JsonContent.Create(apiRequest.Data, options: serializerOptions);
                }
                var apiResponse = await client.SendAsync(message);
                return await apiResponse.Content.ReadFromJsonAsync<T>(serializerOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error {ex.Message}");
                return default;
            }
        }

        private HttpMethod GetHttpMethod(SD.ApiType apiType)
        {
            return apiType switch
            {
                SD.ApiType.POST => HttpMethod.Post,
                SD.ApiType.PUT => HttpMethod.Put,
                SD.ApiType.DELETE => HttpMethod.Delete,
                _ => HttpMethod.Get,
            };
            }
    }
}
