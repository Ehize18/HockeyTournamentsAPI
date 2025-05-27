using HockeyTournamentsAPI.Core.Models;

namespace HockeyTournamentsAPI.Application.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetUserByIdAsync(Guid id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> ChangeRoleAsync(Guid id, Role role);
        Task<User> UpdateUserAsync(User user);
        Task<List<User>> GetUsersAsync(int ageFrom, int ageTo, int page, int pageSize, bool? gender, string orderBy, bool isAscending);
    }
}