namespace DP_backend.Common.Exceptions
{
    [Serializable]
    public class NoPermissionException : Exception
    {
        public NoPermissionException() { }

        public NoPermissionException(string message) : base(message) { }

        public NoPermissionException(string message, Exception innerException) : base(message, innerException) { }
    }
}
