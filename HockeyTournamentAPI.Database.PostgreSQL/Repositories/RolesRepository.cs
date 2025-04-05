using HockeyTournamentsAPI.Database.PostgreSQL.Interfaces;
using HockeyTournamentsAPI.Core.Models;
using Microsoft.EntityFrameworkCore;
using HockeyTournamentsAPI.Database.PostgreSQL.Exceptions;

namespace HockeyTournamentsAPI.Database.PostgreSQL.Repositories
{
    public class RolesRepository : BaseRepository<Role>, IRolesRepository
    {
        public RolesRepository(HockeyTournamentsDbContext context)
            : base(context) { }

        public async Task<Role> GetRoleByName(string name)
        {
            try
            {
                var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == name);

                if (role == null)
                {
                    throw new EntityNotFoundException(name);
                }
                return role;
            }
            catch (Exception ex)
            {
                throw new UnknownDbException(name, ex);
            }
        }
    }
}