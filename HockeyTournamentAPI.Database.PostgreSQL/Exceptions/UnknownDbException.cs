namespace HockeyTournamentsAPI.Database.PostgreSQL.Exceptions
{
    /// <summary>
    /// Исключение, возникающее при неизвестной ошибке при работе с бд.
    /// </summary>
    public class UnknownDbException : Exception
    {
        public UnknownDbException() { }

        public UnknownDbException(string message)
            : base(message) { }

        public UnknownDbException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
