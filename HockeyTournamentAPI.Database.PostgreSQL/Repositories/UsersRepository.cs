using HockeyTournamentsAPI.Database.PostgreSQL.Interfaces;
using HockeyTournamentsAPI.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace HockeyTournamentsAPI.Database.PostgreSQL.Repositories
{
    public class UsersRepository : BaseRepository<User>, IUsersRepository
    {
        public UsersRepository(HockeyTournamentsDbContext context)
            : base(context) { }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var entity = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == email);
            return entity;
        }

        public async Task<User?> GetSupervisorAsync()
        {
            var entity = await _context.Users
                .FirstOrDefaultAsync(x => x.Role == Role.Supervisor);
            return entity;
        }
    }
}
