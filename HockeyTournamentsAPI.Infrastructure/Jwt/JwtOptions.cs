using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace HockeyTournamentsAPI.Infrastructure.Jwt
{
    public class JwtOptions
    {
        public string SecretKey { get; set; }
        public int ExpiredHours { get; set; }
    }
}
