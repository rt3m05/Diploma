namespace webapi.Exceptions
{
    public class EmptyModelException : Exception
    {
        public EmptyModelException(string message) : base(message) { }
    }
}
