namespace DP_backend.Models.Exceptions
{
    [Serializable]
    public class BadDataException : Exception
    {
        public BadDataException() { }

        public BadDataException(string message) : base(message) { }

        public BadDataException(string message, Exception innerException) : base(message, innerException) { }
    }
}
