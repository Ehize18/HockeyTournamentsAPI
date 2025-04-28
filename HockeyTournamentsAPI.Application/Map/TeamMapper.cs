using HockeyTournamentsAPI.Application.Contracts.Members;
using HockeyTournamentsAPI.Application.Contracts.Teams;
using HockeyTournamentsAPI.Core.Models;

namespace HockeyTournamentsAPI.Application.Map
{
    public static class TeamMapper
    {
        public static TeamResponse ToResponse(this Team team)
        {
            var membersResponses = new List<MemberResponse>();

            foreach (var member in team.Members)
            {
                membersResponses.Add(member.ToResponse());
            }

            return new TeamResponse(team.Id, membersResponses);
        }
    }
}
