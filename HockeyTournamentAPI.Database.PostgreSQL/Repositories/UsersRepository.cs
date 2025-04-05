using HockeyTournamentsAPI.Database.PostgreSQL.Interfaces;
using HockeyTournamentsAPI.Core.Models;

namespace HockeyTournamentsAPI.Database.PostgreSQL.Repositories
{
    public class UsersRepository : BaseRepository<User>, IUsersRepository
    {
        public UsersRepository(HockeyTournamentsDbContext context)
            : base(context) { }
    }
}
