using HockeyTournamentsAPI.Database.PostgreSQL.Exceptions;
using HockeyTournamentsAPI.Database.PostgreSQL.Interfaces;
using HockeyTournamentsAPI.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace HockeyTournamentsAPI.Database.PostgreSQL.Repositories
{
    /// <summary>
    /// Базовый класс для репозиториев.
    /// </summary>
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseModel
    {
        /// <summary>
        /// Контекст базы данных.
        /// </summary>
        protected readonly HockeyTournamentsDbContext _context;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="context">Контекст базы данных.</param>
        public BaseRepository(HockeyTournamentsDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получает сущность по Id.
        /// </summary>
        /// <param name="id">Id сущности.</param>
        /// <returns>Сущность.</returns>
        /// <exception cref="EntityNotFoundException"></exception>
        /// <exception cref="UnknownDbException"></exception>
        public async Task<TEntity?> GetByIdAsync(Guid id)
        {
            try
            {
                var entity = await _context.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id);

                return entity;
            }
            catch (Exception ex)
            {
                throw new UnknownDbException(id.ToString(), ex);
            }
        }

        /// <summary>
        /// Получает все сущности.
        /// </summary>
        /// <returns>Список сущностей.</returns>
        /// <exception cref="UnknownDbException"></exception>
        public async Task<IList<TEntity>> GetAllAsync()
        {
            try
            {
                return await _context.Set<TEntity>().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new UnknownDbException($"{ex.Message}", ex);
            }
        }

        /// <summary>
        /// Создаёт запись сущности в таблице.
        /// </summary>
        /// <param name="entity">Сущность.</param>
        /// <returns>Сохранённая сущность.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="UnknownDbException"></exception>
        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.Set<TEntity>().AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new UnknownDbException($"{ex.Message}", ex);
            }
        }

        /// <summary>
        /// Обновляет сущность.
        /// </summary>
        /// <param name="entity">Сущность.</param>
        /// <returns>Обновлённая сущность.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="UnknownDbException"></exception>
        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            entity.UpdatedAt = DateTime.UtcNow;

            try
            {
                _context.Set<TEntity>().Update(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new UnknownDbException($"{ex.Message}", ex);
            }
        }

        public async Task<List<TEntity>> UpdateRangeAsync(List<TEntity> entities)
        {
            try
            {
                _context.Set<TEntity>().UpdateRange(entities);
                await _context.SaveChangesAsync();
                return entities;
            }
            catch (Exception ex)
            {
                throw new UnknownDbException($"{ex.Message}", ex);
            }
        }

        /// <summary>
        /// Сохраняет все изменения.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="UnknownDbException"></exception>
        public async Task SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new UnknownDbException($"{ex.Message}", ex);
            }
        }

        /// <summary>
        /// Удаляет сущность по Id.
        /// </summary>
        /// <param name="id">Id сущности</param>
        /// <returns>True если сущность удалена.</returns>
        /// <exception cref="UnknownDbException"></exception>
        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                var entity = await GetByIdAsync(id);
                _context.Set<TEntity>().Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (UnknownDbException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new UnknownDbException($"{ex.Message}", ex);
            }

        }
    }
}
