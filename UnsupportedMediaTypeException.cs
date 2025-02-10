namespace dotnet_client
{
    public class UnsupportedMediaTypeException : Exception
    {
        public UnsupportedMediaTypeException(string message)
            : base(message) { }

        public UnsupportedMediaTypeException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
