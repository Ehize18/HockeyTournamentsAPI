using HockeyTournamentsAPI.Application.Interfaces;
using HockeyTournamentsAPI.Core.Models;
using HockeyTournamentsAPI.Database.PostgreSQL.Interfaces;

namespace HockeyTournamentsAPI.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUsersRepository _usersRepository;

        public UserService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            var user = await _usersRepository.GetByIdAsync(id);
            return user;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var user = await _usersRepository.GetByEmailAsync(email);
            return user;
        }

        public async Task<bool> ChangeRoleAsync(Guid id, Role role)
        {
            var user = await _usersRepository.GetByIdAsync(id);

            if (user == null)
            {
                return false;
            }

            user.Role = role;

            try
            {
                await _usersRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}
