using RoyalVilla_DTO;

namespace RoyalVilla_Web.Services.IServices
{
    public interface IVillaService
    {
        Task<T?> GetAllAsync<T>(string? token = null);
        Task<T?> GetAsync<T>(int id, string? token = null);
        Task<T?> CreateAsync<T>(VillaCreateDTO villaCreateDTO, string? token = null);
        Task<T?> UpdateAsync<T>(VillaUpdateDTO villaUpdateDTO, string? token = null);
        Task<T?> DeleteAsync<T>(int id, string? token = null);
    }
}
