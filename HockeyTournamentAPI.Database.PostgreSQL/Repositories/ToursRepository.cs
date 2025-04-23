using HockeyTournamentsAPI.Core.Models;
using HockeyTournamentsAPI.Database.PostgreSQL.Interfaces;

namespace HockeyTournamentsAPI.Database.PostgreSQL.Repositories
{
    public class ToursRepository : BaseRepository<Tour>, IToursRepository
    {
        public ToursRepository(HockeyTournamentsDbContext context) : base(context)
        {
        }
    }
}
