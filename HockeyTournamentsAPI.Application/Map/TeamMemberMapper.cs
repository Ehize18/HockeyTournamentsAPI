using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HockeyTournamentsAPI.Application.Contracts.Members;
using HockeyTournamentsAPI.Core.Models;

namespace HockeyTournamentsAPI.Application.Map
{
    public static class TeamMemberMapper
    {
        public static MemberResponse ToResponse(this TeamMember teamMember)
        {
            return new MemberResponse(teamMember.Id,
                teamMember.Participant.User.Id,
                teamMember.Participant.User.LastName,
                teamMember.Participant.User.FirstName,
                teamMember.Participant.User.MiddleName,
                teamMember.Participant.RatingOnTournament);
        }
    }
}
