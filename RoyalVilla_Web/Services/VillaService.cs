using RoyalVilla_DTO;
using RoyalVilla_Web.Models;
using RoyalVilla_Web.Services.IServices;

namespace RoyalVilla_Web.Services
{
    public class VillaService :BaseService, IVillaService
    {
        private readonly string _villaApiEndPoint;

        public VillaService(IHttpClientFactory httpClient) : base(httpClient)
        {
            _villaApiEndPoint = "/api/Villa";
        }

        public Task<T?> CreateAsync<T>(VillaCreateDTO villaCreateDTO, string? token = null)
        {
            return SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = villaCreateDTO,
                Url =_villaApiEndPoint,
                AccessToken = token
            });
        }

        public Task<T?> DeleteAsync<T>(int id, string? token = null)
        {
            return SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = $"{_villaApiEndPoint}/{id}",
                AccessToken = token
            });
        }

        public Task<T?> GetAllAsync<T>(string? token = null)
        {
            return SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = $"{_villaApiEndPoint}",
                AccessToken = token
            });
        }

        public Task<T?> GetAsync<T>(int id, string? token = null)
        {
            return SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = $"{_villaApiEndPoint}/{id}",
                AccessToken = token
            });
        }

        public Task<T?> UpdateAsync<T>(VillaUpdateDTO villaUpdateDTO, string? token = null)
        {
            return SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = villaUpdateDTO,
                Url = $"{_villaApiEndPoint}/{villaUpdateDTO.Id}",
                AccessToken = token
            });
        }
    }
}
