using HockeyTournamentsAPI.Application.Contracts.Members;

namespace HockeyTournamentsAPI.Application.Contracts.Teams
{
    public record TeamResponse(Guid TeamId, List<MemberResponse> Members);
}
