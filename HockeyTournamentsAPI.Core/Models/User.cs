namespace HockeyTournamentsAPI.Core.Models
{
    /// <summary>
    /// Пользователь.
    /// </summary>
    public class User : BaseModel
    {
        /// <summary>
        /// Имя.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Отчество (при наличии).
        /// </summary>
        public string? MiddleName { get; set; }

        /// <summary>
        /// Фамилия.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Дата рождения.
        /// </summary>
        public DateOnly BirthDate { get; set; }

        /// <summary>
        /// Пользователь мужчина?
        /// Гендер.
        /// </summary>
        public bool IsMale { get; set; }

        /// <summary>
        /// Email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Номер телефона.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Хэш пароля.
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// Уровень подготовки.
        /// </summary>
        public string SportLevel { get; set; }

        /// <summary>
        /// Рейтинг.
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// Id телеграма.
        /// </summary>
        public int? TelegramId { get; set; }

        /// <summary>
        /// Роль пользователя.
        /// </summary>
        public Role Role { get; set; }

        /// <summary>
        /// Тренер игрока.
        /// </summary>
        public User? Trainer { get; set; }

        /// <summary>
        /// Id тренера игрока.
        /// </summary>
        public Guid? TrainerId { get; set; }

        /// <summary>
        /// Ученики тренера.
        /// </summary>
        public IList<User> Students { get; set; }
    }
}
