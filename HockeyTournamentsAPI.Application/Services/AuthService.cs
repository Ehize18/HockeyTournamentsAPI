using HockeyTournamentsAPI.Application.Contracts.Auth;
using HockeyTournamentsAPI.Application.Interfaces;
using HockeyTournamentsAPI.Core.Models;
using HockeyTournamentsAPI.Database.PostgreSQL.Interfaces;
using HockeyTournamentsAPI.Infrastructure.Hash;
using HockeyTournamentsAPI.Infrastructure.Jwt.Interfaces;

namespace HockeyTournamentsAPI.Application.Services
{
    public class AuthService
    {
        /// <summary>
        /// Репозиторий пользователей.
        /// </summary>
        private readonly IUsersRepository _usersRepository;

        private readonly IRolesService _rolesService;

        /// <summary>
        /// Провайдер JWT токенов.
        /// </summary>
        private readonly IJwtProvider _jwtProvider;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="usersRepository">Репозиторий пользователей.</param>
        /// <param name="jwtProvider">Провайдер JWT токенов.</param>
        public AuthService(IUsersRepository usersRepository, IRolesService rolesService, IJwtProvider jwtProvider)
        {
            _usersRepository = usersRepository;
            _rolesService = rolesService;
            _jwtProvider = jwtProvider;
        }

        public async Task<User> RegisterUser(AuthRequest request)
        {
            var userRole = await _rolesService.GetRoleByNameAsync("Пользователь");

            if (userRole == null)
            {

            }

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
                PasswordHash = Hash.SHA256Hash(request.Password),
            };

            try
            {
                var entity = await _usersRepository.CreateAsync(user);
            }
        }
    }
}
