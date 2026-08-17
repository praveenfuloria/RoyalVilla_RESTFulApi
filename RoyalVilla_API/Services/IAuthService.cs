using RoyalVilla_DTO;

namespace RoyalVilla_API.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDTO> LoginAsync(LoginRequestDTO loginRequestDTO);
        Task<UserDTO> RegisterAsync(RegistrationRequestDTO registrationRequestDTO);

        Task<bool> IsEmailExistsAsync(string email);

    }
}
