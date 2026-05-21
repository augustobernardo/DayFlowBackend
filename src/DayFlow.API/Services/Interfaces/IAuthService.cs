using DayFlow.API.Models.DTOs;
using DayFlow.API.Models.Requests;

namespace DayFlow.API.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterRequest request);
    Task<AuthResponseDto> LoginAsync(LoginRequest request);
}
