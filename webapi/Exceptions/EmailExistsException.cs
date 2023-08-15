namespace webapi.Exceptions
{
    public class EmailExistsException : Exception
    {
        public EmailExistsException(string message) : base(message)
        {
            
        }
    }
}
