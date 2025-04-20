using HockeyTournamentsAPI.Application.Contracts.Roles;

namespace HockeyTournamentsAPI.Application.Contracts.Users
{
    public record UserResponse(
        Guid Id,
        string FirstName, string? MiddleName, string LastName,
        DateOnly BirthDate, string Gender,
        string Email, string Phone,
        string SportLevel,
        RoleResponse Role,
        Guid? TrainerId);
}
