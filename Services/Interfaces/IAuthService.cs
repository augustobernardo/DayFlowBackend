using DayFlowAPI.Models.DTOs;
using DayFlowAPI.Models.Requests;

namespace DayFlowAPI.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterRequest request);
    Task<AuthResponseDto> LoginAsync(LoginRequest request);
}
