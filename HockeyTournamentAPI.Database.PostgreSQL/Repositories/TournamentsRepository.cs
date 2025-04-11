using HockeyTournamentsAPI.Core.Models;

namespace HockeyTournamentsAPI.Database.PostgreSQL.Repositories
{
    public class TournamentsRepository : BaseRepository<Tournament>
    {
        public TournamentsRepository(HockeyTournamentsDbContext context)
            : base(context) { }
    }
}
