using HockeyTournamentsAPI.Core.Models;

namespace HockeyTournamentsAPI.Database.PostgreSQL.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : BaseModel
    {
        Task<TEntity> CreateAsync(TEntity entity);
        Task<bool> DeleteAsync(Guid id);
        Task<IList<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(Guid id);
        Task SaveChangesAsync();
        Task<TEntity> UpdateAsync(TEntity entity);
    }
}