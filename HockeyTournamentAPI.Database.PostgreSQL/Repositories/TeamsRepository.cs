using HockeyTournamentsAPI.Core.Models;
using HockeyTournamentsAPI.Database.PostgreSQL.Interfaces;

namespace HockeyTournamentsAPI.Database.PostgreSQL.Repositories
{
    public class TeamsRepository : BaseRepository<Team>, ITeamsRepository
    {
        public TeamsRepository(HockeyTournamentsDbContext context) : base(context)
        {
        }

        public async Task AddMemberAsync(Team team, TeamMember member)
        {
            team.Members.Add(member);
            _context.Update(team);
        }
    }
}
