using HockeyTournamentsAPI.Application.Contracts.Participants;
using HockeyTournamentsAPI.Core.Models;

namespace HockeyTournamentsAPI.Application.Map
{
    public static class ParticipantMapper
    {
        public static TournamentParticipantResponse ToResponse(
            this TournamentParticipant participant,
            Tournament tournament, User user)
        {
            var response = new TournamentParticipantResponse(
                participant.Id,
                user.Id,
                user.FirstName, user.MiddleName, user.LastName,
                participant.IsAccepted,
                tournament.Id,
                tournament.Title
                );

            return response;
        }

        public static TournamentParticipantResponse ToResponse(
            this TournamentParticipant participant,
            Tournament tournament)
        {
            var response = new TournamentParticipantResponse(
                participant.Id,
                participant.User.Id,
                participant.User.FirstName, participant.User.MiddleName, participant.User.LastName,
                participant.IsAccepted,
                tournament.Id,
                tournament.Title
                );

            return response;
        }

        public static List<TournamentParticipantResponse> ToResponse(
            this List<TournamentParticipant> participants,
            Tournament tournament)
        {
            var response = new List<TournamentParticipantResponse>();

            foreach (var participant in participants)
            {
                response.Add(participant.ToResponse(tournament));
            }

            return response;
        }
    }
}
