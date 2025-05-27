namespace HockeyTournamentsAPI.Application.Contracts.Users
{
    /// <summary>
    /// Фильтры запроса на получение всех пользователей
    /// </summary>
    /// <param name="AgeFrom">Возраст от</param>
    /// <param name="AgeTo">Возраст до</param>
    /// <param name="Page">Страница</param>
    /// <param name="PageSize">Размер страниц</param>
    /// <param name="Gender">Гендер, true - мужчина, fals - женщина</param>
     public record UsersRequest(
         int AgeFrom = 0, int AgeTo = 200, int Page = 1, int PageSize = int.MaxValue, bool? Gender = null
         );
}
