namespace HockeyTournamentsAPI.Core.Models
{
    public class TournamentParticipant : BaseModel
    {
        public User User { get; set; }
        public Guid UserId { get; set; }
        public Tournament Tournament { get; set; }
        public Guid TournamentId { get; set; }
        public bool IsAccepted { get; set; }
        public int RatingOnTournament { get; set; }

        public bool IsKicked { get; set; }

        public TournamentParticipant Opponent { get; set; }

        public int GamesInRow { get; set; }
        public bool CanPlay { get; set; }

        public List<TournamentParticipant> NotPlayedParticipants { get; set; }

        public List<TournamentParticipant> AvailableOpponents { get; set; }

        public int Age { get; set; }

        public List<Match> MatchPlayedInTour { get; set; }

        public int ToursPlayed { get; set; }
    }
}
