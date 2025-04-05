using System.ComponentModel.DataAnnotations;

namespace HockeyTournamentsAPI.Application.Contracts.Auth
{
    public record AuthRequest(
        [Required] string FirstName, string? MiddleName, [Required] string LastName,
        [Required] DateOnly BirthDate, [Required] bool Gender,
        [EmailAddress] string Email, [Required][Range(11, 11)] string Phone, [Required] string Password,
        string SportLevel);
}
