using HockeyTournamentsAPI.Database.PostgreSQL.Interfaces;
using HockeyTournamentsAPI.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Linq.Dynamic.Core.Exceptions;

namespace HockeyTournamentsAPI.Database.PostgreSQL.Repositories
{
    public class UsersRepository : BaseRepository<User>, IUsersRepository
    {
        public UsersRepository(HockeyTournamentsDbContext context)
            : base(context) { }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var entity = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == email);
            return entity;
        }

        public async Task<User?> GetSupervisorAsync()
        {
            var entity = await _context.Users
                .FirstOrDefaultAsync(x => x.Role == Role.Supervisor);
            return entity;
        }

        public async Task<List<User>> GetUsersWithFiltrationAsync(DateOnly birthdayFrom, DateOnly birthdayTo, int page, int pageSize, bool? gender,
            string orderBy, bool isAscending)
        {
            IQueryable<User> query = _context.Users;

            if (gender is not null)
            {
                query = query.Where(u => (u.IsMale == gender) && (u.BirthDate < birthdayFrom) && (u.BirthDate > birthdayTo));
            }
            else
            {
                query = query.Where(u => (u.BirthDate < birthdayFrom) && (u.BirthDate > birthdayTo));
            }

            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            try
            {
                query = query.OrderBy($"{orderBy} {(isAscending ? "ascending" : "descending")}");
            }
            catch (ParseException ex)
            {
                throw new ArgumentException($"Can't order by column: {orderBy}");
            }

            return await query.ToListAsync();
        }

        public async Task<List<User>> GetReferees()
        {
            var referees = await _context.Users
                .Where(u => u.Role == Role.Judge)
                .ToListAsync();

            return referees;
        }
    }
}
