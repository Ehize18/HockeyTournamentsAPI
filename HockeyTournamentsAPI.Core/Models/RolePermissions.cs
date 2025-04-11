namespace HockeyTournamentsAPI.Core.Models
{
    /// <summary>
    /// Матрица разрешений роли.
    /// </summary>
    [Flags]
    public enum RolePermissions
    {
        None = 0,
        /// <summary>
        /// Возможность добавлять роли.
        /// </summary>
        AddRoles = 1,

        /// <summary>
        /// Возможность создавать турниры.
        /// </summary>
        CreateTournaments = 2,

        /// <summary>
        /// Возможность судить матчи.
        /// </summary>
        JudgingMatches = 4,

       // All = -1
    }
}
