using HockeyTournamentsAPI.Core.Models;
using HockeyTournamentsAPI.Database.PostgreSQL.Interfaces;

namespace HockeyTournamentsAPI.Database.PostgreSQL.Repositories
{
    public class MatchesRepository : BaseRepository<Match>, IMatchesRepository
    {
        public MatchesRepository(HockeyTournamentsDbContext context) : base(context)
        {
        }
    }
}
