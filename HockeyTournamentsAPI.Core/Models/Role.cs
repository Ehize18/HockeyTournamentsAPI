using System.ComponentModel;

namespace HockeyTournamentsAPI.Core.Models
{
    [Flags]
    public enum Role
    {
        [Description("Пользователь")]
        User,

        [Description("Супер пользователь")]
        Supervisor,

        [Description("Администратор")]
        Administrator,

        [Description("Тренер")]
        Trainer,

        [Description("Судья")]
        Judge
    }
}
