using HockeyTournamentsAPI.Application.Contracts.Users;
using HockeyTournamentsAPI.Core.Models;

namespace HockeyTournamentsAPI.Application.Map
{
    public static class UserMapper
    {
        public static UserResponse ToResponse(this User user)
        {
            var response = new UserResponse(
                user.Id,
                user.FirstName, user.MiddleName, user.LastName,
                user.BirthDate, user.IsMale ? "Male" : "Female",
                user.Email, user.Phone,
                user.SportLevel,
                user.Role.ToResponse(),
                user.TrainerId,
                user.Rating);
            return response;
        }
    }
}
