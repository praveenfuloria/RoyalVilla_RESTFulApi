using RoyalVilla_DTO;
using RoyalVilla_Web.Models;

namespace RoyalVilla_Web.Services.IServices
{
    public interface IBaseServices
    {
        ApiResponse<object> ResponseModel { get; set; }
        Task<T?> SendAsync<T>(ApiRequest apiRequest);
    }
}
