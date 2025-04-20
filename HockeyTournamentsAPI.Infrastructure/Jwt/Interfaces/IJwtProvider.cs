namespace HockeyTournamentsAPI.Infrastructure.Jwt.Interfaces
{
    public interface IJwtProvider
    {
        string GenerateToken(string email, string role);
    }
}