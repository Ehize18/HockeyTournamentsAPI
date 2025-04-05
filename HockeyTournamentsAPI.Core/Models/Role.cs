namespace HockeyTournamentsAPI.Core.Models
{
    /// <summary>
    /// Роль пользователя.
    /// </summary>
    public class Role : BaseModel
    {
        /// <summary>
        /// Название.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Матрица разрешений.
        /// </summary>
        public RolePermissions Permissions { get; set; }

        /// <summary>
        /// Список пользователей с ролью.
        /// </summary>
        public IList<User> Users { get; set; }
    }
}
