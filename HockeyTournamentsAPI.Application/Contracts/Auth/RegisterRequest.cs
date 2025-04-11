using System.ComponentModel.DataAnnotations;

namespace HockeyTournamentsAPI.Application.Contracts.Auth
{
    public record RegisterRequest(
        [Required] string FirstName, string? MiddleName, [Required] string LastName,
        [Required] DateOnly BirthDate, [Required] bool Gender,
        [EmailAddress] string Email, [Required][StringLength(11, MinimumLength = 11)] string Phone, [Required] string Password,
        string SportLevel);
}
