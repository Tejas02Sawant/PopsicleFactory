using PopsicleFactory.AuthApi.Entities;
using PopsicleFactory.AuthApi.Models;

namespace PopsicleFactory.AuthApi.Services;

public interface IAuthService
{
    Task<TokenResponseDto?> LoginAsync(UserDto request);
}
