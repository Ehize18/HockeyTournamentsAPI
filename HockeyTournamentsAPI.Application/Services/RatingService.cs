using HockeyTournamentsAPI.Application.Interfaces;
using HockeyTournamentsAPI.Core.Models;
using HockeyTournamentsAPI.Database.PostgreSQL.Interfaces;

namespace HockeyTournamentsAPI.Application.Services
{
    public class RatingService : IRatingService
    {
        private readonly ITournamentParticipantsRepository _participantsRepository;

        public RatingService(ITournamentParticipantsRepository participantsRepository)
        {
            _participantsRepository = participantsRepository;
        }

        public async Task RecalculateRating(List<Match> matches)
        {
            List<TournamentParticipant> participants = GetParticipants(matches);

            var matrix = new double[participants.Count, participants.Count + 1];

            matrix = FillTeammates(matrix, participants);

            matrix = GetAverageRatingEquation(matrix, participants);

            var answers = MatrixSolver.Solve(matrix);

            for (var i = 0; i < answers.Length; i++)
            {
                participants[i].RatingOnTournament = (int)answers[i];
            }

            await _participantsRepository.UpdateRangeAsync(participants);
        }

        private List<TournamentParticipant> GetParticipants(List<Match> matches)
        {
            var participants = new List<TournamentParticipant>();

            foreach (var match in matches)
            {
                foreach (var team in match.Teams)
                {
                    foreach (var member in team.Members)
                    {
                        if (!participants.Contains(member.Participant))
                        {
                            member.Participant.MatchPlayedInTour = new List<Match> { match };
                            participants.Add(member.Participant);
                        }
                        else
                        {
                            member.Participant.MatchPlayedInTour.Add(match);
                        }
                    }
                }
            }

            return participants;
        }

        private double[,] FillTeammates(double[,] matrix, List<TournamentParticipant> participants)
        {
            var defaultCounts = new Dictionary<Guid, Pair>();

            var membersInTeam = participants[0].MatchPlayedInTour[0].Teams[0].Members.Count;

            for (var i = 0; i < participants.Count; i++)
            {
                defaultCounts.Add(participants[i].Id, new Pair
                {
                    OpponentCount = 0,
                    MateCount = 0,
                    Number = i
                });
            }

            var row = 0;

            foreach (var participant in participants)
            {
                var playedCounts = defaultCounts.ToDictionary(e => e.Key,
                    e => new Pair() { Number = e.Value.Number, MateCount = e.Value.MateCount, OpponentCount = e.Value.OpponentCount });


                var goals = 0;
                var missed = 0;


                foreach (var match in participant.MatchPlayedInTour)
                {
                    foreach (var team in match.Teams)
                    {
                        var isMates = false;

                        if (team.Members.Any(m => m.Participant.Id == participant.Id))
                        {
                            isMates = true;
                            goals += team.Goals;
                        }
                        else
                        {
                            missed += team.Goals;
                        }

                        foreach (var member in team.Members)
                        {
                            if (isMates)
                            {
                                playedCounts[member.Participant.Id].MateCount++;
                            }
                            else
                            {
                                playedCounts[member.Participant.Id].OpponentCount++;
                            }
                        }
                    }
                }

                foreach (var id in playedCounts.Keys)
                {
                    matrix[row, playedCounts[id].Number] = GetPartOfEquation(playedCounts[id], membersInTeam);
                }

                matrix[row, participants.Count] = 1000.0 * (goals - missed) / (goals + missed);

                row++;
            }

            return matrix;
        }

        private double GetPartOfEquation(Pair counts, int membersInTeam)
        {
            return (counts.MateCount - counts.OpponentCount) / (double)membersInTeam;
        }

        private double[,] GetAverageRatingEquation(double[,] matrix, List<TournamentParticipant> participants)
        {
            var averageRating = participants.Average(p => p.RatingOnTournament);

            for (var column = 0; column < participants.Count; column++)
            {
                matrix[participants.Count - 1, column] = 1.0 / participants.Count;
            }
            matrix[participants.Count - 1, participants.Count] = averageRating;

            return matrix;
        }
    }

    internal class Pair
    {
        public int Number { get; set; }
        public int MateCount { get; set; }
        public int OpponentCount { get; set; }
    }
}
