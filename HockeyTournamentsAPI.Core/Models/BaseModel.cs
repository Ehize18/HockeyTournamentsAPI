namespace HockeyTournamentsAPI.Core.Models
{
    /// <summary>
    /// Базовая модель для сущностей.
    /// </summary>
    public abstract class BaseModel
    {
        /// <summary>
        /// ID.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Дата создания.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Дата изменения.
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}
