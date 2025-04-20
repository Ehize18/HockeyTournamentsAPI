using HockeyTournamentsAPI.Application.Contracts.Auth;
using HockeyTournamentsAPI.Application.Interfaces;
using HockeyTournamentsAPI.Core.Models;
using HockeyTournamentsAPI.Database.PostgreSQL.Interfaces;
using HockeyTournamentsAPI.Infrastructure.Hash;
using HockeyTournamentsAPI.Infrastructure.Jwt.Interfaces;

namespace HockeyTournamentsAPI.Application.Services
{
    public class AuthService : IAuthService
    {
        /// <summary>
        /// Репозиторий пользователей.
        /// </summary>
        private readonly IUsersRepository _usersRepository;

        /// <summary>
        /// Провайдер JWT токенов.
        /// </summary>
        private readonly IJwtProvider _jwtProvider;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="usersRepository">Репозиторий пользователей.</param>
        /// <param name="jwtProvider">Провайдер JWT токенов.</param>
        public AuthService(IUsersRepository usersRepository, IJwtProvider jwtProvider)
        {
            _usersRepository = usersRepository;
            _jwtProvider = jwtProvider;
        }

        public async Task<bool> RegisterUser(RegisterRequest request)
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                LastName = request.LastName,
                BirthDate = request.BirthDate,
                IsMale = request.Gender,
                Email = request.Email,
                Phone = request.Phone,
                SportLevel = request.SportLevel,
                Role = Role.User,
                PasswordHash = Hash.SHA256Hash(request.Password),
            };

            try
            {
                var entity = await _usersRepository.CreateAsync(user);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<string> LoginUser(LoginRequest request)
        {
            var user = await _usersRepository.GetByEmailAsync(request.Email);

            var requestPasswordHash = Hash.SHA256Hash(request.Password);

            if (user != null &&
                user.PasswordHash == requestPasswordHash)
            {
                var userRole = user.Role.ToString();
                var token = _jwtProvider.GenerateToken(request.Email, userRole);
                return token;
            }
            return string.Empty;
        }
    }
}
