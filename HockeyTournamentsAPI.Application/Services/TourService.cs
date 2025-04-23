using System.Runtime.CompilerServices;
using HockeyTournamentsAPI.Application.Interfaces;
using HockeyTournamentsAPI.Core.Models;
using HockeyTournamentsAPI.Database.PostgreSQL.Interfaces;

namespace HockeyTournamentsAPI.Application.Services
{
    public class TourService : ITourService
    {
        private readonly IToursRepository _toursRepository;
        private readonly IMatchesRepository _matchesRepository;
        private readonly ITeamsRepository _teamsRepository;

        public TourService(IToursRepository toursRepository,
            IMatchesRepository matchesRepository, ITeamsRepository teamsRepository)
        {
            _toursRepository = toursRepository;
            _matchesRepository = matchesRepository;
            _teamsRepository = teamsRepository;
        }

        private List<TournamentParticipant> GetActiveParticipants(Tournament tournament)
        {
            return tournament.Participants.Where(p => p.IsAccepted && !p.IsKicked).ToList();
        }

        private List<TournamentParticipant> FindOpponents(List<TournamentParticipant> participants)
        {
            participants.Sort((p1, p2) => p1.RatingOnTournament - p2.RatingOnTournament);

            for (int i = 0; i < participants.Count; i++)
            {
                if (i == 0)
                {
                    participants[i].Opponent = participants[i + 1];
                    continue;
                }
                if (i == participants.Count - 1)
                {
                    participants[i].Opponent = participants[i - 1];
                    continue;
                }
                var previous = participants[i - 1];
                var next = participants[i + 1];

                if ((previous.RatingOnTournament - participants[i].RatingOnTournament) >
                    participants[i].RatingOnTournament - next.RatingOnTournament)
                {
                    participants[i].Opponent = next;
                }
                else
                {
                    participants[i].Opponent = previous;
                }
            }

            return participants;
        }

        public List<Tour> CreateTours(Tournament tournament, int toursCount, int teamMemberCount)
        {
            var tourParticipants = tournament.Participants.Where(p => !p.IsKicked && p.IsAccepted).ToList();

            if (tournament.Tours == null)
            {
                tournament.Tours = new List<Tour>();
            }

            for (var i = 0; i < toursCount; i++)
            {
                var tour = CreateTour(tournament, tourParticipants, teamMemberCount);
                tournament.Tours.Add(tour);
            }

            return tournament.Tours;
        }

        private void PrepareParticipantsForTour(List<TournamentParticipant> participants)
        {
            foreach (var participant in participants)
            {
                participant.CanPlay = true;
                participant.GamesInRow = 0;
            }
        }

        private Tour CreateTour(Tournament tournament, List<TournamentParticipant> participants, int teamMemberCount)
        {
            PrepareParticipantsForTour(participants);

            participants = FindOpponents(participants);

            foreach (var participant in participants)
            {
                participant.CanPlay = true;
            }

            var tour = new Tour();

            tour.ParticipantsCount = participants.Count;

            tour.Participants = participants;

            tour = CreateMatches(tour, teamMemberCount);

            return tour;
        }

        private Tour CreateMatches(Tour tour, int teamMemberCount)
        {
            var participantsWithNotPlayed = LoadNotPlayed(tour.Participants);

            if (tour.Matches == null)
            {
                tour.Matches = new List<Match>();
            }

            while (participantsWithNotPlayed.Count > 0)
            {
                var pivotPlayer = GetPivotPlayer(participantsWithNotPlayed);
                var match = CreateMatch(pivotPlayer, teamMemberCount, tour.Participants);
                tour.Matches.Add(match);
                participantsWithNotPlayed = UpdateNotPlayed(participantsWithNotPlayed);
                UpdateGamesInRow(tour);
            }
            return tour;
        }

        private List<TournamentParticipant> UpdateNotPlayed(List<TournamentParticipant> participants)
        {
            var newList = new List<TournamentParticipant>();

            foreach (var participant in participants)
            {
                if (participant.NotPlayedParticipants.Count > 0)
                {
                    newList.Add(participant);
                }
            }
            return newList;
        }

        private void UpdateGamesInRow(Tour tour)
        {
            if (tour.Matches.Count < 2)
            {
                return;
            }
            var lastTwoMathces = tour.Matches.Skip(tour.Matches.Count - 2).Take(2);

            foreach (var participant in tour.Participants)
            {
                if (lastTwoMathces.Any(m => m.Teams.Any(t => t.Members.Any(m => m.Participant.Id == participant.Id))))
                {
                    continue;
                }
                participant.GamesInRow = 0;
                participant.CanPlay = true;
            }
        }

        private Match CreateMatch(TournamentParticipant pivotPlayer, int teamMemberCount, List<TournamentParticipant> allParticipants)
        {
            var match = new Match()
            {
                Teams = new List<Team>
                {
                    new Team()
                    {
                        Members = new List<TeamMember>()
                    },
                    new Team()
                    {
                        Members = new List<TeamMember>()
                    }
                }
            };

            match.Teams[0].Members.Add(new TeamMember
            {
                Participant = pivotPlayer
            });
            match.Teams[1].Members.Add(new TeamMember
            {
                Participant = pivotPlayer.Opponent
            });
            pivotPlayer.Opponent.CanPlay = false;

            for (var i = 1; i < teamMemberCount; i++)
            {
                var opponent = GetOpponent(pivotPlayer, allParticipants);
                if (opponent == null)
                {
                    var test = 1;
                }
                match.Teams[1].Members.Add(new TeamMember
                {
                    Participant = opponent
                });
                match.Teams[0].Members.Add(new TeamMember
                {
                    Participant = opponent.Opponent
                });
                opponent.CanPlay = false;
                opponent.Opponent.CanPlay = false;

                pivotPlayer.NotPlayedParticipants.Remove(opponent);
                pivotPlayer.NotPlayedParticipants.Remove(opponent.Opponent);

                foreach (var team in match.Teams)
                {
                    foreach (var member in team.Members)
                    {
                        opponent.NotPlayedParticipants.Remove(member.Participant);
                        opponent.Opponent.NotPlayedParticipants.Remove(member.Participant);
                        member.Participant.NotPlayedParticipants.Remove(opponent);
                        member.Participant.NotPlayedParticipants.Remove(opponent.Opponent);
                    }
                }
            }

            SetCanPlay(match);

            UpdateGamesInRow(match);

            return match;
        }

        private void SetCanPlay(Match match)
        {
            foreach (var team in match.Teams)
            {
                foreach (var member in team.Members)
                {
                    member.Participant.CanPlay = true;
                }
            }
        }

        private void UpdateGamesInRow(Match match)
        {
            foreach (var team in match.Teams)
            {
                foreach (var member in team.Members)
                {
                    member.Participant.GamesInRow++;
                    if (member.Participant.GamesInRow == 3)
                    {
                        member.Participant.CanPlay = false;
                    }
                }
            }
        }

        private TournamentParticipant? GetOpponent(TournamentParticipant pivotPlayer, List<TournamentParticipant> participants)
        {
            var availableOpponents = new List<TournamentParticipant>();

            foreach (var opponent in pivotPlayer.NotPlayedParticipants)
            {
                if (opponent.CanPlay && opponent.Opponent.CanPlay)
                {
                    availableOpponents.Add(opponent);
                }
            }

            foreach (var opponent in pivotPlayer.NotPlayedParticipants)
            {
                if (opponent.CanPlay && opponent.Opponent.CanPlay)
                {
                    return opponent;
                }
            }

            foreach (var participant in participants)
            {
                if (participant.CanPlay && participant.Opponent.CanPlay)
                {
                    return participant;
                }
            }
            return null;
        }

        private List<TournamentParticipant> LoadNotPlayed(List<TournamentParticipant> participants)
        {
            foreach (var participant in participants)
            {
                participant.NotPlayedParticipants = new List<TournamentParticipant>();
                participant.NotPlayedParticipants
                    .AddRange(participants.Where(p => p.Id != participant.Id).ToList());
            }
            var result = new List<TournamentParticipant>(participants);

            return participants;
        }

        private TournamentParticipant GetPivotPlayer(List<TournamentParticipant> participants)
        {
            var pivot = participants.OrderByDescending(p => p.NotPlayedParticipants.Count).FirstOrDefault();

            return pivot;
        }
    }
}