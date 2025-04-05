namespace HockeyTournamentsAPI.Core.Models
{
    /// <summary>
    /// Матрица разрешений роли.
    /// </summary>
    [Flags]
    public enum RolePermissions
    {
        /// <summary>
        /// Возможность добавлять роли.
        /// </summary>
        AddRoles,

        /// <summary>
        /// Возможность создавать турниры.
        /// </summary>
        CreateTournaments,

        /// <summary>
        /// Возможность судить матчи.
        /// </summary>
        JudgingMatches
    }
}
