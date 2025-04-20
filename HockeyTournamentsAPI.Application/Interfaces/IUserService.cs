using HockeyTournamentsAPI.Core.Models;

namespace HockeyTournamentsAPI.Application.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetUserByIdAsync(Guid id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> ChangeRoleAsync(Guid id, Role role);
    }
}