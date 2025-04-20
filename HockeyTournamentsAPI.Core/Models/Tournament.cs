namespace HockeyTournamentsAPI.Core.Models
{
    public class Tournament : BaseModel
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}