namespace HockeyTournamentsAPI.Database.PostgreSQL.Exceptions
{
    /// <summary>
    /// Исключение, возникающее когда сущность не найдена.
    /// </summary>
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException() { }

        public EntityNotFoundException(string message)
            : base(message) { }

        public EntityNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
