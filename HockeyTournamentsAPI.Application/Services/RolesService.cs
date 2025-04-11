using HockeyTournamentsAPI.Application.Interfaces;
using HockeyTournamentsAPI.Core.Models;
using HockeyTournamentsAPI.Database.PostgreSQL.Exceptions;
using HockeyTournamentsAPI.Database.PostgreSQL.Interfaces;

namespace HockeyTournamentsAPI.Application.Services
{
    public class RolesService : IRolesService
    {
        private readonly IRolesRepository _rolesRepository;

        public RolesService(IRolesRepository rolesRepository)
        {
            _rolesRepository = rolesRepository;
        }

        public async Task<Role?> GetRoleByNameAsync(string name)
        {
            try
            {
                var role = await _rolesRepository.GetRoleByName(name);

                return role;
            }
            catch (UnknownDbException ex)
            {
                throw;
            }
        }

        public async Task<Role> CreateRoleAsync(Role role)
        {
            var roleEntity = await _rolesRepository.CreateAsync(role);
            return roleEntity;
        }
    }
}
