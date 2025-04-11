using System.ComponentModel.DataAnnotations;

namespace HockeyTournamentsAPI.Application.Contracts.Auth
{
    public record LoginRequest([Required]string Email, [Required]string Password);
}
