namespace HockeyTournamentsAPI.Core.Models
{
    public class Match : BaseModel
    {
        public Tour Tour { get; set; }
        public Guid TourId { get; set; }

        public User Referee { get; set; }
        public Guid RefereeId { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public List<Team> Teams { get; set; }

        public bool IsLastMatchInTour { get; set; }
    }
}
