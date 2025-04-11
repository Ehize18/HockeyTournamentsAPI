using HockeyTournamentsAPI.Application.Contracts.Auth;

namespace HockeyTournamentsAPI.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> LoginUser(LoginRequest request);
        Task<bool> RegisterUser(RegisterRequest request);
    }
}