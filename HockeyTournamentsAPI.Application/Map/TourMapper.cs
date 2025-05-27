using HockeyTournamentsAPI.Application.Contracts.Tours;
using HockeyTournamentsAPI.Core.Models;

namespace HockeyTournamentsAPI.Application.Map
{
    public static class TourMapper
    {
        public static TourResponse ToResponse(this Tour tour)
        {
            return new TourResponse(tour.Id, tour.Matches.Count, tour.Participants.Count, tour.StartTime, tour.EndTime);
        }

        public static List<TourResponse> ToResponse(this List<Tour> tours)
        {
            var responseList = new List<TourResponse>();

            foreach (var tour in tours)
            {
                responseList.Add(tour.ToResponse());
            }
            return responseList;
        }
    }
}
